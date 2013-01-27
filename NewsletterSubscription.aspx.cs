using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Profile;
using System.Configuration;
using HRE.Business;
using HRE.Dal;
using HRE.Data;


namespace HRE {

    public partial class NewsletterSubscription : HREPage {

        private const string ENCRYPTED_EMAIL_PARAM = "ema";
        private const string EMAIL_PARAM = "ma";

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                string emailAddress = "";
                if (!string.IsNullOrEmpty(Request.Params[ENCRYPTED_EMAIL_PARAM])) {
                    //?ema=zhWvMu2rsfIkuZr7%2fRDj9GgXT66qad9I7bmrUebcBIRPy%2bklU60JbtKTwucXSWPkES%2binKyr8BBRRgigXGP9OQ%3d%3d will result in b.smulders@insightpharma.nl
                    string emailEnc = Request.Params[ENCRYPTED_EMAIL_PARAM];
                
                    String emaCypher = Settings.EmaCypher;

                    try {
                        emailAddress = EncDec.Decrypt(emailEnc, emaCypher, true);
                    } catch (System.FormatException) {
                        // TODO: Show user notification.
                        // For now we do nothing, just leave tbEmail empty. Probably the URL has been tampered with leading to invalid ema URL parameter.
                    }
                } else if (!string.IsNullOrEmpty(Request.Params[EMAIL_PARAM]) && Settings.AllowUnencryptedEmailForNewsletterSubscription) {
                        emailAddress = Request.Params[EMAIL_PARAM];
                    } else {
                        // Stop if no EMA or MA was found.
                        emailAddress = "";
                        return;
                    }
                tbEmail.Text = emailAddress;
                DetermineSubscriptionStatus();
            }

            // Show the edit panel for admin users.
            pnlEditButtons.Visible = User.IsInRole(AppConstants.UserRoles.ADMINISTRATOR);
        }


        protected void DetermineSubscriptionStatus() {
            bool? isSubscribed = GetSubscription(tbEmail.Text);

            // Show the subscribe or unsubscribe button depending on wheter user is subscribed or not (both, if setting not yet set).
            btnUnsubscribe.Visible = isSubscribed.HasValue ? isSubscribed.Value : true;
            btnSubscribe.Visible = isSubscribed.HasValue ? !isSubscribed.Value : true;
        }


        /// <summary>
        /// Gebruiker klik op unsubscribe. Afmelden van gebruiker wiens e-mail adres nu in de tekstbox staat.
        /// </summary>
        protected void btnUnsubscribe_Click(object sender, EventArgs e) {
            bool isUnsubscribed = SetSubscription(false, tbEmail.Text);

            pnlSubscriptionInput.Visible = true;
            pnlUnsubscribeSucceeded.Visible = isUnsubscribed;
            pnlSubscribeSucceeded.Visible = false;
            lblUnsubscribeFailed.Visible = !isUnsubscribed;

            btnSubscribe.Visible = isUnsubscribed;
            btnUnsubscribe.Visible = !isUnsubscribed;
        }


        /// <summary>
        /// Gebruiker klikt op unsubscribe. Afmelden van gebruiker wiens e-mail adres nu in de tekstbox staat.
        /// </summary>
        protected void btnSubscribe_Click(object sender, EventArgs e) {
            bool isSubscribed = SetSubscription(true, tbEmail.Text);

            pnlSubscriptionInput.Visible = true;
            pnlSubscribeSucceeded.Visible = isSubscribed;
            pnlUnsubscribeSucceeded.Visible = false;
            lblSubscribeFailed.Visible = !isSubscribed;

            btnSubscribe.Visible = !isSubscribed;
            btnUnsubscribe.Visible = isSubscribed;
        }


        
        /// <summary>
        /// Gebruiker klik op unsubscribe. Afmelden van gebruiker wiens e-mail adres nu in de tekstbox staat.
        /// Geeft true terug als het gelukt is.
        /// </summary>
        protected bool SetSubscription(bool isSubscribed, string userEmail) {
            LogonUserDal user = LogonUserDal.GetByEmailAddress(userEmail);
            if (user!=null) { 
                user.IsMailingListMember = isSubscribed;
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// Bepaal of een gebruiker is aangemeld voor de nieuwsbrief.
        /// Geeft true terug als de gebruiker zich heeft aangemeld voor de nieuwsbrief en false als deze zich heeft afgemeld.
        /// Geeft null terug als de gebruiker geen profiel heeft, de nieuwsbrief lidmaatschap niet expliciet is geset of de gebruiker niet gevonden is.
        /// </summary>
        protected bool? GetSubscription(string userEmail) {
            LogonUserDal user = LogonUserDal.GetByEmailAddress(userEmail);
            if (user!=null) { 
                return user.IsMailingListMember;
            }
            
            // Return nothing if the user account was not found.
            return null;
        }


        /// <summary>
        /// Beheerder gebruikt de 'edit e-mail adres' knop.
        /// </summary>
        protected void btnEditEmailAddress_Click(object sender, EventArgs e) {
            if (User.IsInRole(AppConstants.UserRoles.ADMINISTRATOR)) {
                tbEmail.Enabled = true;
                btnEditEmailAddress.Enabled = false;
                btnCheckStatus.Visible = true;
            }
        }

        /// <summary>
        /// Beheerder gebruikt de 'check status' knop.
        /// </summary>
        protected void btnCheckStatus_Click(object sender, EventArgs e) {
            if (User.IsInRole(AppConstants.UserRoles.ADMINISTRATOR)) {
                DetermineSubscriptionStatus();
                btnCheckStatus.Visible = false;
                btnEditEmailAddress.Enabled=true;
                tbEmail.Enabled = false;

            }

        }


    }
}