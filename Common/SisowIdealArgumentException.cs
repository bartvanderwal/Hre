using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRE.Business {
    
    /// <summary>
    /// Custom Ecception class to catch errors in arguments to call for Sisow IDEAL transaction.
    /// Before the actual call is made, or wrapping possible exception returned during or after the call.
    /// </summary>
    public class SisowIdealArgumentException : Exception {

        protected string _errorMessage;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SisowIdealArgumentException(string errorMessage) {
            _errorMessage = errorMessage;
        }

        public override string  Message {
	        get { 
		        return "Fout in SISOW call voortijdig afgevangen: " + _errorMessage;
	        }
        }
    }
}