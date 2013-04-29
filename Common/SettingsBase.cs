using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace HRE.Common {

    public class SettingsBase {

        /// <summary>
        /// Read a boolean setting from the appsettings. The setting must be set to "True", "true", "TRUE" or something like that, for other values or if it is not present it will be regarded as False.
        /// </summary>
        /// </summary>
        protected static bool ReadBoolSetting(string settingKey) {
            return ReadBoolSetting(settingKey, false);
        }


        /// <summary>
        /// Read a boolean setting from the appsettings. The setting must be set to "True", "true", "TRUE" or something like that, for other values or if it is not present it will be regarded as False.
        /// </summary>
        /// </summary>
        protected static bool ReadBoolSetting(string settingKey, bool defaultValue) {
            bool boolSettingAsBool;
            string boolSettingAsString = ConfigurationManager.AppSettings[settingKey];
            if (!string.IsNullOrEmpty(boolSettingAsString) && bool.TryParse(boolSettingAsString, out boolSettingAsBool)) {
                return boolSettingAsBool;
            } else {
                return defaultValue;
            }
        }


        /// <summary>
        /// Read an int setting from the settings and allow for a default value.
        /// </summary>
        protected static int ReadIntSetting(string settingKey, int defaultValue) {
            int result;
            // Try to read the setting from the appsettings file.
            if (int.TryParse(ConfigurationManager.AppSettings[settingKey], out result)) {
                // And return it.
                return result;
            }
            // When no valid value is found in the settings then return the default value.
            return defaultValue;
        }


        /// <summary>
        /// Read a string setting from the settings and allow for a default value.
        /// </summary>
        protected static string ReadStringSetting(string settingKey, string defaultValue = "") {
            // Read the setting from the appsettings file.
            string result = ConfigurationManager.AppSettings[settingKey];
            return result != null ? result : defaultValue;
        }


        /// <summary>
        /// Read a datetime setting from the settings or else return the passed default time-value-as-string as a date.
        /// TODO BW 2013-02-28 Make a little more robust, with other string parsing options, and an overwrite without default.
        /// </summary>
        protected static DateTime ReadDateTimeSetting(string settingKey, string defaultValue) {
            string dateString = ReadStringSetting(settingKey, defaultValue);
            return DateTime.Parse(dateString, CultureInfo.CreateSpecificCulture("nl-NL"));
        }


        /// <summary>
        /// Get a list of ints from a comma seperated list.
        /// </summary>
        /// <param name="CsvStringOfInts"></param>
        /// <returns></returns>
        protected static List<int> GetIntsFromCSVString(string CsvStringOfInts) {
            List<int> ints = new List<int>(0);

            // Read the values, if any, from the comma seperated list and add them to int list if they can be parsed.
            if (!string.IsNullOrEmpty(CsvStringOfInts)) {
                List<string> listOfInts = CsvStringOfInts.Split(',').ToList();
                foreach (string cultureString in listOfInts) {
                    int result = 0;
                    int.TryParse(cultureString, out result);
                    if (result != 0) {
                        ints.Add(result);
                    }
                }
            }
            return ints;
        }

    }
}