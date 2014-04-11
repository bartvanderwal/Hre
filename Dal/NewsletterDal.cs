using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Web;
using System.Web.UI;
using HRE.Business;
using HRE.Data;

namespace HRE.Dal {
    public class NewsletterDal : ObjectDal {
        
        #region :: Members

        private newsletter _newsletter;

        private const int _newsletterWidthInPixels = 600;

        // TODO: Support any nr of newsletter items instead of 3 fixed ones.
        private NewsletterItemDal _item1;
        private NewsletterItemDal _item2;
        private NewsletterItemDal _item3;
        #endregion :: Members

        /// <summary>
        /// Constructor. Construct an e-mail newsletter object.
        /// </summary>
        public NewsletterDal() {
            _newsletter = new newsletter();
        }

        /// <summary>
        /// Constructor. Construct an e-mail newsletter object.
        /// </summary>
        public NewsletterDal(newsletter newsletter) {
            _newsletter = newsletter;
        }


        #region :: Properties
        /// <summary>
        /// Key.
        /// </summary>
        public int ID {
            get { return _newsletter.Id; }
        }
		
        /// <summary>
        /// The date (time) the e-mail newsletter was first created.
        /// </summary>
        public DateTime DateCreated {
            get { return _newsletter.DateCreated; }
            set { _newsletter.DateCreated = value; }
        }

        /// <summary>
        /// The date the e-mail newsletter was sent.
        /// </summary>
		public DateTime? DateSent {
            get { return _newsletter.DateSent; }
            set { _newsletter.DateSent = value; }
        }


        /// <summary>
        /// Was the newsletter already sent or not?
        /// </summary>
		public bool IsSent {
            get { return DateSent.HasValue; }
        }

        /// <summary>
        /// Was the newsletter not sent or was it :)?
        /// </summary>
		public bool NotSent {
            get { return !IsSent; }
        }


        /// <summary>
        /// The sequence number of this newsletter.
        /// </summary>
		public string IntroText {
            get { return _newsletter.IntroText; }
            set { _newsletter.IntroText = value; }
        }

        /// <summary>
        /// The culture ID for the language/country of the users of the e-mail newsletter.
        /// </summary>
		public int CultureID {
            get { return _newsletter.CultureId; }
            set { _newsletter.CultureId = value; }
        }

        /// <summary>
        /// The title of the newsletter.
        /// </summary>
		public String Title {
            get { return _newsletter.Title; }
            set { _newsletter.Title = value; }
        }


        /// <summary>
        /// The path (including filename) to the file that is to be attached to the newsletter (if any, empty or null otherwise).
        /// </summary>
		public String AttachmentFilePath {
            get { return _newsletter.AttachmentFilePath; }
            set { _newsletter.AttachmentFilePath = value; }
        }


        // TODO BW Allow N newsletter items instead of three fixed ones.
        public NewsletterItemDal Item1 {
            get { 
                if (_item1==null) {
                    _item1 = new NewsletterItemDal(1);
                }
                return _item1;
            }
            set { _item1 = value; }
        }

        public NewsletterItemDal Item2 {
            get {
                if (_item2==null) {
                    _item2 = new NewsletterItemDal(2);
                }
                return _item2;
            }
            set { _item2 = value; }
        }

        public NewsletterItemDal Item3 {
            get {
                if (_item3==null) {
                    _item3 = new NewsletterItemDal(3);
                }
                return _item3;
            }
            set { _item3 = value; }
        }


        public bool IsStoredInDB {
            get { return DB.newsletter.Where(e => e.Id == _newsletter.Id).Count() == 1; }
        }

        #endregion :: Properties

