using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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

        //Desactivo el borrado en cascada:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        //Cerrar la base de datos:
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);    
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

      
        public DbSet<GroupDetail> GroupDetails { get; set; }
    }
}