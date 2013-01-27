using System.ComponentModel.DataAnnotations;

namespace HRE.Models {
    public class VolonteerModel {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Afzender { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Opmerkingen { get; set; }
    }
}