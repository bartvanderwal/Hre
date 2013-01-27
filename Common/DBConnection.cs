using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using HRE.Models;
using HRE.Data;


namespace HRE.Common {
    /// <summary>
    /// Database connection.
    /// </summary>
    public static class DBConnection {

        private static object _lock = new object();

        /// <summary>
        /// Return a new DataContext on a 'per request use case'.
        /// </summary>
        /// <returns></returns>
        public static hreEntities GetHreContext() {
            // lock (_lock) {
                string dcKey = "dcm_" + HttpContext.Current.GetHashCode().ToString();
                if (!HttpContext.Current.Items.Contains(dcKey)) {
                    HttpContext.Current.Items.Add(dcKey, new hreEntities());
                }
                return HttpContext.Current.Items[dcKey] as hreEntities;
            // }
        }
    }
}