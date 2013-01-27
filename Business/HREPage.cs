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
using HRE.Data;
using HRE.Common;

namespace HRE.Business {

    public class HREPage : System.Web.UI.Page {
        protected logonuser _loggedOnUser;
        protected hreEntities _db;

        // Properties.
        public MembershipUser MembershipUser {
            get {
                return Membership.GetUser();
            }
        }

        // Indicates whether we are in debug mode.
        public bool IsDebug {
            get { return Request.QueryString["debug"]=="1"; }
        }

        public hreEntities Db {
            get {
                if (_db == null) {
                    _db = DBConnection.GetHreContext();
                }
                return _db;
            }
        }

        public logonuser LoggedOnUser {
            get {
                if (_loggedOnUser == null) {
                    if (MembershipUser!=null) {
                        string userKey = MembershipUser.ProviderUserKey.ToString();
                        logonuser user = Db.logonuser.Where(u => u.ExternalId == userKey).FirstOrDefault();
                        if (user == null) {
                            throw new Exception("User with userkey '" + userKey + "' was not found in the 'user' table");
                        }
                        _loggedOnUser = user;
                    }
                }
                return _loggedOnUser;
            }
        }
    }
}