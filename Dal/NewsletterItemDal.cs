using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web;
using HRE.Data;
using HRE.Business;

namespace HRE.Dal {
    public class NewsletterItemDal : ObjectDal {
        
        #region :: Members

        private newsletteritem _newsletterItem;

        #endregion :: Members


        #region :: Properties

        /// <summary>
        /// Constructor. Construct an empty e-mail newsletter item DAL object
        /// Connects it to the given newsletter and gives it the given sequencenumber.
        /// </summary>
        public NewsletterItemDal(int sequenceNumber) {
            _newsletterItem = new newsletteritem();
            SequenceNumber = sequenceNumber;
        }

        /// <summary>
        /// Constructor. Construct an e-mail newsletter item DAL object based on the DB object.
        /// </summary>
        public NewsletterItemDal(newsletteritem item) {
            _newsletterItem = item;
        }

        /// <summary>
        /// Key.
        /// </summary>
        private int ID {
            get { return _newsletterItem.Id; }
        }

        /// <summary>
        /// The id of the newsletter it belongs to.
        /// </summary>
		public int NewsletterID {
            get { return _newsletterItem.NewsletterId; }
            set { _newsletterItem.NewsletterId = value; }
        }

        /// <summary>
        /// The sequence number of this item in the newsletter that contains it.
        /// </summary>
		public int SequenceNumber {
            get { return _newsletterItem.SequenceNumber; }
            set { _newsletterItem.SequenceNumber = value; }
        }

        /// <summary>
        /// The title of the newsletter item.
        /// </summary>
		public String ItemTitle {
            get { return _newsletterItem.ItemTitle; }
            set { _newsletterItem.ItemTitle = value; }
        }

        /// <summary>
        /// The subtitle of the newsletter item.
        /// </summary>
		public String ItemSubTitle {
            get { return _newsletterItem.ItemSubTitle; }
            set { _newsletterItem.ItemSubTitle = value; }
        }

        /// <summary>
        /// The text of the newsletter item.
        /// </summary>
		public String ItemText {
            get { return _newsletterItem.ItemText; }
            set { _newsletterItem.ItemText = value; }
        }

        /// <summary>
        /// The URL of the picture in the e-mail newsletter item.
        /// </summary>
        public String PictureURL {
            get { return _newsletterItem.PictureURL; }
            set { _newsletterItem.PictureURL = value; }
        }
 
        private bool IsStoredInDB {
            get { // return ID == 0; }
                return DB.newsletteritem.Where(e => e.Id == _newsletterItem.Id).Count() == 1; }
        }

        #endregion :: Properties
        
        
        #region :: Methods

        /// <summary>
        /// Store the newsletter item in the database.
        /// </summary>
        public void Save(int parentNewsletterID) {
            // Add the entity if it has no primary key yet.
            if (!IsStoredInDB) {
                // Also set the parent id for the associative relation.
                NewsletterID = parentNewsletterID;
                DB.AddTonewsletteritem(_newsletterItem);
            }
  		    DB.SaveChanges();
        }

        /// <summary>
        /// Delete the newsletter item from the database.
        /// </summary>
        public void Delete() {
            // Add the entity if it has no primary key yet.
            if (!IsStoredInDB) {
                throw new InvalidOperationException("This newsletter item cannot be deleted because it was not yet stored in the database.");
            }
  		    DB.DeleteObject(_newsletterItem);
            DB.SaveChanges();
        }

        /// <summary>
        /// Determines whether the item is filled in cq complete.
        /// </summary>
        public bool IsFilledIn {
            get { return !string.IsNullOrEmpty(ItemTitle) && !string.IsNullOrEmpty(ItemText); }
        }
        #endregion :: Methods

        /// <summary>
        /// Create the HTML for this newsletter item.
        /// </summary>
        /// <returns>The HTML string of the newsletter item.</returns>
        public string CreateHtml(int cultureID) {
            StringBuilder txt = new StringBuilder("");
            if (IsFilledIn) {
                txt.Append("<h2>" + ItemTitle + "</h2>");
                if (!String.IsNullOrEmpty(ItemSubTitle)) {
                    txt.Append("<p style=\"font-weight: bold; \">" + ItemSubTitle + "</b><p>");
                }
                string pictureHtml = "";
                if (!String.IsNullOrEmpty(PictureURL)) {
                    pictureHtml="<img align=\"right\" vertical-align=\"top\"; style=\"margin: 0px 20px; border: 0px;\" src=\"" + GeneralUtil.DetermineDomainBaseHttp(cultureID) + "/" + PictureURL + "\" />";
                }
                // Add the picture and the text, trimming leading and trailing white space and converting newlines to HTML paragraphs.
                txt.Append("<p>" + pictureHtml + ItemText.Trim().Replace("\n","<br />") + "</p>");
                txt.Append("<br clear=\"all\" />");
                txt.Append("<hr size=\"1\" noshade color=\"#FF0000\">");
            }
            return txt.ToString();
        }

        /// <summary>
        /// Create the plain text for this newsletter item.
        /// </summary>
        /// <returns>The HTML string of the newsletter item.</returns>
        public string CreateText() {
            StringBuilder txt = new StringBuilder("");
            if (IsFilledIn) {
                txt.Append(ItemTitle + "\n");
                if (!String.IsNullOrEmpty(ItemSubTitle)) {
                    txt.Append(ItemSubTitle  + "\n");
                }
                // Add the text, trimming leading and trailing white space.
                txt.Append(ItemText.Trim());
            }
            return txt.ToString();
        }

    }
}
