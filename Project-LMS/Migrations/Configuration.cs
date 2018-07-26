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

        //Struct for seeding users. using properties instead of fields due to VisualStudio recommendations
        private struct NewUser
        {
            public string Email { get; set; }
            public string Rolestring { get; set; }
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public int? CourseId { get; set; }
        }

        protected override void Seed(Project_LMS.Models.ApplicationDbContext db)
        {

            // Seeding Roles. Changes to roles will affect the application
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

            //Seeding ActivityTypes. Changes to the type "Homework" might affect the application. The order in wich these entities are seeded reflects the order shown in relevant dropdownboxes.
            var activityTypes = new[] {
                new ActivityType { Type = "Lecture" },
                new ActivityType { Type = "Exercise" },
                new ActivityType { Type = "E-learning" },
                new ActivityType { Type = "Homework" },
                new ActivityType { Type = "Other" }
            };
            db.ActivityTypes.AddOrUpdate(s => new { s.Type }, activityTypes);

            //Seeding courses
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

            //Seeding modules, referencing courses
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

            //Seeding Activities
            var activities = new[] {
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate,
                    End = modules[0].StartDate.AddDays(1),
                    ActivityName = "Fine dining",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "Eating cookies without leaving ccrumbles requires lots of training"
                },
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate.AddDays(1),
                    End = modules[0].StartDate.AddDays(2),
                    ActivityName = "Conversation",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "How to make fine conversation while eating cookies. Keeping your mouth shut until all crumbles have been swallowed and other essential skills."
                },
                //Lets seed a homework (activityTypes[3]). Lets also seed it so the start and end interferes with another activity.
                new Activity { 
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate,
                    End = modules[0].StartDate.AddDays(1),
                    ActivityName = "Choosing the right blend",
                    ActivityTypeId = activityTypes[3].ActivityTypeId,
                    Description = "Choose 7 cookies that mix well and can make the base for a good fika. To pass your written report must be uploaded in time."
                },
            };
            db.Activities.AddOrUpdate(a => a.ActivityName, activities);
            db.SaveChanges();


            //Seeding users
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser adminUser;

            //Students must be linked to courses, teachers must not
            var newUser = new[]
{
                new NewUser
                {
                    Email = "admin@admin.se",
                    Rolestring = "Teacher",
                    GivenName = "Boris",
                    FamilyName= "Jeltsin",
                    CourseId = null
                },
                new NewUser
                {
                    Email = "Donald@duck.se",
                    Rolestring = "Teacher",
                    GivenName = "Donald",
                    FamilyName= "Duck",
                    CourseId = null
                },
                new NewUser
                {
                    Email = "student@student.se",
                    Rolestring = "Student",
                    GivenName = "Kattis",
                    FamilyName= "Hoppsan",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Sten.Sture@svea.se",
                    Rolestring = "Student",
                    GivenName = "Sten Sture",
                    FamilyName= "Den Äldre",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Gorm@asa.dk",
                    Rolestring = "Student",
                    GivenName = "Gorm",
                    FamilyName= "Den Gamle",
                    CourseId = courses[0].CourseId
                }
            };

            foreach (var item in newUser)
            {
                if (!db.Users.Any(u => u.UserName == item.Email))
                {
                    var result = userManager.Create(
                        new ApplicationUser
                        {
                            GivenName = item.GivenName,
                            FamilyName = item.FamilyName,
                            ProfileImageRef = "",
                            UserName = item.Email,
                            Email = item.Email,
                            TimeOfRegistration = DateTime.Now,
                            FirstTimeLogin = false,
                            isActive = true
                        },
                        "Ante_007");
                    if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
                }
                adminUser = userManager.FindByName(item.Email);
                userManager.AddToRole(adminUser.Id, item.Rolestring);
            }
        }
    }
}
