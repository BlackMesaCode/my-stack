using System.Data.Entity;
using BlackMesa.MyStack.Main.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.MyStack.Main.DataLayer
{
    public class MyStackDbContext : IdentityDbContext<User>
    {
        public MyStackDbContext() : base("DefaultConnection")
        {
            
        }

        // Identity Entities

        // DbSets for Identity come with the inherited IdentityDbContext<User>


        // MyStack Entities

        public DbSet<Folder> MyStack_Folders { get; set; }
        public DbSet<Card> MyStack_Cards { get; set; }
        public DbSet<TestItem> MyStack_TestItems { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Identity

            modelBuilder.Entity<IdentityUserRole>().ToTable("Identity_UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Identity_UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Identity_UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Identity_Roles");

            modelBuilder.Entity<IdentityUser>().ToTable("Identity_Users");
            modelBuilder.Entity<User>().ToTable("Identity_Users");



        }

    }
}