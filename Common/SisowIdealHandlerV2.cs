using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.Mvc;

using HRE.Common;
using HRE.Sisow;
using System.Text.RegularExpressions;

namespace HRE.Business {
    /// <summary>
    // Verzorgt het verrichten en afhandelen van Ideal transacties obv v2.0 e.g. https://www.sisow.nl/Sisow/sisow.asmx 
    // Deze klasse gaat uit van PSP = Sisow.
    /// </summary>
    public static class SisowIdealHandlerV2 {

        private static string _merchantID = HreSettings.SisowMerchantId;
        private static string _password = HreSettings.SisowPassword;
        private static string _merchantKey = HreSettings.SisowMerchantKey;
        private const string _pageUrl = "https://www.sisow.nl/Sisow/sisow.asmx";


        /// <summary>
        /// Bepaal de URL voor get request voor de Ideal transactie bij Sisow.
        /// </summary>
        /// <param name="amountInCents"></param>
        /// <param name="purchaseId"></param>
        /// <param name="description"></param>
        /// <param name="returnUrl"></param>
        /// <param name="issuerid"></param>
        /// <returns></returns>
        public static string DetermineSisowGetUrl(int amountInCents, string transactionID, string description, string returnUrl, string issuerid) {
            if (transactionID.Length>16) {
                // De purchaseId mag niet langer zijn dan 16 karakters (Sisow specificatie).
                throw new SisowIdealArgumentException("De transactionID is langer dan 16 karakters: '" + transactionID + "'.");
            }
            // De purchaseId mag alleen cijfers en letters bevatten, geen leestekens en dergelijke.
            if (Regex.IsMatch(transactionID, "^/w+$")) {
                throw new SisowIdealArgumentException("De transactionID mag alleen cijfers en letters bevatten, geen leestekens en dergelijke: '" + transactionID + "'.");
            }

            if (description.Length>32) {
                // De description mag niet langer zijn dan 32 karakters (Sisow specificatie).
                throw new SisowIdealArgumentException("De description is langer dan 32 karakters: '" + description + "'.");
            }


            string key = SHA1Encode(_merchantID + _password + transactionID + amountInCents);
            string sisowUrl = string.Empty;
            string purchaseIDBeforeCall = transactionID;

            string purchaseID = null;
            using (SisowV2.sisowSoapClient service = new SisowV2.sisowSoapClient()) {
                int result = service.GetURL(_merchantID, _merchantKey, "", issuerid, amountInCents, transactionID, description, null, returnUrl, null, null, null, out purchaseID, out sisowUrl);
                if (result!=0) {
                    throw new ArgumentException(string.Format("Error: error code {0} returned by 'GetUrl(...)' method.", result));
                }
            }
            /* if (purchaseID!=purchaseIDBeforeCall) {
                throw new ArgumentException(string.Format("The webservice 'changed' the purchaseID from {0} to {1}.", purchaseIDBeforeCall, purchaseID));
            } */
            return sisowUrl;
        }


        /// <summary>
        /// Returns the SHA1 (Secure Hash Algorithm 1) encoded string of the input string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1Encode(string str) {
            string hashMethod = "SHA1";
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, hashMethod);
        }


        /// <summary>
        /// Get the currently valid bank names and id's from the Sisow iDEAL webservice.
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetIssuerList() {
            List<SelectListItem> result = new List<SelectListItem>();
            
            using (SisowV2.sisowSoapClient service = new SisowV2.sisowSoapClient()) {
                SisowV2.ArrayOfString banks = new SisowV2.ArrayOfString();
                // List<string> banks = service.GetIssuers(HreSettings.IsDevelopment).ToList();
                int dummy = service.GetIssuers(HreSettings.IsDevelopment, out banks);
                
                // Add empty item (for placeholder).
                result.Add(new SelectListItem());

                for (int i = 0; i < banks.Count(); i = i+2) {
                    string bankText = banks[i];
                    string bankValue = banks[i+1];
                    result.Add(new SelectListItem() {
                        Text = bankText,
                        Value= bankValue
                    });
                }
            }
            return result;
        }


        /// <summary>
        ///  Check that the check string checks out correctly, by performing SHA1.
        /// </summary>
        /// <param name="txId">The transaction identifier</param>
        /// <param name="ec">The event code</param>
        /// <param name="status">The status code</param>
        /// <param name="check">The SHA1 checksum string</param>
        /// <param name="error">Message in case of error (if present than doesn't check out)</param>
        /// <param name="isCheckSumValid">Out parameters indicates whether the checksum is valid (true), or invalid (false) or not applicable (null)( asin the case no URL's params were present.</param>
        /// <returns>True if checks out, false otherwise</returns>
        public static bool DoesConfirmationCheckOut(string txId, string ec, string status, string sha1, string error, out bool? isCheckSumValid) {
            isCheckSumValid = null;
            // If the error is set or one of the other paramaters is set, than the confirmation does not check out.
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(txId) 
                 || string.IsNullOrEmpty(ec) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(sha1)) {
                return false;
            }

            // Perform the actual SHA1 check of txId+ec+status+password against the 'check' param.
            string hashee = txId + ec + status + _merchantID + _merchantKey;
            string checkSHA1 = SHA1Encode(hashee);

            isCheckSumValid = checkSHA1.Equals(sha1.ToUpper());
            return isCheckSumValid.Value;
        }

    }
}