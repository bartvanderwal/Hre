using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HRE.Business {
    
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class TypeExtensions {

        /// <summary>
        /// Shorten the string if it is longer than a certain length.
        /// If it is longer then the string '...' is postfixed behind the result as an indication to the user.
        /// </summary>
        public static string Shorten(this string s, int length) {
            string shortenedPostfix = "...";
            if (s.Length > length && length>shortenedPostfix.Length) {
                return s.Substring(0, length - shortenedPostfix.Length) + shortenedPostfix; 
            } else {
                return s;
            }
        }

        /// <summary>
        /// Shorten the string if it is longer than 100 (default value, use override to specify custom length) .
        /// If it is longer then the string '...' is postfixed behind the result as an indication to the user.
        /// </summary>
        public static string Shorten(this string s) {
            return Shorten(s, 100);
        }


        /// <summary>
        /// Trim the string, and even remove leading and trailing non-breaking-spaces (&nbsp;'s). 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimThisShit(this string s) {
            return s.Replace("&nbsp;", " ").Trim();
        }


        /// <summary>
        /// Check if the given string is a valid email address, based on a RegEx pattern.
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>true if string is an email | false if string is not an email</returns>
        public static bool IsValidEmail(this string s) {
            return Regex.IsMatch(s, "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
        }


        /// <summary>
        /// Uppercase the first letter.
        /// </summary>
        /// <param name="s">word</param>
        /// <returns>Word</returns>
        public static string UppercaseFirst(this string s) {
	        if (string.IsNullOrEmpty(s)) {
	            return string.Empty;
	        }
    	    char[] a = s.ToCharArray();
	        a[0] = char.ToUpper(a[0]);
	        return new string(a);
        }


        /// <summary>
        /// This function extends DateTime and formats a date as "vandaag", "gisteren", etc. 
        /// or just the date for longer than 4 days.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string RelativeDateDescription(this DateTime date) {
            DateTime currentDate = DateTime.Now.Date;

            // Determine difference in days...
            int daysApart = Convert.ToInt32(currentDate.Subtract(date.Date).TotalDays);

            if (daysApart<30) {
                switch (daysApart) {
            	    case 0: return RelativeDateTimeDescription(date);
            	    case 1: return "gisteren";
            	    case 2: return "eergisteren";
                    default: return string.Format("{0} dagen geleden ({1})", daysApart, date.NoYearMonthAsLetters());
                }
            }
            if (daysApart<100) {
                int weeksApart = daysApart/7;
                return string.Format("{0} weken geleden ({1})", weeksApart, date.NoYearMonthAsLetters());
            }
            if (daysApart<365) {
                int monthsApart = daysApart/30;
                return string.Format("{0} maanden geleden ({1})", monthsApart, date.YearAndMonthAsLetters());
            }
            if (daysApart<1000) {
                int yearsApart = daysApart/365;
                return string.Format("{0} jaar geleden ({1})", yearsApart, date.YearAndMonthAsLetters());
            } else {
                return date.YearAndMonthAsLetters();
            }
        }

        
        public static string NoYearMonthAsLetters (this DateTime date) {
            return date.ToString("dd MMMM");
        }


        public static string YearAndMonthAsLetters (this DateTime date) {
            return date.ToString("dd MMMM yyyy");
        }

        public static string RelativeDateTimeDescription(this DateTime date) {
            DateTime currentDateTime = DateTime.Now;
            if (currentDateTime.Date != date.Date) {
                // More than a day difference, show the date.
                return date.RelativeDateDescription();
            }

            // Determine the difference in seconds...
            int numberOfSecondsAgo = date.NumberOfSecondsAgo();

            // See if the date dt is within the last hour...
            if (numberOfSecondsAgo < 60)
                return "zojuist";
            if (numberOfSecondsAgo < 120)
                return "een minuut geleden";
            if (numberOfSecondsAgo < 3600)
                return string.Format("{0:N0} minuten geleden", numberOfSecondsAgo / 60 + 1);
            return string.Format("{0:N0} uur geleden", numberOfSecondsAgo / 3600 + 1);
        }

        /// <summary>
        /// Turns 340 into 3,40 and 500 into € 5,- and so on.
        /// </summary>
        /// <param name="bedragInCenten">int amount to convert to formatted euro amount</param>
        /// <returns></returns>
        public static string AsAmount(this int bedragInCenten) {
            string result = string.Format("{0:F}", ((double) bedragInCenten)/100);
            result = result.Replace(".",",");
            result = result.Replace(",00",",- ");
            return "€ " + result;
        }


        /// <summary>
        /// Turns 340 into '€ 3,40' and '500' into € 5,- and so on (null becomes '?'.
        /// </summary>
        /// <param name="bedragInCenten">int amount to convert to formatted euro amount</param>
        /// <returns></returns>
        public static string AsAmount(this int? bedragInCenten) {
            return bedragInCenten.HasValue ? bedragInCenten.Value.AsAmount() : "?";
        }

        /// <summary>
        /// Returns the number of days that a certain day is ago from the current (system) date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int NumberOfDaysAgo(this DateTime date) {
             return Convert.ToInt32(DateTime.Now.Date.Subtract(date.Date).TotalDays);
        }


        /// <summary>
        /// Returns the date that was the given number of days ago from the current (system) date.
        /// </summary>
        public static DateTime DateThisNumberOfDaysAgo(this int numberOfDaysAgo) {
             return DateTime.Now.AddDays(-numberOfDaysAgo);
        }

        /// <summary>
        /// Returns the number of seconds that a certain datetime is ago from the current (system) datetime.
        /// <param name="date"></param>
        /// <returns></returns>
        public static int NumberOfSecondsAgo(this DateTime date) {
            return Convert.ToInt32(DateTime.Now.Subtract(date).TotalSeconds);
        }


        /// <summary>
        /// Returns the number of hours that a certain datetime is ago from the current (system) datetime.
        /// <param name="date"></param>
        public static int NumberOfHoursAgo(this DateTime date) {
            return Convert.ToInt32(DateTime.Now.Subtract(date).TotalHours);
        }

    }
}
