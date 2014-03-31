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
using System.Net.Mail;
using HRE.Models.Newsletters;
using HRE.Attributes;
using ClosedXML.Excel;
using System.Data;


namespace HRE.Controllers {

    public class InschrijvingenController : BaseController {


        public override bool IsConfidentialPage {
            get {
                return true;
            }
        }


        public void Initialise(string activeSubMenuItem) {
            ActiveMenuItem = AppConstants.Meedoen;
            ActiveSubMenuItem = activeSubMenuItem;
            ViewBag.SubMenuItems = SubMenuItems;
        }

        public ActionResult Index() {
            Initialise(AppConstants.MeedoenOverzicht);
            InschrijvingenModel model = new InschrijvingenModel();
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


        [Authorize]
        public ActionResult MijnRondjeEilanden(string externalId, string eventNr, bool emailConfirmed=false) {
            bool? isCheckSumValid;
            InschrijvingModel model = CheckConfirmationParameters(out isCheckSumValid);

            // If the checksum was incorrect then throw an exception in order to be notified (via Health Monitoring) of possible hacking or problems.
            if (!isCheckSumValid.HasValue || isCheckSumValid.HasValue) {
                throw new ArgumentException(string.Format("Error in checksum on checking iDeal parameters (extId: {0})", externalId));
            }
            if (model==null) {
                model = InschrijvingenRepository.GetByExternalIdentifier(externalId, eventNr);
            }
            return View(model);
        }


        public ActionResult Aangemeld(InschrijvingModel model) {
            bool? isCheckSumValid;
            model = CheckConfirmationParameters(out isCheckSumValid);

            // If the checksum was incorrect then throw an exception in order to be notified (via Health Monitoring) of possible hacking or problems.
            if (!isCheckSumValid.HasValue || isCheckSumValid.HasValue) {
                string txId = Request.QueryString["txid"];
                string ec = Request.QueryString["ec"];
                string error = Request.QueryString["error"];
                string status = Request.QueryString["status"];
                throw new ArgumentException(string.Format("Er ging iets mis. Error in iDeal checksum (txId: {0}, ec: {1}, error: {2}, status: {3}.", txId, ec, error, status ));
            }
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteTestUser(InschrijvingenModel model) {
            int eventId = SportsEventDal.GetByExternalId(model.EventNumber).ID;
            int userId = model.UserIdToDelete;
            SportsEventParticipationDal participation = SportsEventParticipationDal.GetByUserIdEventId(userId, eventId);
            if (participation!=null) {
                participation.Delete();
                model.Message = string.Format("Inschrijving van gebruiker {0} voor event {1} verwijderd.", userId, eventId);
            }
            
            return RedirectToAction("Index");
        }


        protected MemoryStream CreateExcelFile(string tableName, bool isForSpeaker) {
            try {
                // Create an Excel Workbook.
                XLWorkbook workbook = new XLWorkbook();
                var dataTable = GetTable(tableName, isForSpeaker);

                // Add the data.
                IXLWorksheet sheet = workbook.Worksheets.Add(dataTable);

                // Run autofit on all the columns.
                sheet.Columns().AdjustToContents();
                
                // Mark the first row as BOLD.
                sheet.FirstRow().Style.Font.Bold = true;
                
                // Freeze the first row with the headers.
                sheet.SheetView.FreezeRows(1);

                // All done.
                MemoryStream ms = new MemoryStream();
                workbook.SaveAs(ms);
                return ms;
            } catch (Exception e) {
                string errmsg = String.Format("Failed to create Excel file: {0}", e.Message);
                throw new Exception(errmsg, e);
            }
        }
        

        protected DataTable GetTable(String tableName, bool isForSpeaker) {
            InschrijvingenModel model = new InschrijvingenModel();

            DataTable table = new DataTable();
            table.TableName = tableName;

            table.Columns.Add("StartNr", typeof(int));
            table.Columns.Add("StartTijd", typeof(string));
            table.Columns.Add("Naam", typeof(string));
            table.Columns.Add("Woonplaats", typeof(string));
            table.Columns.Add("Heb je er zin in?", typeof(string));
            table.Columns.Add("Speaker", typeof(string));
            table.Columns.Add("M/V", typeof(string));
            table.Columns.Add("Food", typeof(string));
            table.Columns.Add("Camp", typeof(string));
            table.Columns.Add("Bike", typeof(string));
            table.Columns.Add("EarlyBird", typeof(string));
            table.Columns.Add("Geb. dat.", typeof(DateTime));
            table.Columns.Add("Lic. nr", typeof(string));
            table.Columns.Add("Aanmelddatum", typeof(DateTime));

            if (!isForSpeaker) {
                table.Columns.Add("UserId", typeof(int));
                table.Columns.Add("ExternalIdentifier", typeof(string));
                table.Columns.Add("MyLaps", typeof(string));
                table.Columns.Add("Email", typeof(string));
                table.Columns.Add("Telefoon", typeof(string));
                table.Columns.Add("Te betalen", typeof(int));
                table.Columns.Add("Betaald", typeof(int));
                table.Columns.Add("Voornaam", typeof(string));
                table.Columns.Add("tv", typeof(string));
                table.Columns.Add("Achternaam", typeof(string));
                table.Columns.Add("Postcode", typeof(string));
                table.Columns.Add("Straat", typeof(string));
                table.Columns.Add("Huisnr", typeof(string));
                table.Columns.Add("Toevoeging", typeof(string));
                table.Columns.Add("OpmerkingenAanOrganisatie", typeof(string));
                table.Columns.Add("DateCreated", typeof(DateTime));
                table.Columns.Add("DateUpdated", typeof(DateTime));
                table.Columns.Add("DateFirstScraped", typeof(DateTime));
                table.Columns.Add("DateLastScraped", typeof(DateTime));
            }

            // Initializeer startnummer en starttijd.
            // int startNummer = 1;
            // DateTime startTijd = HRE.Common.HreSettings.DatumTijdstipH2RE;

            foreach (var inschrijving in model.Inschrijvingen) {
                var row = isForSpeaker ?
                    new object[] {
                        inschrijving.StartNummer, // startNummer,
                        inschrijving.StartTijd.ToString(), // startTijd.ToString("H:mm:ss"),
                        Common.Common.SmartJoin(" ", new string[] { inschrijving.Voornaam, inschrijving.Tussenvoegsel, inschrijving.Achternaam}),
                        inschrijving.Woonplaats,
                        inschrijving.HebJeErZinIn,
                        inschrijving.OpmerkingenTbvSpeaker,
                        inschrijving.Geslacht,
                        inschrijving.IsEarlyBird.HasValue && inschrijving.IsEarlyBird.Value ? "1" : "0",
                        inschrijving.Food ? "1" : "0",
                        inschrijving.Camp ? "1" : "0",
                        inschrijving.Bike ? "1" : "0",
                        inschrijving.GeboorteDatum.Value.Date,
                        inschrijving.LicentieNummer,
                        inschrijving.RegistrationDate.Date
                    }
                    : new object[] {
                        inschrijving.StartNummer, // startNummer,
                        inschrijving.StartTijd.ToString(), // startTijd.ToString("H:mm:ss"),
                        Common.Common.SmartJoin(" ", new string[] { inschrijving.Voornaam, inschrijving.Tussenvoegsel, inschrijving.Achternaam}),
                        inschrijving.Woonplaats,
                        inschrijving.HebJeErZinIn,
                        inschrijving.OpmerkingenTbvSpeaker,
                        inschrijving.Geslacht,
                        inschrijving.IsEarlyBird.HasValue && inschrijving.IsEarlyBird.Value ? "1" : "0",
                        inschrijving.Food ? "1" : "0",
                        inschrijving.Camp ? "1" : "0",
                        inschrijving.Bike ? "1" : "0",
                        inschrijving.GeboorteDatum.Value.Date,
                        inschrijving.LicentieNummer,
                        inschrijving.RegistrationDate.Date,

                        // Extra velden voor NIET speakers.
                        inschrijving.UserId,
                        inschrijving.ExternalIdentifier,
                        inschrijving.MyLapsChipNummer,
                        inschrijving.Email,
                        inschrijving.Telefoon,
                        inschrijving.BedragTeBetalen,
                        inschrijving.BedragBetaald,
                        inschrijving.Voornaam,
                        inschrijving.Tussenvoegsel,
                        inschrijving.Achternaam,
                        inschrijving.Postcode,
                        inschrijving.Straat,
                        inschrijving.Huisnummer,
                        inschrijving.HuisnummerToevoeging,
                        inschrijving.OpmerkingenAanOrganisatie,
                        inschrijving.DateCreated,
                        inschrijving.DateUpdated,
                        inschrijving.DateFirstSynchronized,
                        inschrijving.DateLastSynchronized
                    };
                
                    table.Rows.Add(row);
                    /* if ((startNummer%HRE.Common.HreSettings.AantalPersonenPerStartschot)==0) {
                        startTijd = startTijd.AddSeconds(HRE.Common.HreSettings.AantalSecondenTussenStartschots);
                    }
                    startNummer++; */
            }
            
            return table;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteEntry(int participantUserId, InschrijvingenModel model) {
            SportsEventParticipationDal participation = SportsEventParticipationDal.GetByID(participantUserId);
            if (participation!=null) {
                participation.Delete();
            }
            
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Download the speaker list.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles="Admin,Speaker")]
        public ActionResult DownLoadSpeakerList(InschrijvingenModel model) {
            string filename = string.Format("HRE-Spkr-{0}-{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString().Replace(":", ""));
            MemoryStream ms = CreateExcelFile(filename, true);
            
            // Return the memorystream (IF something was created).
            if (ms != null) {
		        // Rewind the memory stream to the beginning.
		        ms.Seek(0, SeekOrigin.Begin);
		        return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", filename + ".xlsx");
	        } else {
                return View("Index");
            }
        }


        /// <summary>
        /// Scrape the inschrijvingen from NTB inschrijvingen.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult Index(InschrijvingenModel model) {
            if (model==null || model.SubmitAction!="Scrape") {
                if (model==null) {
                    model = new InschrijvingenModel();
                    return View(model);
                }

                if (model.SubmitAction=="Download") {
                    string filename = string.Format("HRE-{0}-{1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString().Replace(":", ""));
                    MemoryStream ms = CreateExcelFile(filename, false);
            
                    // Return the memorystream.
                    if (ms != null) {
		                // Rewind the memory stream to the beginning.
		                ms.Seek(0, SeekOrigin.Begin);
		                return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", filename + ".xlsx");
	                } else {
                        // No Excel file, show the main 'Inschrijvingen' page.
                        return RedirectToAction("Index");
                    }
                }
            }

            Initialise(AppConstants.MeedoenOverzicht);
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

                string currentEvenementNumber = model.EventNumber;
                
                string currentSerieNumber;
                switch (model.EventNumber) {
                    case InschrijvingenRepository.H2RE_EVENTNR: 
                        currentSerieNumber = InschrijvingenRepository.H2RE_SERIENR;
                        break;
                    case InschrijvingenRepository.HRE_SERIENR:
                        currentSerieNumber = InschrijvingenRepository.HRE_SERIENR;
                        break;
                    case InschrijvingenRepository.H3RE_SERIENR:
                    default:
                        currentSerieNumber = InschrijvingenRepository.H3RE_SERIENR;
                        break;
                }
                // I. Overzichtscherm. Voorbeeld URL: https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/inschrijvingen_serie_individueel.asp?SID={20B12A29-FFA4-4606-B912-5928181C7D4D}&Serie=4549
                // Create the GET request.
                string overviewUrl = "https://mijn.triathlonbond.nl/evenementbeheer/inschrijvingen/inschrijvingen_serie_individueel.asp?SID=" 
                    + NtbISessionCode + "&Serie=" + currentSerieNumber; // CurrentSerieNr;
            
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
                string evenementUrl = string.Format("https://mijn.triathlonbond.nl/evenementbeheer/evenement.asp?SID={0}&Evenement={1}", NtbISessionCode, currentEvenementNumber);

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

                    raceEntry.ExternalEventIdentifier = currentEvenementNumber;
                    raceEntry.ExternalEventSerieIdentifier = currentSerieNumber;

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
                    raceEntry.Newsletter = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[4]/td[2]").InnerText.TrimThisShit()=="Ja";

                    // 7. Interesse in overnachten na de wedstrijd
                    raceEntry.Camp = raceEntryDetails[4].SelectSingleNode("./td[3]/table/tr[5]/td[2]").InnerText.TrimThisShit()=="Ja";

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

                    // 16. Opmerkingen aan organisatie.
                    raceEntry.OpmerkingenAanOrganisatie = raceEntryDetails[13].SelectSingleNode("./td[2]").InnerText.TrimThisShit();

                    // 17. Deelnamebedrag.
                    string deelnamebedragAsString = raceEntryDetails[14].SelectSingleNode("./td[2]").InnerText;
                    string deelnamebedragKaleString = deelnamebedragAsString.Replace("&euro;", "").Replace(",", "").TrimThisShit();
                    int deelnamebedragAsInt;
                    bool result = int.TryParse(deelnamebedragKaleString, out deelnamebedragAsInt);
                    if (result) {
                        raceEntry.InschrijfGeld = deelnamebedragAsInt;
                    }

                    // Save the entry (Determine if the entry aready exists using externalIdentifier and then update, otherwise insert).
                    InschrijvingenRepository.SaveEntry(raceEntry, currentEvenementNumber, true, model.OverrideLocallyUpdated);
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
        // [RequiresSsl]
        public ActionResult IkDoeMee() {
            InschrijvingModel model = new InschrijvingModel();
            sportsevent currentEvent = InschrijvingenRepository.GetCurrentEvent();
            model.ExternalEventIdentifier = currentEvent.ExternalEventIdentifier;
            model.ExternalEventSerieIdentifier = currentEvent.ExternalEventSerieIdentifier;
            model.ExternalIdentifier = string.Empty;
            model.RegistrationDate = DateTime.MinValue;

            model.IsInschrijvingNieuweGebruiker = true;

            return View("Edit", model);
        }
        
        
        /// <summary>
        /// Upload test.
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult UploadTest() {
            InschrijvingModel model = new InschrijvingModel();
            return View(model);
        }

        
        /// <summary>
        /// Load the screen with a model filled with the given external ID and the data from HRE 2012.
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public ActionResult Edit(string externalId, string eventNr) {
            Initialise(AppConstants.MeedoenOverzicht);

            InschrijvingModel model = InschrijvingenRepository.GetByExternalIdentifier(externalId, eventNr);

            // Initialize the email before update, so that a change in the e-mail address can be detected.

            // Als inladen is mislukt en het gaat om inschrijving voor dit jaar - 2014.
            if (model==null && eventNr==InschrijvingenRepository.H3RE_EVENTNR) {
                // Probeer dan de gegevens voor te laden uit het jaar ervoor (2013)
                model = InschrijvingenRepository.GetByExternalIdentifier(externalId, InschrijvingenRepository.H2RE_EVENTNR);
                if (model==null) {
                    // Of zo niet, dan uit 2 jaar terug - 2012.
                    model = InschrijvingenRepository.GetByExternalIdentifier(externalId, InschrijvingenRepository.HRE_EVENTNR);
                }
                // Als gevonden..
                if (model!=null) {
                    // Reset de gegevens uit 2013.
                    model.InschrijfGeld = null;
                    model.OpmerkingenTbvSpeaker = string.Empty;
                    model.OpmerkingenAanOrganisatie = string.Empty;
                    model.ExternalEventIdentifier = InschrijvingenRepository.H3RE_EVENTNR;
                    model.ExternalEventSerieIdentifier = InschrijvingenRepository.H3RE_SERIENR;
                    model.ExternalIdentifier = string.Empty;
                    model.RegistrationDate = DateTime.MinValue;
                    model.Bike = false;
                    model.Camp = false;
                    model.Food = false;
                } else {
                    return HttpNotFound();
                }
            }

            if (model!=null) {
                model.EmailBeforeUpdateIfAny = model.Email;
            }

            return View("Edit", model);
        }


        public ActionResult Reglementen() {
            Initialise(AppConstants.MeedoenOverzicht);
            return View();
        }


        /// <summary>
        /// Save the data (possibly) changed onscreen via a model assuming HRE 2013.
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(InschrijvingModel model) {
            Initialise(AppConstants.MeedoenOverzicht);

            // Run server side validations.
            if (model.HasLicentieNummer && (string.IsNullOrEmpty(model.LicentieNummer) || !Regex.IsMatch(model.LicentieNummer, @"^\d\d[LA]\d\d\d\d\d[MV]\d\d\d$"))) {
                ModelState.AddModelError("LicentieNummer", "Geef correct licentienummer (of zet licentie-icoon uit)");
            }
            // 4R-YGF8T.
            if (model.HasMyLapsChipNummer) {
                if (string.IsNullOrEmpty(model.MyLapsChipNummer)) {
                    ModelState.AddModelError("MyLapsChipNummer", "Vul een correct MyLaps chipnr in (of zet icoon 'eigen-chip' uit)");
                } else if (!CheckMyLapsChipNumber(model.MyLapsChipNummer)) {
                    ModelState.AddModelError("MyLapsChipNummer", "Geen geldig MyLaps chip nummer! Graag controleren.");
                }
            }

            if (model.Land=="NL" && !string.IsNullOrEmpty(model.Postcode) && !Regex.IsMatch(model.Postcode, @"^\d{4}\s*\w{2}$")) {
                ModelState.AddModelError("Postcode", "Geen geldige Nederlandse postcode!");
            }

            if (!string.IsNullOrEmpty(model.Email) && !Regex.IsMatch(model.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")) {
                ModelState.AddModelError("Email", "Geen geldig e-mail adres!");
            }

            // Only check the payment if the subscription is new.
            if ((!model.IsBetaald.HasValue || !model.IsBetaald.Value)  && model.Betaalwijze != PaymentType.iDeal) {
                ModelState.AddModelError("PaymentType", "Alleen iDeal betaling mogelijk");
            }

            // Check final yes/no if the subsription is new.
            if (model.IsNewUser && string.IsNullOrEmpty(model.Finale)) {
                ModelState.AddModelError("PaymentType", "Geef aan of je de finale denkt mee te kunnen en willen doen.");
            }

            // Only check the payment if the subsription is new.
            if ((!model.IsBetaald.HasValue || !model.IsBetaald.Value) && string.IsNullOrEmpty(model.BankCode)) {
                ModelState.AddModelError("BankCode", "Selecteer bank voor iDeal betaling");
            }

            // TODO BW 2013-03-18: Make currentEventNr variable, depending on which event is subscribed to.
            // string currentEventNr = model.ExternalEventIdentifier;
            string currentEventNr = InschrijvingenRepository.H3RE_EVENTNR;

            // Controleer als het een nieuwe inschrijving betreft dat er nog geen deelnemer is met hetzelfde e-mail adres.
            LogonUserDal user = LogonUserDal.GetUserByUsername(model.Email);
            if ((model.IsInschrijvingNieuweGebruiker || (!model.IsInschrijvingNieuweGebruiker && model.Email!=model.EmailBeforeUpdateIfAny)) 
                    && user!=null && InschrijvingenRepository.GetInschrijving(user, currentEventNr)!=null) {
                ModelState.AddModelError("Email", "Er is al een deelnemer met dit e-mail adres ingeschreven. Elke deelnemer moet een eigen e-mail adres opgeven!");
            }

            // Store the race entry in the local database.
            if (ModelState.IsValid) {
                // Set the eventIdentifier to the event of 2014.
                // TODO BW 2013-02-10: Refactor "HRE" to prefix constant.
                model.ExternalIdentifier = "HRE" + model.ParticipationId;
                
                // Sla op!
                InschrijvingenRepository.SaveEntry(model, InschrijvingenRepository.H3RE_EVENTNR, false, true);
                
                if (!model.DateConfirmationSend.HasValue || model.DoForceSendConfirmationOfChange) {
                    SendSubscriptionConfirmationMail(model);
                    model.DateConfirmationSend = DateTime.Now;
                    
                    // Sla de entry nog een keer op, om ook de dateConfirmationSend op te slaan.
                    // TODO BW 2013-03-01: Doe dit wat netter dan alles geheel twee keer opslaan.
                    InschrijvingenRepository.SaveEntry(model, InschrijvingenRepository.H3RE_EVENTNR, false, true);
                }

                // TODO BW 2013-02-04: Synchronize the data to NTB inschrijvingen.
                if (string.IsNullOrEmpty(model.ExternalIdentifier) || model.ExternalIdentifier.StartsWith("HRE")) {
                    // The data is not yet in NTB inschrijvingen, write it to a new form.
                } else {
                    // The entry is already in NTB inschr, load the edit page of it, edit the existing entry and save.
                }

                // TODO BW 2013-02-10: Set the model as updated and synchronized/scraped if it was written to NTB inschrijvingen.
                // model.DateUpdated=DateTime.Now;
                // model.DateLastScraped=model.DateUpdated;
                // if (!model.DateFirstSynchronized.HasValue) {
                //    model.DateFirstSynchronized=model.DateUpdated;
                // }
                
                if (model.IsInschrijvingNieuweGebruiker) {
                    string sisowUrl = model.SisowUrl;
                    // return RedirectToAction("Aangemeld", model);
                    return Redirect(sisowUrl);
                } else {
                    if (!model.IsBetaald.HasValue || !model.IsBetaald.Value) {
                        model.SisowReturnUrl = Url.Action("MijnRondjeEilanden", new { externalId=model.ExternalIdentifier, eventNr=model.ExternalEventIdentifier });
                        string sisowUrl = model.SisowUrl;
                        return Redirect(sisowUrl);
                    } else {
                        return RedirectToAction("MijnRondjeEilanden", new { externalId=model.ExternalIdentifier, eventNr=model.ExternalEventIdentifier });
                    }
                }
            }

            return View(model);
        }


        // New Block Chips
        // 7 positions
        // 1st position: D,E,F,G,H,K,M,N,P,R,S,T,V,W,Y,Z
        // 2nd position: A,B,C,D,E,F,G,H,K,M,N,P,R,S,T,V,W,X,Y,Z
        // 3rd position: 0,1,2,3,4,5,6,7,8,9
        // 4th, 5th, 6th position: 0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,K,M,N,P,R,S,T,V,W,X,Y,Z

        // The 7th position is the check-character. The check character is calculated from the 6 preceding characters. 
        // Each character has an assigned value. The digits represent values 0 – 9, the letters represent values 10 – 35.
        // Sum all values of the preceding characters and divide the total by 29. 
        // The remainder of the division points to the x-th position in the series: Z97BN4XSVE56AWYM1TPK8H2GDCF3R

        // As an example: If the remainder is 0, the last position should be Z. If the remainder is 2, the last position should be 7.
        // Examples: DY7AM37, DW24PWH, DG7SKAS, DA51D0W, HD7W0RE

        /// Performs the check for NewBlock MyLaps nr's (taken from document from MyLaps).
        protected static bool CheckMyLapsChipNumber(string input) {
            bool result = Regex.IsMatch(input, @"^[\d\w][\w]-?[\d\w][\d\w][\d\w][\d\w][\d\w]$");
            if (!result) {
                return false;
            }
            bool isOldBlock = Regex.IsMatch(input, @"^(A|B|C)", RegexOptions.IgnoreCase);
            if (isOldBlock) {
                return true;
            }
            string inputWithoutDash = input.Replace("-", "");
            char checkChar = inputWithoutDash[6];
            int sum = 0;
            foreach (char c in inputWithoutDash.Substring(0, 6).ToCharArray()) {
                int value = (int) (c - '0');
                if (value>10) {
                    value = value-7;
                }
                sum += value;
            }
            int index = sum % 29;
            const string checkString = "Z97BN4XSVE56AWYM1TPK8H2GDCF3R";
            char computedChar = checkString[index];
            result = computedChar==checkChar;
            return result;
        }
        

        /// <summary>
        /// Stuurt een bevestigingsmail van inschrijving naar de gebruiker.
        /// Hiervoor wordt de nieuwsbrief template gebruikt.
        /// </summary>
        /// <param name="model"></param>
        private void SendSubscriptionConfirmationMail(InschrijvingModel model) {
            NewsletterViewModel newsletter = new NewsletterViewModel();
            newsletter.IncludeLoginLink = true;
            newsletter.Items = new List<NewsletterItemViewModel>();

            if (!model.DoForceSendConfirmationOfChange) {
                newsletter.IntroText = string.Format("Je bent aangemeld voor Hét Rondje Eilanden!");
                if (model.BedragTeBetalen>0) {
                    newsletter.IntroText = "Je inschrijving wordt definitief na betaling van het inschrijfgeld.";
                }
            } else {
                if (model.EmailBeforeUpdateIfAny!=model.Email) {
                    newsletter.IntroText = string.Format("Je bent aangemeld voor Hét Rondje Eilanden! Je hebt je e-mail adres gewijzigd, of iemand anders heeft zijn inschrijving naar jouw e-mail overgezet. Bevestig je inschrijving en dit e-mail adres via onderstaande link. ");
                } else {
                    newsletter.IntroText = string.Format("Hier een Flessenpost achtige mail omdat je je inschrijfgegevens hebt gewijzigd.");
                }
            }
            newsletter.IntroText += "<br/>Hieronder je inschrijfgegevens. Als er iets niet klopt graag direct zelf wijzigen door op onze site door in te loggen via de persoonlijke link hiernaast.";

            NewsletterItemViewModel item1 = new NewsletterItemViewModel();
            if (!model.DoForceSendConfirmationOfChange) {
                item1.Title = "You're in!";
                item1.SubTitle = "voor Hét 3e Rondje Eilanden";
            } else {
                item1.Title = "Tough that you are there by!";
                item1.SubTitle = "...we can haste not wait ourselves!";
            }
            item1.HeadingHtmlColour = "208900";
            item1.ImagePath = "News_2013.png";
            item1.IconImagePath = "News_TileEB.png";

            item1.Text = string.Format("");
            item1.Text += string.Format("<table>");
                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Inschrijfgegevens</b></th></tr>");

                item1.Text += string.Format("<tr><td>Naam</td><td>{0}</span></td></tr>", model.VolledigeNaamMetAanhef);
                item1.Text += string.Format("<tr><td>Geboortedatum</td><td>{0}</span></td></tr>", model.GeboorteDatum.HasValue ? model.GeboorteDatum.Value.ToShortDateString() : "-");
                item1.Text += string.Format("<tr><td>NTB Licentie</td><td>{0}</span></td></tr>", string.IsNullOrEmpty(model.LicentieNummer) ? " - " : model.LicentieNummer);
                item1.Text += string.Format("<tr><td>Persoonlijke MyLaps Chip</td><td>{0}</span></td></tr>", string.IsNullOrEmpty(model.MyLapsChipNummer) ? " - " : model.MyLapsChipNummer);
                
                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Persoonsgegevens</b></th></tr>");

                item1.Text += string.Format("<tr><td>Geslacht</td><td>{0}</span></td></tr>", model.Geslacht ?? "-");
                item1.Text += string.Format("<tr><td>Straat</td><td>{0}</span></td></tr>", model.Straat ?? "-");
                item1.Text += string.Format("<tr><td>Huisnummer</td><td>{0}</span></td></tr>", model.Huisnummer ?? "-");
                item1.Text += string.Format("<tr><td>Toevoeging</td><td>{0}</span></td></tr>", model.HuisnummerToevoeging ?? "-");
                item1.Text += string.Format("<tr><td>Postcode</td><td>{0}</span></td></tr>", model.Postcode ?? "-");
                item1.Text += string.Format("<tr><td>Woonplaats</td><td>{0}</span></td></tr>", model.Woonplaats?? "-");
                item1.Text += string.Format("<tr><td>Land</td><td>{0}</span></td></tr>", model.Land ?? "-");
                item1.Text += string.Format("<tr><td>E-mail adres</td><td>{0}</span></td></tr>", model.Email ?? "-");
                item1.Text += string.Format("<tr><td>Telefoon</td><td>{0}</span></td></tr>", model.Telefoon ?? "-");

                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Extra's</b></th></tr>");

                item1.Text += string.Format("<tr><td>Meeeten?</td><td>{0}</span></td></tr>", model.Food ? "Ja" : "Nee");
                item1.Text += string.Format("<tr><td>Kamperen</td><td>{0}</span></td></tr>", model.Camp ? "Interesse" : "Geen interesse");
                item1.Text += string.Format("<tr><td>Fiets stallen</td><td>{0}</span></td></tr>", model.Bike ? "Interesse" : "Geen interesse");
                item1.Text += string.Format("<tr><td>Finale</td><td>{0}</span></td></tr>", model.Finale=="W" ? "Ik ga ervoor!" : "Ik zie wel" );

                
                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Opmerkingen</b></th></tr>");

                item1.Text += string.Format("<tr><td>Opmerkingen</td><td>{0}</span></td></tr>", model.HebJeErZinIn ?? "-");
                item1.Text += string.Format("<tr><td>Opmerkingen voor speaker</td><td>{0}</span></td></tr>", model.OpmerkingenTbvSpeaker?? "-");
                item1.Text += string.Format("<tr><td>Opmerkingen voor organisatie</td><td>{0}</span></td></tr>", model.OpmerkingenAanOrganisatie ?? "-");

                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Inschrijfkosten</b></th></tr>");
                item1.Text += string.Format("<tr><td>Inschrijfgeld</td><td>{0}</span></td></tr>", model.BasisKosten.AsAmount() );    
                item1.Text += string.Format("<tr><td>NTB Licentie</td><td>{0}</span></td></tr>", model.KostenNtbDagLicentie.AsAmount() );
                item1.Text += string.Format("<tr><td>MyLaps Chip</td><td>{0}</span></td></tr>", model.KostenChip.AsAmount());
                if (model.IsEarlyBird.HasValue && model.IsEarlyBird.Value) {
                    item1.Text += string.Format("<tr><td>Early Bird™ korting</td><td>{0}</span></td></tr>", model.EarlyBirdKorting.AsAmount());
                }
                item1.Text += string.Format("<tr><td>Avondeten</td><td>{0}</span></td></tr>", model.KostenEten.AsAmount());
                item1.Text += string.Format("<tr><td><b></b></td><td><span class=\"quotable\">------</span></td></tr>");
                item1.Text += string.Format("<tr><td><b>Totaal</b></td><td><span class=\"quotable\">{0}</span></td></tr>", model.InschrijfGeld.AsAmount());
                
                /*
                item1.Text += string.Format("<tr><td></td></tr>");
                item1.Text += string.Format("<tr><th colspan=\"2\"><b>Overmaken naar</b></th></tr>");

                item1.Text += string.Format("<tr><td>Bank rek nr</td><td><span class=\"quotable\">'1684.92.059'</span> (Rabobank)</td></tr>");
                item1.Text += string.Format("<tr><td>Ten name van </td><td><span class=\"quotable\">'Stichting Woelig Water'</span> (te Vinkeveen)</td></tr>");
                item1.Text += string.Format("<tr><td>Onder vermelding van</td><td><span class=\"quotable\">'H2RE {0}'</span></td></tr>", model.VolledigeNaam);
                */

                item1.Text += string.Format("</table></td></tr>");
                item1.Text += string.Format("</table>");

            newsletter.Items.Add(item1);

            SendPersonalNewsletterViewModel spnvm = new SendPersonalNewsletterViewModel();
            spnvm.IsEmail = true;
            spnvm.Newsletter = newsletter;

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(HreSettings.ReplyToAddress);
            mm.To.Add(new MailAddress(model.Email));
            
            // Send the confirmation mail to an appsetting defined admin email address for backup purposes.
            mm.Bcc.Add(new MailAddress(HreSettings.ConfirmationsCCAddress));

            if (!string.IsNullOrEmpty(model.OpmerkingenAanOrganisatie)) {
                mm.Bcc.Add(new MailAddress(HreSettings.MailAddressSecretary));
            }

            mm.Subject = model.DoForceSendConfirmationOfChange ? 
                "Wijziging in inschrijfgegevens Het 3e Rondje Eilanden " + model.VolledigeNaam
                : "Aanmeldbevestiging deelname Het 3e Rondje Eilanden " + model.VolledigeNaam;

            mm.IsBodyHtml = true;
            spnvm.UserId = model.UserId;
            mm.Body = this.RenderNewsletterViewToString("../Newsletter/NewsletterTemplates/NewsletterTemplate", spnvm);
            EmailSender.SendEmail(mm, EmailCategory.SubscriptionConfirmation, model.ParticipationId, model.UserId);
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

        [Authorize(Roles = "Admin")]
        public ActionResult UploadVolonteers() {
            InschrijvingModel model = new InschrijvingModel();
            return View(model);
        }


        /// <summary>
        /// Handles the situation when the page is called back from iDEAL payment (with URL params).
        /// </summary>
        protected InschrijvingModel CheckConfirmationParameters(out bool? isCheckSumValid) {
            InschrijvingModel result = null;
            string txID = Request.QueryString["txId"];
            string check = Request.QueryString["check"];
            string ec = Request.QueryString["ec"];
            string status = Request.QueryString["status"];
            string error = Request.QueryString["error"];
            bool confirmationChecksOut = SisowIdealHandler.DoesConfirmationCheckOut(txID, ec, status, check, error, out isCheckSumValid);
            
            if (confirmationChecksOut) {
                int participationID = int.Parse(ec);
                result = InschrijvingenRepository.GetInschrijvingByParticipationID(participationID);
                if (result !=null && !string.IsNullOrEmpty(status) && confirmationChecksOut) {
                    bool isPaid = status == "Success";
                    bool isJustPaid = isPaid && !result.DatumBetaald.HasValue;
                    if (isJustPaid) {
                        InschrijvingenRepository.MarkEntryAsPaid(participationID);
                        result.IsBetaald = true;
                    }
                }
            }
            return result;
        }

    }
}
