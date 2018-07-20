namespace Project_LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Project_LMS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project_LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project_LMS.Models.ApplicationDbContext db)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (db.Roles.Any(r => r.Name == roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }


            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            string email, rolestring;
            ApplicationUser adminUser;

            email = "admin@admin.se";
            rolestring = "Teacher";
            if (!db.Users.Any(u => u.UserName == email)) { 
                var result = userManager.Create(
                    new ApplicationUser
                    {
                        GivenName = "Boris",
                        FamilyName = "Jeltsin",
                        ProfileImageRef = "",
                        UserName = email,
                        Email = email,
                        TimeOfRegistration = DateTime.Now
                    }, 
                    "password");
                if (!result.Succeeded) {throw new Exception(string.Join("\n", result.Errors)); }
            }
            adminUser = userManager.FindByName(email);
            userManager.AddToRole(adminUser.Id, rolestring);


            email = "Donald@duck.se";
            rolestring = "Teacher";
            if (!db.Users.Any(u => u.UserName == email))
            {
                var result = userManager.Create(
                    new ApplicationUser
                    {
                        GivenName = "Donald",
                        FamilyName = "Duck",
                        ProfileImageRef = "",
                        UserName = email,
                        Email = email,
                        TimeOfRegistration = DateTime.Now,
                        FirstTimeLogin = false
                    },
                    "Ante_007");
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
            }
            adminUser = userManager.FindByName(email);
            userManager.AddToRole(adminUser.Id, rolestring);

            email = "student@student.se";
            rolestring = "Student";
            if (!db.Users.Any(u => u.UserName == email))
            {
                var result = userManager.Create(
                    new ApplicationUser
                    {
                        GivenName = "Kattis",
                        FamilyName = "Hoppsan",
                        ProfileImageRef = "",
                        UserName = email,
                        Email = email,
                        TimeOfRegistration = DateTime.Now
                    },
                    "password");
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
            }
            adminUser = userManager.FindByName(email);
            userManager.AddToRole(adminUser.Id, rolestring);

            var activityTypes = new[] {
                new ActivityType { Type = "Lecture" },
                new ActivityType { Type = "Exercise" },
                new ActivityType { Type = "E-learning" },
                new ActivityType { Type = "Other" }
            };
            db.ActivityTypes.AddOrUpdate(s => new { s.Type }, activityTypes);
        }
    }
}
