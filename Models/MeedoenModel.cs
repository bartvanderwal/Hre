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
    }





}