        #region :: Methods
        /// <summary>
        /// Initialize e-mail newsletter.
        /// </summary>
        public void Initialize() {
            // Initialize the newsletter if not found.
            if (_newsletter == null) {
                _newsletter = new newsletter();
                _newsletter.DateCreated = DateTime.Now;
            }

            // Try to retrieve the newsletter items with this newsletter.
            var newsletterItems = DB.newsletteritem.Where(e => e.NewsletterId == ID);

            // Set the newsletter items if found.
            if (newsletterItems.Count()==3) {
                Item1 = new NewsletterItemDal(newsletterItems.Where(e => e.SequenceNumber == 1).FirstOrDefault());
                Item2 = new NewsletterItemDal(newsletterItems.Where(e => e.SequenceNumber == 2).FirstOrDefault());
                Item3 = new NewsletterItemDal(newsletterItems.Where(e => e.SequenceNumber == 3).FirstOrDefault());
            } else {
                // Construct new items and add them to the newsletter.
                Item1 = new NewsletterItemDal(1);
                Item2 = new NewsletterItemDal(2);
                Item3 = new NewsletterItemDal(3);
            }
        }

        /// <summary>
        /// Store the e-mail newsletter in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (!IsStoredInDB) {
                DateCreated = System.DateTime.Now;
                DB.AddTonewsletter(_newsletter);
            }
  		    DB.SaveChanges();

