using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRE.Dal;
using HRE.Business;
using System.Net.Mail;
using System.Text;
using HRE.Common;

namespace HRE.Admin {
    public partial class Newsletter : HREAdminPage {

	    #region :: Members

        protected NewsletterDal _newsletterDal;

        public int NrOfAddressees;

	    #endregion :: Members


	    #region :: EventMethods


        protected override void Page_Load(object sender, EventArgs e) {
            base.Page_Load(sender, e);
        
            // Initialize the newsletter (create new or retrieve most current newsletter from the database).
            string id = Request.QueryString.Get("Id");
            if (id != null) {
                _newsletterDal = NewsletterDal.GetById(int.Parse(id));
            } else { 
                if (_newsletterDal==null) {
                _newsletterDal = new NewsletterDal();
                }
            }
            _newsletterDal.Initialize();

            litSequenceNumber.Text = DetermineSequenceNumber().ToString();


            if (!IsPostBack) {
                if (!NewsletterIsReadOnly()) {
                    // Otherwise go to the edit (=first=landing) screen and set the screen values.
                    GoToEditStep();
                } else {
                    // If so go to the preview screen and set the screen values (navigation buttons have been disabled there, so the newsletter can no longer be edited).
                    GoToPreviewStep();
                }
            }
        }

            /// <summary>
    /// Change the sequence number on update of selected culture.
    /// </summary>
    protected void ddlLocale_OnSelectedIndexChanged(object sender, EventArgs e) {
        litSequenceNumber.Text = DetermineSequenceNumber().ToString();
        // cfImage.CultureID = _newsletterDal.CultureID;
    }

    /// <summary>
    /// Determine the sequence numer as n+1, where n = the nr of existing newsleters for the selected locale.
    /// </summary>
    private int DetermineSequenceNumber() {
        int nrOfNewslettersWithCurrentCulture = NewsletterDal.GetAll().Count();

        // Determine whether the newsletter was already saved.
        if (_newsletterDal.IsStoredInDB) {
            return nrOfNewslettersWithCurrentCulture;
        } else {
            return nrOfNewslettersWithCurrentCulture+1;
        }
    }


        /// <summary>
        /// The user wants to go back to the newsletteroverview screen (from the first screen of the wizard).
        /// Note that the newsletter is NOT saved.
        /// </summary>
        protected void btnBackToOverview_Click(object sender, EventArgs e) {
            GoToOverviewScreen();
        }


        /// <summary>
        /// The user wants to go back to the newsletteroverview screen (from the first screen of the wizard).
        /// Note that the newsletter is NOT saved.
        /// </summary>
        protected void btnBackToOverviewFromPreviewStep_Click(object sender, EventArgs e) {
            GoToOverviewScreen();
        }

        /// <summary>
        /// Go back to the newsletter overview screen.
        /// </summary>
        private void GoToOverviewScreen() {
            Response.Redirect("newsletters.aspx");
        }

        private void SetItemsInPreviewStep() {
            // Set the dates.
            lbDateCreated.Text = _newsletterDal.DateCreated.ToShortDateString();
            lbDateSent.Text = _newsletterDal.DateSent.HasValue ? _newsletterDal.DateSent.Value.ToShortDateString() : "-";

            // Set the text of the preview area to the HTML text of the newsletter.
            litNewsletterPreview.Text = _newsletterDal.CreateHtml();

            // Set test e-mail address to default if it is empty;
            if (string.IsNullOrEmpty(tbTestEmailAddress.Text)) {
                tbTestEmailAddress.Text = Settings.NewsletterTestMailAddresses;
            }
        }

        /// <summary>
        /// Set the items on the first screen.
        /// </summary>
        private void SetItemsInEditStep() {
            if (_newsletterDal.IsStoredInDB) {

                // ddlLocale.SelectedValue = _newsletterDal.CultureID.ToString();
                tbTitle.Text = _newsletterDal.Title;

                // Initialize the items and set the fields on screen accordingly.
                tbItem1Title.Text = _newsletterDal.Item1.ItemTitle;
                tbItem1Text.Text = _newsletterDal.Item1.ItemText;
                tbItem1Picture.Text = _newsletterDal.Item1.PictureURL;
                tbItem2Title.Text = _newsletterDal.Item2.ItemTitle;
                tbItem2Text.Text = _newsletterDal.Item2.ItemText;
                // TODO: Remove temporary test picture.
                tbItem2Picture.Text = _newsletterDal.Item2.PictureURL ?? "Site/Images/Upload/random01.jpg";
                tbItem3Title.Text = _newsletterDal.Item3.ItemTitle;
                tbItem3Text.Text = _newsletterDal.Item3.ItemText;
                tbItem3Picture.Text = _newsletterDal.Item3.PictureURL;
            }
        }

