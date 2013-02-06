using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Security;
using HtmlAgilityPack;
using HRE.Data;
using HRE.Models;
using HRE.Business;
using System.Diagnostics;
using System.Threading;
using HRE.Common;
using HRE.Dal;

namespace HRE.Controllers {

    public class InschrijvingenController : BaseController {

        public ActionResult Index() {
            ScrapeNtbIModel model = new ScrapeNtbIModel();
            model.Entries = InschrijvingenRepository.GetEntries(InschrijvingenRepository.HRE_EVENTNR);
            return View(model);
        }


        public const string NTBI_SESSION_CODE_KEY = "SessionCode";


        private string NtbISessionCode { 
            get {
                return HttpRuntime.Cache[NTBI_SESSION_CODE_KEY] as string;
            }
            set { 
                HttpRuntime.Cache[NTBI_SESSION_CODE_KEY] = value;
            }
        }

        /// <summary>
        /// The current selected serienumber.
        /// TODO BW 2012-12-24 Allow user to select serie from dropdown instead of fixed serie number.
        /// </summary>
        string CurrentSerieNr { 
            get {
                return InschrijvingenRepository.HRE_SERIENR;
            }
        }


        string CurrentEvenementNumber {
            get {
                return InschrijvingenRepository.HRE_EVENTNR;
            }
        }


        CookieCollection Cookies {get; set;}


