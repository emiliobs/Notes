using Notes.Classes;
using Notes.Migrations;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Notes.Classes;
namespace Notes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //me permite crear cambios en la bd:
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NotesContext, Configuration>());

            this.CheckRoles();
            Utilities.CheckSuperUser("Admin");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRoles()
        {
            Utilities.CheckRole("Admin");
            Utilities.CheckRole("Student");
            Utilities.CheckRole("Teacher");
        }

    }
}