        private bool NewsletterIsReadOnly() {
            return _newsletterDal.DateSent != null;
        }

        protected void Page_Init(object sender, EventArgs e) {
        }
    
        /// <summary>
        /// Save the newsletter when user clicks 'Save'.
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e) {
            SaveNewsletter();

            DateTime savedTime = DateTime.Now;
            lbSavedMessage.Text = "Nieuwsbrief opgeslagen om " + savedTime.ToShortTimeString() + ".";
            lbSavedMessage.Visible = true;

        }

        /// <summary>
        /// Show HTML preview of the newsletter with the titles and texts filled in by the user.
        /// </summary>
        protected void btnGoToPreview_Click(object sender, EventArgs e) {        
            // First save the newsletter
            SaveNewsletter();

            GoToPreviewStep();
        }


        /// <summary>
        /// Go back to editing the newsletter from the preview screen.
        /// </summary>
        protected void btnBackToEdit_Click(object sender, EventArgs e) {        
            GoToEditStep();
        }


        /// <summary>
        /// Send a test newsletter with current titles and text to the specified test addresses.
        /// </summary>
        protected void btnSendTestMail_Click(object sender, EventArgs e) {        
            // Save the newsletter to be able to show fields if it was not stored before, 
            // or to show new data in case user updated it since last save.
            SaveNewsletter();

            // Fetch the test addresses.
            string emailAddresses = tbTestEmailAddress.Text;

            if (!string.IsNullOrEmpty(tbTestEmailAddress.Text)) {
                List<string> emailAddresList = emailAddresses.Split(',').ToList();

                DateTime sendTime = DateTime.Now;

                // Send the mail to the test mail adress(es).
                SendNewsletters(emailAddresList, true);

                if (emailAddresList.Count==1) {
                    lbSendMessage.Text = "Handmatig e-mail verzonden naar " + emailAddresses + " om " + sendTime.ToShortTimeString() + ".";
                } else {
                    lbSendMessage.Text = "Handmatig e-mails verzonden naar " + emailAddresses + " om " + sendTime.ToShortTimeString() + ".";
                }
            } else {
                lbSendMessage.Text = "Fout! Geef één of meer handmatige e-mail adressen op.";
            }
            lbSendMessage.Visible = true;
        }


        /// <summary>
        /// Determine and show the addressees for the newsletter based on the selected CultureID.
        /// </summary>
        protected void btnGoToAdressees_Click(object sender, EventArgs e) {
            SaveNewsletter();

            // NrOfAddressees        
            List<string> addressees = DetermineAddressees();
            NrOfAddressees = addressees.Count();
            blAddressees.DataSource = addressees;
            blAddressees.DataBind();

            GoToAddresseesStep();
        }


        /// <summary>
        /// Go back to the preview step from the addressees screen.
        /// </summary>
        protected void btnBackToPreview_Click(object sender, EventArgs e) {        
            GoToPreviewStep();
        }
    

        /// <summary>
        /// Send the newsletter with current titles and text.
        /// </summary>
        protected void btnSendNewsletter_Click(object sender, EventArgs e) {        
            // Save the newsletter to be able to show fields if it was not stored before, 
            // or to show new data in case user updated it since last save.
            SaveNewsletter();

            // Determine the addressees and sent the mail.
            List<string> addressees = DetermineAddressees();
            SendNewsletters(addressees, false);

            // Set the date sent of the newsletter and go to the last step.
            _newsletterDal.DateSent = System.DateTime.Now;

            // Then save the newsletter (to store DateSent).
            SaveNewsletter();

            GoToConfirmationStep();
        }

        /// <summary>
        /// Send the newsletter with current titles and text.
        /// </summary>
        protected void btnSendNewsletterDirect_Click(object sender, EventArgs e) {        
            // Determine the addressees and sent the mail.
            List<string> addressees = DetermineAddressees();
            SendNewsletters(addressees, false);

            // Set the date sent of the newsletter and go to the last step.
            _newsletterDal.DateSent = System.DateTime.Now;

            // Then save the newsletter (to store DateSent).
            SaveNewsletter();

            GoToConfirmationStep();
        }


        #endregion :: EventMethods


        #region :: Methods

