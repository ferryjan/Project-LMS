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

           

            var activityTypes = new[] {
                new ActivityType { Type = "Lecture" },
                new ActivityType { Type = "Exercise" },
                new ActivityType { Type = "E-learning" },
                new ActivityType { Type = "Other" }
            };
            db.ActivityTypes.AddOrUpdate(s => new { s.Type }, activityTypes);

            var courses = new[] {
                new Course {    CourseName = "Swedish fika done right",
                                StartDate = DateTime.Now.Date,
                                EndDate = DateTime.Now.AddMonths(1).Date,
                                CourseDescription = "It takes a lot of practice to get the finer points of swedish fika right." +
                                                    " We will start with the basic 7 types of cookies and quickly move into the " +
                                                    "more advanced areas of the fine art of fika." },
                new Course {    CourseName = "Cowtilting",
                                StartDate = DateTime.Now.AddDays(2).Date,
                                EndDate = DateTime.Now.AddMonths(1).AddDays(2).Date,
                                CourseDescription = "While cowtilting is rather new to most of us here, the sport actually " +
                                                    "have a very long history. " +
                                                    "This cource is a prerequisite for the more advanced cource called \"Bulltilting\"."}
            };
            db.Courses.AddOrUpdate(n => n.CourseName, courses);
            db.SaveChanges();

            var modules = new[] {
                new Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate,
                    EndDate = courses[0].StartDate.AddDays(2),
                    Name = "Cookies, not just for web-pages",
                    Description = "An introduction to Cookies: the common combinations."
                },
                new Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(2),
                    EndDate = courses[0].StartDate.AddDays(4),
                    Name = "Coffie, the black gold",
                    Description = "An introduction to Coffie: Brew or boil?"
                }
            };
            db.Modules.AddOrUpdate(m => m.Name, modules);
            db.SaveChanges();

            string email, rolestring;
            ApplicationUser adminUser;

            email = "admin@admin.se";
            rolestring = "Teacher";
            if (!db.Users.Any(u => u.UserName == email))
            {
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
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
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

            

            var activityTypes = new[] {
                new ActivityType { Type = "Lecture" },
                new ActivityType { Type = "Exercise" },
                new ActivityType { Type = "E-learning" },
                new ActivityType { Type = "Homework" },
                new ActivityType { Type = "Other" }
            };
            db.ActivityTypes.AddOrUpdate(s => new { s.Type }, activityTypes);

            var courses = new[] {
                new Course {    CourseName = "Swedish fika done right",
                                StartDate = DateTime.Now.Date,
                                EndDate = DateTime.Now.AddMonths(1).Date,
                                CourseDescription = "It takes a lot of practice to get the finer points of swedish fika right." +
                                                    " We will start with the basic 7 types of cookies and quickly move into the " +
                                                    "more advanced areas of the fine art of fika." },
                new Course {    CourseName = "Cowtilting",
                                StartDate = DateTime.Now.AddDays(2).Date,
                                EndDate = DateTime.Now.AddMonths(1).AddDays(2).Date,
                                CourseDescription = "While cowtilting is rather new to most of us here, the sport actually " +
                                                    "have a very long history. " +
                                                    "This cource is a prerequisite for the more advanced cource called \"Bulltilting\"."}
            };
            db.Courses.AddOrUpdate(n => n.CourseName, courses);
            db.SaveChanges();

            var modules = new[] {
                new Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate,
                    EndDate = courses[0].StartDate.AddDays(2),
                    Name = "Cookies, not just for web-pages",
                    Description = "An introduction to Cookies: the common combinations."
                },
                new Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(2),
                    EndDate = courses[0].StartDate.AddDays(4),
                    Name = "Coffie, the black gold",
                    Description = "An introduction to Coffie: Brew or boil?"
                }
            };
            db.Modules.AddOrUpdate(m => m.Name, modules);
            db.SaveChanges();
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
                        CourseId = courses[0].CourseId,
                        TimeOfRegistration = DateTime.Now
                    },
                    "password");
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
            }
            adminUser = userManager.FindByName(email);
            userManager.AddToRole(adminUser.Id, rolestring);
        }
    }
}
