using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public class NotesContext : DbContext
    {
        //constructor: a la db
        public NotesContext():base("DefaultConnection")
        {

        }



        //Cerrar la base de datos:
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);    
        }

        public DbSet<User> Users { get; set; }

        public System.Data.Entity.DbSet<Notes.Models.Group> Groups { get; set; }
    }
}