            // Then save the newsletter items.
            Item1.Save(ID);
            Item2.Save(ID);
            Item3.Save(ID);
        }

        /// <summary>
        /// Delete the newsletter and it's corresponding Newsletteritems.
        /// Note that newsletters that are not yet saved, or that were already sent can not be deleted.
        /// </summary>
        public void Delete() {
            // Add entity if it has no primary key yet.
            if (!IsStoredInDB) {
                throw new InvalidOperationException("This newsletter cannot be deleted because it was not yet stored in the database.");
            }
            if (IsSent) {
                throw new InvalidOperationException("This newsletter cannot be deleted because it was already sent to one or more recipients on " + DateSent.Value.ToShortDateString() + ".");
            }

            // Delete the newsletter items.
            Item1.Delete();
            Item2.Delete();
            Item3.Delete();

  		    DB.DeleteObject(_newsletter);
  		    DB.SaveChanges();
        }

        /// <summary>
        /// Create the HTML for this newsletter.
        /// </summary>
        /// <returns>The HTML string of the newsletter.</returns>
        public string CreateHtml() {
            StringBuilder txt = new StringBuilder();

            // TODO: Create pictures also for other countrys than only NL.
            txt.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" valign=\"top\" align=\"left\" style=\"width: " + _newsletterWidthInPixels + "px; font-family: Gill Sans MT, Tahoma, Verdana, sans-serif; font-size: 12px; \">");
            txt.Append("<tr>");
            txt.Append("<td>");
            string newsletterHeaderImage = Settings.NewsletterHeaderImage;
            txt.Append("<img style=\"border: 0px; margin: 10px 0px 20px 0px; padding: 0px;\" src=\"" + GeneralUtil.DetermineDomainBaseHttp(CultureID) + newsletterHeaderImage + "\" alt=\"karakterstructuren.com\" />");
            txt.Append("</td>");
            txt.Append("</tr>");
            txt.Append("<tr>");
            txt.Append("<td>");
            txt.Append("<h1>" + _newsletter.Title + "</h1>");
            List<NewsletterItemDal> newsletterItems = new List<NewsletterItemDal>();
            newsletterItems.Add(Item1);
            newsletterItems.Add(Item2);
            newsletterItems.Add(Item3);
        
            // Create the HTML for the items.    
            foreach (NewsletterItemDal item in newsletterItems) {
                txt.Append(item.CreateHtml(CultureID));
            }
            txt.Append("</td>");
            txt.Append("</tr>");
            txt.Append("</table>");
            txt.Append("<br/ ><br clear=\"all\"/>");
            txt.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" valign=\"top\" align=\"left\" style=\"width: " + _newsletterWidthInPixels + "px; font-family: Gill Sans MT, Tahoma, Verdana, sans-serif; font-size: 12px; \">");

            txt.Append("<tr>");
            txt.Append("<td style=\"text-align: center;\">");
            txt.Append("<a href=\"http://www.karakterstructuren.com\">www.karakterstructuren.com</a>");
            txt.Append("</td>");
            txt.Append("<td style=\"text-align: center;\">");

            txt.Append("&nbsp;");
            txt.Append("</td>");
            txt.Append("<td style=\"text-align: center;\">");
            txt.Append("E-mail: <a href=\"mailto:tijn@karakterstructuren.com\">tijn@karakterstructuren.com</a>");
            txt.Append("</td>");
            txt.Append("</tr>");
            txt.Append("</table>");
            return txt.ToString();
        }

        /// <summary>
        /// Create the plain text version of this newsletter (for non HTML mail programs).
        /// </summary>
        /// <returns>The HTML string of the newsletter.</returns>
        public string CreateText () {
            StringBuilder txt = new StringBuilder();

            txt.Append(_newsletter.Title  + "\n");
            txt.Append("Nieuwsbrief van " + _newsletter.DateCreated.ToShortDateString()  + "\n");
            List<NewsletterItemDal> newsletterItems = new List<NewsletterItemDal>();
            newsletterItems.Add(Item1);
            newsletterItems.Add(Item2);
            newsletterItems.Add(Item3);
        
            // Create the text for the items.    
            foreach (NewsletterItemDal item in newsletterItems) {
                txt.Append(item.CreateText());
            }

            txt.Append("www.karakterstructuren.com\n");
            txt.Append("E-mail: tijn@karakterstructuren.com\n");
            return txt.ToString();
        }

        /// <summary>
        /// Create an HTML newsletter based on user provided values.
        /// </summary>
        /// <returns>The HTML string of the newsletter.</returns>
        public string DetermineUnsubscribeLink(string mailAddress) {
            StringBuilder unsubscribeUrl = new StringBuilder();
            
            unsubscribeUrl.Append(GeneralUtil.DetermineDomainBaseHttp(CultureID));
            unsubscribeUrl.Append("/Site/NewsletterSubscription.aspx");
            
            bool allowUnencryptedEmailForNewsletterSubscription = Settings.AllowUnencryptedEmailForNewsletterSubscription;
            if (allowUnencryptedEmailForNewsletterSubscription) {
                unsubscribeUrl.Append("?ma=" + mailAddress);
            } else {
                // Haal de encryptie sleutel uit de configuratie (=web.config bestand) en versleutel het e-mail adres.
                // string emaCypher = ConfigurationManager.AppSettings["emaCypher"].ToString();
                string emaCypher = Settings.EmaCypher;
                unsubscribeUrl.Append("?ema=" + EncDec.Encrypt(mailAddress, emaCypher, true));
            }
            
            // TODO: Support the other cultureID's / locales (French and German).
            return unsubscribeUrl.ToString();
        }

        /// <summary>
        /// Return all the newsletters.
        /// </summary>
        public static List<NewsletterDal> GetAll() {
             List<NewsletterDal> newsletters = new List<NewsletterDal>();
             
             foreach (newsletter newsletter in DB.newsletter.OrderByDescending(n => n.DateCreated)) {
                newsletters.Add(new NewsletterDal(newsletter));
             }

             return newsletters;
        }


        /// <summary>
        /// Return all the newsletters that were already sent.
        /// </summary>
        public static List<NewsletterDal> GetSent() {
             List<NewsletterDal> newsletters = new List<NewsletterDal>();
             
             foreach (newsletter newsletter in DB.newsletter.Where(n => n.DateSent.HasValue).OrderByDescending(m => m.DateCreated)) {
                newsletters.Add(new NewsletterDal(newsletter));
             }

             return newsletters;
        }


        /// <summary>
        /// Return all the newsletters of a certain culture.
        /// </summary>
        public static List<NewsletterDal> GetByCultureId(int? cultureId) {
            // Return all newsletters if the cultureId is empty.
            if (!cultureId.HasValue) {
                return GetAll();
            }
            
            List<NewsletterDal> newsletters = new List<NewsletterDal>();
            foreach (newsletter newsletter in DB.newsletter.Where(n => n.CultureId == cultureId).OrderByDescending(m => m.DateCreated)) {
                newsletters.Add(new NewsletterDal(newsletter));
            }
            return newsletters;
        }


        /// <summary>
        /// Return the newsletter with the given ID.
        /// </summary>
        public static NewsletterDal GetById(int id) {
            newsletter newsletter = DB.newsletter.Where(n => n.Id == id).FirstOrDefault();
            if (newsletter != null) { 
                return new NewsletterDal(newsletter);
            } else {
                return null;
            }
        }

        #endregion :: Methods
    }
}