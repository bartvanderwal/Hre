using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace HRE.Business {

    public class HREAdminPage : HREPage {

        protected virtual void Page_Load(object sender, EventArgs e) {
            if (!Roles.IsUserInRole("Admin") || (Request.IsLocal && IsDebug)) {
                // Go to not authorized page.
                Server.Transfer("~/Errors/NotAuthorized.aspx");
            }
        }
    }
}