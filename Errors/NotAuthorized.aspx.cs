using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace HRE.Site {
    public partial class NotAuthorized : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// Log on the user and go to next step if succesful.
        /// </summary>
        protected void LogonUser_Click(object sender, EventArgs e) {
            if (Page.IsValid) {
                // Go to the next step in the wizard.                
                string userName = UserNameExisting.Text;
                string password = PasswordExisting.Text;
                if (Membership.ValidateUser(userName, password)) {
                    FormsAuthentication.SetAuthCookie(userName, true);
                    LogOnExistingUser.Visible = false;
                    ExistingUserLoggedInMessage.Visible = true;
                }
            }
        }

        /// <summary>
        /// Custom validation to check the password for an existing user is correct.
        /// </summary>
        protected void validateExistingPasswordCorrect(Object sender, ServerValidateEventArgs args) {
            string userName = UserNameExisting.Text;
            string password = PasswordExisting.Text;
            args.IsValid = Membership.ValidateUser(userName, password);
            // Onderstaande kan niet gecontroleerd worden ivm beveiligingsniveau applicatie.. :?
            // && Roles.FindUsersInRole("Admin", userName).Length!=0;
        }

    }
}