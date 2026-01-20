namespace AppPeade.Migrations
{
    using AppPeade.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppPeade.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AppPeade.Models.ApplicationDbContext";
        }

        protected override void Seed(AppPeade.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            //1.Loo admin roll
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }
            //2. Loo admin kasutaja
            var adminEmail = "oleksandraryshniak@gmail.com";
            var adminUser = context.Users.FirstOrDefault(u => u.Email == adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                { UserName = adminEmail, Email = adminEmail, };
                userManager.Create(user, "Parol123!");
                adminUser = user;
            }
            //3. Lisa admin kasutaja admin roli
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
        }
    }
}
