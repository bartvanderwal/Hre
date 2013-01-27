using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HRE.Data;
using HRE.Common;

namespace HRE.Dal {
    
    /// <summary>
    /// Contains generic methods for DAL objects.
    /// </summary>
    public abstract class ObjectDal {
	
        /// <summary>
        /// The datacontext is stored in the request so it can be shared by all entities in the current request.
        /// The datacontext is lazy-loaded (e.g. it is created, if it does not exist yet).
        /// </summary>
        protected static hreEntities DB {
            get {
                return DBConnection.GetHreContext();
            }
        }

        /// <summary>
        /// The ID of an object
        /// </summary>
        // private virtual int ID;

        /// <summary>
        /// Save changes.
        /// TODO!!
        /// </summary>
        /*
    	public static void Save() {
            MeaMedicaDataContext __dc = DBConnection.GetMeaMedicaDataContext();
            
            // Add entity if it has no primary key yet
            // if (ID == null) {
            //    __dc.MailAudits.InsertOnSubmit(_mailAudit);
            // }
  		    __dc.SubmitChanges();
        }
        */

    }

}