        /// <summary>
        /// Non http post variant of Scrape, for case where the user session is timed out when user pushes Scrape button and is returned via returnUrl via GET request...
        /// This just redirects to index, the user will have to push the button again after succesfully logging in again.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        public ActionResult Scrape() {
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Scrape the inschrijvingen from NTB inschrijvingen.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult Scrape(ScrapeNtbIModel model) {
            try {
                int maxNumberOfItemsToScrape = model.MaxNumberOfScrapedItems;
                if (maxNumberOfItemsToScrape==0) {
                    maxNumberOfItemsToScrape = int.MaxValue;
                }

                // 0. Inloggen (sessie ID krijgen).
                // Posten naar https://mijn.triathlonbond.nl/default.asp */
                string ntbInschrijvingenStartUrl = "https://mijn.triathlonbond.nl/default.asp";

                ConsoleWrite(string.Format("Logging in via post to: {0}", ntbInschrijvingenStartUrl));

                // In form InlogFormulier invullen input met Id 'Gebruikersnaam' = SWW en input met Id 'wachtwoord' '.
                string gebruikersnaam = HreSettings.NtbIUsername;
                string wachtwoord = HreSettings.NtbIPassword;
                // Creates the post data for the POST request (fiddler: gebruikersnaam=SWW&wachtwoord=18jan2012&Inloggen=True&tijdverschil=-60)
                string postData = ("gebruikersnaam=" + gebruikersnaam + "&wachtwoord="+ wachtwoord+"&Inloggen=True&tijdverschil=-60");

                // Create the POST request.
                HttpWebRequest loginRequest = (HttpWebRequest) WebRequest.Create(ntbInschrijvingenStartUrl);
                loginRequest.Method = "POST";
            
                loginRequest.KeepAlive = true;
                loginRequest.ContentType = "application/x-www-form-urlencoded";
                loginRequest.ContentLength = postData.Length;
            
                // POST the data.
                using (StreamWriter requestWriter = new StreamWriter(loginRequest.GetRequestStream())) {
                    requestWriter.Write(postData);
                }

                string framesetUrl;

                // Post the request and get the response back.
                using (HttpWebResponse response = (HttpWebResponse) loginRequest.GetResponse()) {
                    
                    if (response.Cookies!=null && response.Cookies.Count>0) {
                        Cookies = response.Cookies;
                    }

                    // In het teruggegeven resultaat request heeft de URL dan een sessie code in de URL parameter 'SID'. 
                    // https://mijn.triathlonbond.nl/_interface/frameset.asp?SID={20B12A29-FFA4-4606-B912-5928181C7D4D}
                    // Deze gebruik je in stap 2.

                    framesetUrl = HttpUtility.UrlDecode(response.ResponseUri.AbsoluteUri);
                    ConsoleWrite(string.Format("Frame set URL (to open for manual editing stuff): {0}", framesetUrl));
                    NtbISessionCode = HttpUtility.ParseQueryString(response.ResponseUri.Query, Encoding.UTF8).Get("SID");
                    ConsoleWrite(string.Format("Session ID: {0}", NtbISessionCode));
                    response.Close();
                }

                // I. Overzichtscherm. Voorbeeld URL: https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/inschrijvingen_serie_individueel.asp?SID={20B12A29-FFA4-4606-B912-5928181C7D4D}&Serie=4549
                // Create the GET request.
                string overviewUrl = "https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/inschrijvingen_serie_individueel.asp?SID=" + NtbISessionCode + "&Serie=" + CurrentSerieNr;
            
                // Make a GET request and get the response back.
                // HtmlDocument overviewDoc = GetHtmlDocumentFromUrl(overviewUrl);
                HtmlDocument overviewDoc = new HtmlDocument();
                ConsoleWrite(string.Format("Getting Html document from: {0}", overviewUrl));

                HttpWebRequest overviewReq = (HttpWebRequest) WebRequest.Create(overviewUrl);
                if (Cookies!=null && Cookies.Count>0) {
                    overviewReq.CookieContainer.Add(Cookies);
                }
                overviewReq.KeepAlive = true;
                overviewReq.Method = "GET";
                overviewReq.Headers.Add("Authorization: Basic ZGNpOkFpR2g3YWVj");

                using (HttpWebResponse response = overviewReq.GetResponse() as HttpWebResponse) {
                    if (response.Cookies!=null && response.Cookies.Count>0) {
                        Cookies = response.Cookies;
                    }
                    NtbISessionCode = HttpUtility.ParseQueryString(response.ResponseUri.Query, Encoding.UTF8).Get("SID");

                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    overviewDoc.Load(reader);
                    reader.Close();
                }

                ////////////////////////////// START Evenement screen.
                // Open the evenement once to do some stuff that's apparantly necesary before you can access the entry detail page (setting cookies or something?).
                string evenementUrl = string.Format("https://mijn.triathlonbond.nl/evenementbeheer/evenement.asp?SID={0}&Evenement={1}", NtbISessionCode, CurrentEvenementNumber);

                // Make a GET request and get the response back.
                // HtmlDocument overviewDoc = GetHtmlDocumentFromUrl(overviewUrl);
                HtmlDocument evenementDoc = new HtmlDocument();
                ConsoleWrite(string.Format("Getting Html document from: {0}", evenementUrl));

                HttpWebRequest evenementReq = (HttpWebRequest) WebRequest.Create(evenementUrl);
                if (Cookies!=null && Cookies.Count>0) {
                    evenementReq.CookieContainer.Add(Cookies);
                }
                evenementReq.KeepAlive = true;
                evenementReq.Method = "GET";

                using (HttpWebResponse response = evenementReq.GetResponse() as HttpWebResponse) {
                    if (response.Cookies!=null && response.Cookies.Count>0) {
                        Cookies = response.Cookies;
                    }

                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    evenementDoc.Load(reader);
                    reader.Close();
                }
                ////////////////////////////// END evenement.


                // Filter op de 2e geneste <table> in de 2e <table> in de 2e <div> in de <body> van de <html>.
                // HtmlNodeCollection raceEntryRows = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("div")[1].SelectNodes("table")[1].SelectNodes("table")[1].SelectNodes("tr");
                HtmlNodeCollection raceEntryRows = overviewDoc.DocumentNode.SelectNodes("/html/body/div[2]/table/tr[2]/td/table/tr");
            
                raceEntryRows.Remove(raceEntryRows.ElementAt(0));
                List<InschrijvingModel> raceEntries = new List<InschrijvingModel>(raceEntryRows.Count);

                int counter = 1;

                foreach (HtmlNode raceEntryRow in raceEntryRows) {
                    InschrijvingModel raceEntry = new InschrijvingModel();

                    raceEntry.DateFirstSynchronized = DateTime.Now;

                    // Pak binnen elke TR even van de 7e td de innerHtml. Dit is de inschrijfdatum; en wel in het volgende format: 3-7-2012 23:03:15.
                    string registrationsDateAsString = raceEntryRow.SelectSingleNode("./td[7]").InnerHtml;
                    raceEntry.RegistrationDate = DateTime.Parse(registrationsDateAsString, CultureInfo.CreateSpecificCulture("nl-NL")); // "dd-MM-yyyy hh:mm:ss"

                    // Daarin neem je de 2e tot en met laatste <tr> (1e is header) en daarin telkens de waarde van het 'onclick' attribuut en wel de waarde tussen de aanhalingstekens (') binnen de Open functie.
                    // bijvoorbeeld: Open('inschrijvingen_serie_detail_individueel.asp?SID={7D72F8DA-7E50-46AC-B22E-81A638FB3171}&Inschrijving=81836');
                    // Regex re = new Regex(@"\'(.*?)\'");
                    string onClickValue = raceEntryRow.Attributes["onclick"].Value;
                    string entryUrlPostfix = Regex.Match(onClickValue, @"\'(.*?)\'").ToString();
                    entryUrlPostfix = entryUrlPostfix.Substring(1, entryUrlPostfix.Length-2);

                    string externalInschrijvingsId = HttpUtility.ParseQueryString(entryUrlPostfix, Encoding.UTF8).Get("Inschrijving");
                    raceEntry.ExternalIdentifier = externalInschrijvingsId;

                    raceEntry.ExternalEventIdentifier = CurrentEvenementNumber;
                    raceEntry.ExternalEventSerieIdentifier = CurrentSerieNr;

                    // Deze pagina open je voor alle rijen (GET request) door het achter de URL te plakken.
                    // Voorbeeld URL: https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/inschrijvingen_serie_detail_individueel.asp?SID={7D72F8DA-7E50-46AC-B22E-81A638FB3171}&Inschrijving=94938
                    string entryUrl = "https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/" + entryUrlPostfix;

                    ConsoleWrite(string.Format("Getting Html document for entry {0} of {1} from: {2}", counter, raceEntryRows.Count, entryUrl));

                    HtmlDocument entryDoc = new HtmlDocument();
                    HttpWebRequest entryReq = (HttpWebRequest) WebRequest.Create(entryUrl);
                    if (Cookies!=null && Cookies.Count>0) {
                        entryReq.CookieContainer.Add(Cookies);
                    }

                    entryReq.KeepAlive = true;
                    entryReq.Method = "GET";
                    // entryReq.Headers.Add("Authorization: Basic ZGNpOkFpR2g3YWVj");
                    loginRequest.Referer=overviewUrl;
                    // entryReq.Credentials = myCache;

                    using (HttpWebResponse response = entryReq.GetResponse() as HttpWebResponse) {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        if (response.Cookies!=null && response.Cookies.Count>0) {
                            Cookies = response.Cookies;
                        }

                        entryDoc.Load(reader);
                        reader.Close();
                    }

                    // II. Skrape van elke pagina dan de gegevens zoals hieronder gegeven.
                    // Hieruit pak je de <table> uit de tweede <div> in de <body> van de <html>.
                    // Hieruit pak je de 3e tot en met de laatste <tr>.
                    HtmlNodeCollection raceEntryDetails = entryDoc.DocumentNode.SelectNodes("/html/body/div[2]/table/tr");

                    // A. Uit de 3e en 4e <tr>'s pak je telkens de content die staat tussen de tweede <td> en </td> en de vierde <td> en </td>. Dan krijg je achtereenvolgens:
                    // 1. Voornaam, Geboortedatum.
                    raceEntry.Voornaam = raceEntryDetails[2].SelectNodes("./td[2]").First().InnerText.TrimThisShit();
                    string geboorteDatum = raceEntryDetails[2].SelectNodes("./td[4]").First().InnerText.TrimThisShit();
                    raceEntry.GeboorteDatum = DateTime.Parse(geboorteDatum); // CultureInfo.CreateSpecificCulture("nl-NL")

                    // 2. Tussenvoegsel, Licentienummer.
                    raceEntry.Tussenvoegsel = raceEntryDetails[3].SelectNodes("./td[2]").First().InnerText.TrimThisShit();
                    raceEntry.LicentieNummer = raceEntryDetails[3].SelectNodes("./td[4]").First().InnerText.TrimThisShit();

                    // B. In de 5e <tr> pak je uit de tweede <td>
                    // 3. Achternaam.
                    raceEntry.Achternaam = raceEntryDetails[4].SelectSingleNode("./td[2]").InnerText.TrimThisShit();
                
                    // In de 3e <td> van deze zelfde 5e <tr> zit daarna een geneste <table> met optionele gegevens (id=optioneel) in de 2e tot en met 5e <tr>.
                    // Uit de <tr> pak je telkens de 2e <td> voor achtereenvolgens:
                    // 4. ChampionChip nummer.
                    raceEntry.MyLapsChipNummer = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[2]/td[2]").InnerText.TrimThisShit();

                    // 5. Maat T-shirt
                    raceEntry.MaatTshirt = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[3]/td[2]").InnerText.TrimThisShit();

                    // 6. Aanmelden Nieuwsbrief
                    raceEntry.InteresseNieuwsbrief = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[4]/td[2]").InnerText.TrimThisShit()=="Ja";

                    // 7. Interesse in overnachten na de wedstrijd
                    raceEntry.InteresseOvernachtenNaWedstrijd = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[5]/td[2]").InnerText.TrimThisShit()=="Ja";

                    // C. In de 10 <tr>'s van de zesde en tot en met de laatste - 16e - <tr> pak je telkens de content die staat tussen de 2e <td> en </td>. Dan krijg je achtereenvolgens:
                    // 8. Geslacht.
                    raceEntry.Geslacht = raceEntryDetails[5].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 9. Straat en huisnummer (huisnummer gescheiden door &nbsp, spaties in straat zijn gewoon spaties).
                    string straatEnHuisnummer = raceEntryDetails[6].SelectSingleNode("./td[2]").InnerText.Trim();
                    const string straatEnHuisnummerSeperator = "&nbsp;";
                    int straatEnHuisnummerSeperatorIndex = straatEnHuisnummer.IndexOf(straatEnHuisnummerSeperator);
                    raceEntry.Straat = straatEnHuisnummer.Substring(0, straatEnHuisnummerSeperatorIndex);
                    string huisnummerEnToevoeging = straatEnHuisnummer.Substring(straatEnHuisnummerSeperatorIndex+straatEnHuisnummerSeperator.Length, straatEnHuisnummer.Length-(raceEntry.Straat.Length+straatEnHuisnummerSeperator.Length));
                    const string huisnummerEnToevoegingSeperator = "-";
                    int huisnummerEnToevoegingSeperatorIndex = huisnummerEnToevoeging.IndexOf(huisnummerEnToevoegingSeperator);
                    if (huisnummerEnToevoegingSeperatorIndex>0) {
                        raceEntry.Huisnummer = huisnummerEnToevoeging.Substring(0, huisnummerEnToevoegingSeperatorIndex);
                        raceEntry.HuisnummerToevoeging = huisnummerEnToevoeging.Substring(huisnummerEnToevoegingSeperatorIndex+huisnummerEnToevoegingSeperator.Length, huisnummerEnToevoeging.Length-(raceEntry.Huisnummer.Length+huisnummerEnToevoegingSeperator.Length));
                    } else {
                        raceEntry.Huisnummer = huisnummerEnToevoeging;
                        raceEntry.HuisnummerToevoeging = string.Empty;
                    }
                    // 10. Postcode.
                    raceEntry.Postcode = raceEntryDetails[7].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 11. Woonplaats+.
                    raceEntry.Woonplaats = raceEntryDetails[8].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 12. Land.
                    raceEntry.Land = raceEntryDetails[9].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 13. Telefoon.
                    raceEntry.Telefoon = raceEntryDetails[10].SelectSingleNode("./td[2]").InnerText.TrimThisShit();
                
                    // 14. E-mail.
                    raceEntry.Email = raceEntryDetails[11].SelectSingleNode("./td[2]").InnerText.TrimThisShit();
                    raceEntry.UserName = raceEntry.Email;

                    // 15. Opmerkingen tbv speaker.
                    raceEntry.OpmerkingenTbvSpeaker = raceEntryDetails[12].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 16. Bijzonderheden.
                    raceEntry.Bijzonderheden = raceEntryDetails[13].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 17. Deelnamebedrag.
                    string deelnamebedragAsString = raceEntryDetails[14].SelectSingleNode("./td[2]").InnerText;
                    string deelnamebedragKaleString = deelnamebedragAsString.Replace("&euro;", "").Replace(",", "").TrimThisShit();
                    int deelnamebedragAsInt;
                    bool result = int.TryParse(deelnamebedragKaleString, out deelnamebedragAsInt);
                    if (result) {
                        raceEntry.InschrijfGeld = deelnamebedragAsInt;
                    }

                    // Save the entry (Determine if the entry aready exists using externalIdentifier and then update, otherwise insert).
                    InschrijvingenRepository.SaveEntry(raceEntry, InschrijvingenRepository.HRE_EVENTNR, true, model.OverrideLocallyUpdated);
                    raceEntries.Add(raceEntry);

                    if (counter>=maxNumberOfItemsToScrape) {
                        ConsoleWrite(string.Format("Stopping scraping after {0} entries.", counter));
                        break;
                    }
                    counter++;

                }

            } catch (WebException ex) {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream())) {
                    string str = reader.ReadToEnd();
                    ConsoleWrite(string.Format("Error! Response: {0}", str)); 
                }
            }
            
