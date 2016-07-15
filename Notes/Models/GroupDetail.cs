using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public class GroupDetail
    {
        [Key]
        public int GroupDetailId { get; set; }

        [Required]
        public int GroupId { get; set; }
        
        [Required]
        public int UserId { get; set; }

        //Relación:
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}