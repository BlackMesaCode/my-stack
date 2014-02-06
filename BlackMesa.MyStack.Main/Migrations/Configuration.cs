using System.Data.Entity.Migrations;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BlackMesa.MyStack.Main.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MyStackDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyStackDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("RegisteredUser"))
                roleManager.Create(new IdentityRole("RegisteredUser"));

            var userManager = new UserManager<User>(new UserStore<User>(context));

            var adminInDb = userManager.FindByName("Admin");

            if (adminInDb == null)
            {
                var admin = new User
                {
                    UserName = "Admin",
                };
                userManager.Create(admin, "test123");
                userManager.AddToRole(admin.Id, "Admin");
            }  


        }
    }
}
