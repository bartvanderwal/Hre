using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace HRE.Models {

    public class MeedoenModel {

        [Required(ErrorMessage="Vul een geldig e-mail adres in!")]
        public string Email { get; set; }


        /// <summary>
        /// Geeft aan of de algemene inschrijving al geopend is, afhankelijk van de startdatum in de appsettings.
        /// </summary>
        /// <returns></returns>
        public static bool AlgemeneInschrijvingGeopend() {
            return DateTime.Compare(DateTime.Now, SportsEventRepository.OpeningsdatumAlgemeneInschrijving)>0;
        }
    }





}