            model.Entries = InschrijvingenRepository.GetEntries(InschrijvingenRepository.HRE_EVENTNR);
            return View("Index", model);
        }


        /// <summary>
        /// Write something to the console.
        /// </summary>
        /// <param name="ntbInschrijvingenStartUrl"></param>
        private static void ConsoleWrite(string str) {
            // Console.WriteLine(str);
            Debug.WriteLine(str);
        }


        /// <summary>
        /// Load the screen with a model filled with the given external ID and the data from HRE 2012.
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public ActionResult Edit(string externalId) {
            InschrijvingModel model = InschrijvingenRepository.GetByExternalIdentifier(externalId, InschrijvingenRepository.HRE_EVENTNR);
            
            
            if (model==null) {
                return HttpNotFound();
            }
            // Reset de opmerkingen velden, deze zijn voor 2013 NIET voorgevuld met gegevens uit 2012 zoals de overige velden.
            model.OpmerkingenTbvSpeaker = string.Empty;
            model.Bijzonderheden = string.Empty;
            return View("Edit", model);
        }


        /// <summary>
        /// Save the data (possibly) changed onscreen via a model assuming HRE 2013.
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(InschrijvingModel model) {
            // Run server side validations.
            if (model.HasMyLapsChipNummer && (string.IsNullOrEmpty(model.MyLapsChipNummer) || !Regex.IsMatch(model.LicentieNummer, @"^\w\w-*\d\d[\d\w][\d\w]\d$"))) {
                ModelState.AddModelError("MyLapsChipNummer", "Als je een eigen MyLaps chip hebt, vul dan het nummer in");
            }
            if (model.HasLicentieNummer && (string.IsNullOrEmpty(model.LicentieNummer) || !Regex.IsMatch(model.LicentieNummer, @"^\d\d[LA]\d\d\d\d\d[MV]\d\d\d$"))) {
                ModelState.AddModelError("LicentieNummer", "Als je lid bent van NTB, KNWU of KNZB vul dan een correct licentienummer in");
            }

            // Store the race entry in the local database.
            if (ModelState.IsValid) {
                InschrijvingenRepository.SaveEntry(model, InschrijvingenRepository.H2RE_EVENTNR, false);

                // TODO BW 2013-02-04: Synchronize the data to NTB inschrijvingen.
                if (model.ExternalIdentifier!=null) {
                    // The data is not yet in NTB inschrijvingen, write it to a new form.

                } else {
                    // The entry is already in NTB inschrijvingen, open and edit the existing entry.
                }

                // Set the model as updated and synchronized/scraped if it was written to NTB inschrijvingen.
                // model.DateUpdated=DateTime.Now;
                // model.DateLastScraped=model.DateUpdated;
                if (!model.DateFirstSynchronized.HasValue) {
                    model.DateFirstSynchronized=model.DateUpdated;
                }
            }
            return View("Edit", model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// 
        /*
        private static HtmlDocument GetHtmlDocumentFromUrl(string url) {
            ConsoleWrite(string.Format("Getting Html document from: {0}", url));
            
            HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
            req.Method = "GET";
            HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(resp.GetResponseStream());
            resp.GetResponseStream().Close();
            resp.Close();
            return doc;
        }
        */

        // Licentienummer koppeling
        // URL: http://ntbinschrijvingen.nl/Inschrijvingen/inschrijving_toevoegen_individueel.asp?Evenement=2005881&Serie=4549&Oorsprong=kalender
    }
}
