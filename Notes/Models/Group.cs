using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "The field {0} is requered.")]
        [StringLength(100, ErrorMessage = "The field {0} can contain maximum {1} an minimum {2}", MinimumLength =3)]
        [Index("GroupDesceiptionIndex", IsUnique =true)]
        public string Description { get; set; }

        [Display(Name = "Profesor")]
        [Required(ErrorMessage = "The field {0} is requered.")]
        public int UserId { get; set; }


        //Relacion:
        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupDetail> GroupDetails { get; set; }
    }
}