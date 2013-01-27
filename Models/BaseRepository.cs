using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRE.Common;
using HRE.Data;

namespace HRE.Models {
    
    /// <summary>
    /// The base repository, with the connection to the database.
    /// </summary>.
    public class BaseRepository {
        
        /// <summary>
        /// The datacontext is stored in the request so it can be shared by all entities in the current request.
        /// The datacontext is lazy-loaded (e.g. it is created, if it does not exist yet).
        /// </summary>
        protected static hreEntities DB {
            get {
                return DBConnection.GetHreContext();
            }
        }
    }

}