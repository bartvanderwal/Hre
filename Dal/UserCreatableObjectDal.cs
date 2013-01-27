using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Serialization;

namespace HRE.Dal {
    
    /// <summary>
    /// Extension for entities creatable by (admin or end) users through the interface.
    /// These contain a DateCreated and DateUpdated field for auditing purposes.
    /// </summary>
    [DataContract]
    public abstract class UserCreatableObjectDal : ObjectDal {
	
        /// <summary>
        /// The date (time) the entity was first created.
        /// </summary>
        public abstract DateTime DateCreated { get; }


        /// <summary>
        /// The date (time) the e-mail newsletter was last updated.
        /// </summary>
        public abstract DateTime DateUpdated { get; }

    }

}
