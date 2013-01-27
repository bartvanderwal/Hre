using System.ComponentModel.DataAnnotations;

namespace HRE.Models {
    public class ContactViewModel {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Afzender { get; set; }

        [Required]
        public string Onderwerp { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Bericht { get; set; }
    }
}