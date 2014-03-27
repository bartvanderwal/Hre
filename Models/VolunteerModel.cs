using System;
using System.ComponentModel.DataAnnotations;

namespace HRE.Models {
    public class VolunteerModel {

        public string Name  { get; set; }
            
        public string Emailadres  { get; set; }
            
        public string Telefoonnummer  { get; set; }
            
        public string Straat  { get; set; }

        public string HouseNumber { get; set; }
            
        public string Postcode { get; set; }
        
        public string Woonplaats { get; set; }
        
        public DateTime GeboorteDatum { get; set; }

    }
}