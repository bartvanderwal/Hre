using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRE.Business;
using HRE.Dal;

namespace HRE.Admin {

    public partial class Newsletters : HREAdminPage {

        protected override void Page_Load(object sender, EventArgs e) {
            // TODO: Uncomment below.
            // base.Page_Load(sender, e);
            if (!IsPostBack) {
                LoadNewsletters();
            }
        }

    
        /// <summary>
        /// Create to edit / detail screen to create a new newsletter.
        /// </summary>
        protected void btnCreateNew_Click(object sender, EventArgs e) {
            Response.Redirect("newsletter.aspx");
        }

        /// <summary>
        /// When a command button is clicked.
        /// </summary>
        protected void CommandBtn_Click(Object sender, CommandEventArgs e) {
            switch(e.CommandName) {
                case "Delete":
                    // Call the method to delete the item.
                    int newsletterID = int.Parse((string) e.CommandArgument);
                    NewsletterDal newsletterDal = NewsletterDal.GetById(newsletterID);
                    if (newsletterDal.NotSent) { 
                        newsletterDal.Initialize();
                        newsletterDal.Delete();
                        LoadNewsletters();
                    }
                    break;
            }
        }

        /// <summary>
        /// Update newsletters list if the user changes the selected item in the culture dropdownlist.
        /// </summary>
        protected void ddlLocale_OnSelectedIndexChanged(object sender, EventArgs e) {
            LoadNewsletters();
        }

        /// <summary>
        /// Initialize controls.
        /// </summary>
        private void LoadNewsletters() {
            GVNewsletter.DataSource = NewsletterDal.GetAll();
            GVNewsletter.DataBind();
        }


        /// <summary>
        /// Gebruiker klikt op knop voor verversen van lijst met nieuwsbrieven.
        /// </summary>
        protected void reload_Click(object sender, EventArgs e) {
		    LoadNewsletters();
	    }
    }
}