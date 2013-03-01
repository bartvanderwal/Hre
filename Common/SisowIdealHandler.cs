using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using HRE.Common;

namespace HRE.Business {
    /// <summary>
    // Verzorgt het verrichten en afhandelen van Ideal transacties.
    // Deze klass gaat uit van PSP = Sisow
    /// </summary>
    public static class SisowIdealHandler {

        private static string MERCHANT_ID = HreSettings.SisowMerchantId;
        private static string PASSWORD = HreSettings.SisowPassword;
        private static string MERCHANT_KEY = HreSettings.SisowMerchantKey;
        private const string PAGEURL = "https://www.sisow.nl/Sisow/iDeal/Betaal.aspx";


        // Betalingskenmerk: "Boek Word wie je bent"
        // Bepaal de URL voor get request voor Ideal transactie bij SISOW.
        public static string DetermineSisowGetUrl(int amountInCents, string purchaseId, string description, string returnurl, string issuerid) {
            
            if (purchaseId.Length>16) {
                // De purchaseId mag niet langer zijn dan 16 karakters (Sisow specificatie).
                throw new SisowIdealArgumentException("De purchaseId is langer dan 16 karakters: '" + purchaseId + "'.");
            }
            // TODO: Hier een regexp gebruiken [a-z,A-Z,0-9], oftewel: w.
            if (purchaseId.Contains('.')) {
                // De purchaseId mag alleen cijfers en letters bevatten, geen leestekens en dergelijke.
                throw new SisowIdealArgumentException("De purchaseId mag alleen cijfers en letters bevatten, geen leestekens en dergelijke: '" + purchaseId + "'.");
            }

            if (description.Length>32) {
                // De description mag niet langer zijn dan 32 karakters (Sisow specificatie).
                throw new SisowIdealArgumentException("De description is langer dan 32 karakters: '" + description + "'.");
            }

            string key = SHA1Encode(MERCHANT_ID + PASSWORD + purchaseId + amountInCents);
            string payparams = "?key=" + key;
            payparams += "&merchantid=" + MERCHANT_ID;
            payparams += "&purchaseid=" + HttpUtility.UrlEncode(purchaseId);
            payparams += "&amount=" + amountInCents;
            payparams += "&description=" + HttpUtility.UrlEncode(description);
            if (!string.IsNullOrEmpty(returnurl)) {
                payparams += "&returnurl=" + returnurl;
            }
            if (!string.IsNullOrEmpty(issuerid)) {
                payparams += "&issuerid=" + HttpUtility.UrlEncode(issuerid);
            } else {
                payparams += "&confirm=1";
            }
            // The confirmback URL param in the querystring results in the user getting an acknowledgement screen within Sisow. 
            // at the end of the his/her IDEAL transaction.
            // payparams += "&confirmback=";
            return PAGEURL + payparams;
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
        /// Get the currently valid bank names from Sisow iDEAL webservice.
        /// </summary>
        /// <returns></returns>
        public static List<String> Banks() {
            Sisow.AssurePaySoapClient ap = new Sisow.AssurePaySoapClient();
            List<String> sourceBankList = ap.GetBanken();
            List<String> destBankList = new List<String>();

            for (int i=0; i<sourceBankList.Count(); i++) {
                if (i%2==0) destBankList.Add(sourceBankList.ElementAt(i));
            }
            return destBankList;
        }


        /// <summary>
        ///  Check that the check string checks out correctly, by performing SHA1.
        /// </summary>
        /// <param name="txId">The transaction identifier</param>
        /// <param name="ec">The event code</param>
        /// <param name="status">The status code</param>
        /// <param name="check">The SHA1 checksum string</param>
        /// <param name="error">Optional message in case of error (if present than doesn't check out)</param>
        /// <returns>True if checks out, false otherwise</returns>
        public static bool DoesConfirmationCheckOut(string txId, string ec, string status, string check, string error) {
            // If the error is set or one of the other paramaters is set, than the confirmation does not check out.
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(txId) 
                 || string.IsNullOrEmpty(ec) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(check)) {
                return false;
            }
            // Perform the actual SHA1 check of txId+ec+status+password against the 'check' param.
            string checkSHA1 = SHA1Encode(txId + ec + status + PASSWORD);
            return checkSHA1.Equals(check.ToUpper());
        }
    }
}