using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRE.Business;
using System.Net.Mail;
using HRE.Dal;

namespace HRE.Admin {
    public partial class Dashboard : HREAdminPage {
        
        protected new void Page_Load(object sender, EventArgs e) {
        }


        /// <summary>
        /// Stuur een test mail.
        /// </summary>
        protected void SendEmail_Click(object sender, EventArgs e) {
            if (IsPostBack) {
                string subject="Test subject";
                string body = "Test body.";
                EmailSender.SendEmail("bartvanderwal@gmail.com", "noreply@hetrondjeeilanden.nl", null, null, 
                        subject, body, false, EmailCategory.Test, null, null, null);
            }
        }



    }
}