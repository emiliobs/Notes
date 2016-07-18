using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public int GroupDetailId { get; set; }

        [Range(0, 1, ErrorMessage = "Invalid Note")]
        public float Percentaje { get; set; }

        [Range(0, 5, ErrorMessage = "Invalid Note")]
        public float Qualification { get; set; }

        //relación:
        [JsonIgnore]
        public virtual GroupDetail GroupDetail { get; set; }
    }
}