        /// <summary>
        /// Save the newsletter with current filled in titles and texts.
        /// </summary>
        private void SaveNewsletter() {
            // Set the newsletter items according to values on screen and then save.
            _newsletterDal.Title = tbTitle.Text;
            // _newsletterDal.CultureID = int.Parse(ddlLocale.SelectedValue);

            _newsletterDal.SequenceNumber = int.Parse(litSequenceNumber.Text);
            // Set the newsletter items according to values on screen and then save.
            _newsletterDal.Item1.ItemTitle = tbItem1Title.Text;
            _newsletterDal.Item1.ItemText = tbItem1Text.Text;
            _newsletterDal.Item1.PictureURL = tbItem1Picture.Text;
            _newsletterDal.Item2.ItemTitle = tbItem2Title.Text;
            _newsletterDal.Item2.ItemText = tbItem2Text.Text;
            _newsletterDal.Item2.PictureURL = tbItem2Picture.Text;
            _newsletterDal.Item3.ItemTitle = tbItem3Title.Text;
            _newsletterDal.Item3.ItemText = tbItem3Text.Text;
            _newsletterDal.Item3.PictureURL = tbItem3Picture.Text;

            _newsletterDal.Save();
        }

        /// <summary>
        /// Create the newsletter and send it to a list of recipients
        /// </summary>
        /// <param name="addressees">String with an e-mail adress or (comma-seperated) list of multiple e-mail addresses.</param>
        /// <param name="isTest">Boolean indicating whether this is a test newsletter (true) or actual newsletter (false) to person from the customer/user list.</param>
        private void SendNewsletters(List<string> addressees, bool isTest) {
            // Create the basic HTML mail for the mail (fixed part).
            string bodyWithoutUnsubscribeLink = _newsletterDal.CreateHtml();

            // Set the mail and send it to the addressee.
            MailMessage message = new MailMessage();

            // TODO: Get the 'noreply' e-mail adress from a property. Make it localizable?
            message.From = new MailAddress(HreSettings.ReplyToAddress, "Het Rondje Eilanden");
            message.Subject = _newsletterDal.Title;
            message.IsBodyHtml = true;

            // Send the e-mails to all addressees.
            foreach (string address in addressees) {
                // Finish the HTML with the 'dynamic' unsubscribe link at the bottom of the mail. The link depends on the addressee's e-mail adddress.
                // TODO Make multi-language.
                StringBuilder body = new StringBuilder(bodyWithoutUnsubscribeLink);
                body.Append("<br clear=\"all\"><p style=\"font-size: 11px;\" ><br/>");
                body.Append("Geen interesse? Klik ");
                body.Append("<a href=\"" + _newsletterDal.DetermineUnsubscribeLink(address) + "\">");
                body.Append("hier");
                body.Append("</a> om je af te melden voor de nieuwsbrief</p>");

                // Set the message to address to the current e-mail adres.
                message.To.Clear();
                message.To.Add(new MailAddress(address.Trim()));
                message.Body = body.ToString();
            
                EmailCategory emailCategory = isTest ? EmailCategory.NewsletterTest : EmailCategory.Newsletter;
                EmailSender.SendEmail(message, EmailCategory.Newsletter, _newsletterDal.ID);
            }
        }


        /// <summary>
        /// Determine the e-mail addresses of the relevant customer for this newsletters and put them in a list.
        /// </summary>
        private List<string> DetermineAddressees() {
            // Send the mail to the customers.
            List<string> addressees = new List<string>();

            foreach (LogonUserDal user in LogonUserDal.GetMailingListMembers()) {
                    // Add the e-mail adress to the list of addressees.
                    addressees.Add(user.EmailAddress);
            }
            return addressees;
        }


        /// <summary>
        /// Go to the wizard step for editing the newsletter.
        /// </summary>
        public void GoToEditStep() {
            SetItemsInEditStep();
            NewsletterWizard.ActiveStepIndex = 0;
        }


        /// <summary>
        /// Go to the wizard step for previewing the newsletter.
        /// </summary>
        public void GoToPreviewStep() {
            SetItemsInPreviewStep();

            // Hide the navigation buttons invisible the newsletter is read only (was sent previously) 
            // to prevent the user from being able to resent and/or change the newsletter.
            // divButtonsInPreviewStep.Visible = !NewsletterIsReadOnly();
            bool showButtons = !NewsletterIsReadOnly();
            btnBackToEdit.Visible = showButtons;
            btnGoToAdressees.Visible = showButtons;
            btnSendNewsletterDirect.Visible = showButtons;
            btnBackToOverviewFromPreviewStep.Visible = NewsletterIsReadOnly();

            NewsletterWizard.ActiveStepIndex = 1;
        }


        /// <summary>
        /// Go to the wizard step for showing the addressees.
        /// </summary>
        public void GoToAddresseesStep() {
            NewsletterWizard.ActiveStepIndex = 2;
        }

    
        /// <summary>
        /// Go to the wizard step for showing the addressees.
        /// </summary>
        public void GoToConfirmationStep() {
            NewsletterWizard.ActiveStepIndex = 3;
        }
    
        #endregion :: Methods
    }
}