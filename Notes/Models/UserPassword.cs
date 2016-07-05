using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    //esta clase no va a la bd:solo en memoria:
    [NotMapped]
    public class UserPassword : User
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximum {1} an d minimum {2} characteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}