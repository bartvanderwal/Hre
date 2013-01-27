using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.IO;
using System.Globalization;

namespace HRE.Business {

    /// <summary>
    // Genereert een staafdiagram.
    // Deze klasse gebruikt de Google Chart API. Zie de uitgebreide online documentatie.
    /// </summary>
    public static class GeneralUtil {

        /// <summary>
        /// Determine if there is more than x days difference between two dates.
        /// </summary>
        public static bool IsHoursBetweenDateTimesGreaterThan(DateTime startTime, DateTime endTime, int minDiff) {
            return endTime.Subtract(startTime).Hours >= minDiff;
        }

        /// <summary>
        /// Format the price.
        /// </summary>
        /// <param name="amountInCents"></param>
        /// <returns></returns>
        public static string FormatPrice(int amountInCents) {
            decimal amountWithDecimals = ((decimal) amountInCents) / 100;
            return String.Format("{0:C}", amountWithDecimals);
        }

        /// <summary>
        /// Convert all postal codes, e.g. '1234ed', to format '1234 DF' (so with a space and all caps).
        /// </summary>
        public static string FormatDutchPostalcode(string input) {
            // Trim leading and trailing spaces
            input = input.Trim();
            
            // Insert space between numeric and alphanumeric part, if not yet present.
            if (input.Length==6) {
                input = input.Substring(0,4) + ' ' + input.Substring(4, 2);
            }
            
            // Convert to upper case (only applies to alphanumeric part) and return.
            return input.ToUpper();
        }

        /// <summary>
        /// Convert all first letters of Name or Names to an upper case (for first and last names)
        /// </summary>
        public static string FormatName(string input) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        /// <summary>
        /// Determine the base domain name, given the cultureID
        /// </summary>
        public static string DetermineDomainBase(int cultureID) {
            if (Settings.IsDevelopment) {
                return "localhost:8081";
            }

            return "hetrondjeeilanden.nl";
        }


        /// <summary>
        /// Determine the base domain name including www prefix - if relevant - given the cultureID
        /// </summary>
        public static string DetermineDomainBaseWww(int cultureID) {
            string domainBase = DetermineDomainBase(cultureID);
            return HttpContext.Current.IsDebuggingEnabled? domainBase : "www." + domainBase;
        }


        /// <summary>
        /// Determine the base domain name with http:// prefix, given the cultureID.
        /// </summary>
        public static string DetermineDomainBaseHttp(int cultureID) {
            string domainBaseWww = DetermineDomainBaseWww(cultureID);
            return "http://" + domainBaseWww;
        }
    }
}
