using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRE.Attributes {

    /// <summary>
    /// Custom validation attributes.
    /// </summary>
    public class IsTrueAttribute : ValidationAttribute {
        #region Overrides of ValidationAttribute

        /// <summary>
        /// Determines whether the specified value of the object is valid. 
        /// </summary>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        /// <param name="value">The value of the specified validation object on which the <see cref="T:System.ComponentModel.DataAnnotations.ValidationAttribute"/> is declared.</param>
        public override bool IsValid(object value) {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");

            return (bool)value;
        }

        #endregion
    }

    public class EmailAttribute : RegularExpressionAttribute {
        public EmailAttribute()
            : base("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?") {
        }
    }

    /// <summary>
    /// Determines if the value is a valid year (after 1900 and before 10000), e.g. '1980' or '2113'.
    /// </summary>
    public class ValidYearNumberAttribute : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null) {
                return true;
            }
            int year;
            if (int.TryParse(value.ToString(), out year)) {
                return year >= 1900 && year < 10000; 
            } else {
                return false;
            }
        }
    }

}
