using HRE.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace HRE.Common {
    
    public class Common {
        
        
        /// <summary>
        /// Try to convert a string to a DateTime.
        /// </summary>
        /// <param name="dateText"></param>
        /// <returns></returns>
        public static DateTime DetermineDateTime(string dateText) {
            DateTime result;

            if (!DateTime.TryParse(dateText, out result)) {
                result = new DateTime();
            }

            return result;
        }

        /// <summary>
        /// Try to convert a DateTime to a String.
        /// </summary>
        /// <param name="dateText"></param>
        /// <returns></returns>
        public static string DetermineDateTimeString(DateTime? date) {
            return date.HasValue ? string.Format("{0:d/M/yyyy}", date.Value) : string.Empty;
        }

        /// <summary>
        /// Try to convert a DateTime to a String.
        /// </summary>
        /// <param name="dateText"></param>
        /// <returns></returns>
        public static DateTime? ConvertDateTimeString(string dateString) {
            if (string.IsNullOrEmpty(dateString)) {
                return null;
            }
            DateTime result;
            if (DateTime.TryParse(dateString, out result)) {
                return result;
            }
            else {
                throw new FormatException("Datum " + dateString + " kon niet worden geconverteerd.");
            }
        }

        /// <summary>
        /// Try to convert a string to a decimal value.
        /// </summary>
        /// <param name="decimalText">The decimal as a string</param>
        /// <returns></returns>
        public static decimal? DetermineDecimal(string decimalText) {
            decimal result;
            if (decimal.TryParse(decimalText, out result)) {
                return result;
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logicalContentFileName"></param>
        /// <returns></returns>
        public static String RetrievePhysicalFilename(string logicalContentFileName) {
            string contentFile;

            // First check if the specific culture resource file (i.e. 'language-country') is available.
            contentFile = logicalContentFileName;
            if (File.Exists(contentFile)) {
                return contentFile;
            }

            // Otherwise return null;
            return null;
        }

        
        public static string RC2Encryption(string strInput, string strKey, string strIV) {
            byte[] byteInput = Encoding.UTF8.GetBytes(strInput);
            byte[] byteKey = Encoding.ASCII.GetBytes(strKey);
            byte[] byteIV = Encoding.ASCII.GetBytes(strIV);
            MemoryStream MS = new MemoryStream();
            RC2CryptoServiceProvider CryptoMethod = new RC2CryptoServiceProvider();
            CryptoStream CS = new CryptoStream(MS, CryptoMethod.CreateEncryptor(byteKey, byteIV), CryptoStreamMode.Write);
            CS.Write(byteInput, 0, byteInput.Length);
            CS.FlushFinalBlock();
            return HttpServerUtility.UrlTokenEncode(MS.ToArray());
        }


        public static string RC2Decryption(string strInput, string strKey, string strIV) {
            byte[] byteInput = HttpServerUtility.UrlTokenDecode(strInput);
            byte[] byteKey = Encoding.ASCII.GetBytes(strKey);
            byte[] byteIV = Encoding.ASCII.GetBytes(strIV);
            MemoryStream MS = new MemoryStream();
            RC2CryptoServiceProvider RC2 = new RC2CryptoServiceProvider();
            CryptoStream CS = new CryptoStream(MS, RC2.CreateDecryptor(byteKey, byteIV), CryptoStreamMode.Write);
            CS.Write(byteInput, 0, byteInput.Length);
            CS.FlushFinalBlock();
            return Encoding.UTF8.GetString(MS.ToArray());
        }


        /// <summary>
        /// Returns the domain base of the URL for the current environment: local, test or production.
        /// </summary>
        public static string GetDomainBase() {
            return HreSettings.IsProduction ? "https://www.hetrondjeeilanden.nl" : HreSettings.IsDevelopment ? "https://localhost:44301" : "http://test.hetrondjeeilanden.nl";
        }


        /// <summary>
        /// Reorder a list in random order.
        /// Source: http://stackoverflow.com/questions/6294671/asp-net-list-order-by-random
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalList"></param>
        /// <returns>The list but now in random order</returns>
        public static List<T> RandomizeList<T>(IList<T> originalList) {
            List<T> randomList = new List<T>();
            Random random = new Random();
            T value = default(T);

            //now loop through all the values in the list
            while (originalList.Count() > 0) {
                //pick a random item from th original list
                var nextIndex = random.Next(0, originalList.Count());
                //get the value for that random index
                value = originalList[nextIndex];
                //add item to the new randomized list
                randomList.Add(value);
                //remove value from original list (prevents
                //getting duplicates
                originalList.RemoveAt(nextIndex);
            }

            //return the randomized list
            return randomList;
        }

    }
}