using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using HRE.Dal;

namespace HRE.Models {

    public class EarlyBirdViewModel {

        public LogonUserDal User { get; set; }
    }

}
