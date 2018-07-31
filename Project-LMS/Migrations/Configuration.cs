namespace Project_LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Project_LMS.Models;
    using System;
    using System.IO;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Reflection;

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

        //Function for using relative paths in seed
        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null) return HostingEnvironment.MapPath(seedFile);
            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));
            return path;
        }

        protected override void Seed(Project_LMS.Models.ApplicationDbContext db)
        {
            //Enables debugging of seed, comment out to disable
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

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
                new ActivityType { Type = "Holiday" },
                new ActivityType { Type = "Other" }
            };
            db.ActivityTypes.AddOrUpdate(s => new { s.Type }, activityTypes);
            db.SaveChanges();

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
                new Project_LMS.Models.Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate,
                    EndDate = courses[0].StartDate.AddDays(2),
                    Name = "Cookies, not just for web-pages",
                    Description = "An introduction to Cookies: the common combinations."
                },
                new Project_LMS.Models.Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(2),
                    EndDate = courses[0].StartDate.AddDays(4),
                    Name = "Coffie, the black gold",
                    Description = "An introduction to Coffie: Brew or boil?"
                },
                new Project_LMS.Models.Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(4),
                    EndDate = courses[0].StartDate.AddDays(6),
                    Name = "fika: rest, recreation or regulation",
                    Description = "A theoretical glance at the position of fika in todays society."
                },
                new Project_LMS.Models.Module {
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(6),
                    EndDate = courses[0].StartDate.AddDays(8),
                    Name = "Fika: the F-word in Sweden",
                    Description = "Fika is a coffee break in Sweden, but it is invested with considerable socio-cultural and symbolic significance."
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
                    Description = "Eating cookies without leaving crumbles requires lots of training",
                    Color = "red"
                },
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate.AddDays(1),
                    End = modules[0].StartDate.AddDays(2),
                    ActivityName = "Conversation",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "How to make fine conversation while eating cookies. Keeping your mouth shut until all crumbles have been swallowed and other essential skills.",
                    Color = "red"
                },
                new Activity {
                    ModuleId = modules[2].ModuleId,
                    Start = modules[2].StartDate,
                    End = modules[2].StartDate.AddDays(1),
                    ActivityName = "Definition",
                    ActivityTypeId = activityTypes[0].ActivityTypeId,
                    Description = "Fika, as a noun, refers to the combination of coffee and usually some sort of sweet snack. But fika, as a verb, is the act of partaking in a Swedish social institution.",
                    Color = "red"
                },
                //Lets seed a homework (activityTypes[3]). Lets also seed it so the start and end interferes with another activity.
                new Activity { 
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate,
                    End = modules[0].StartDate.AddDays(1),
                    ActivityName = "Choosing the right blend",
                    ActivityTypeId = activityTypes[3].ActivityTypeId,
                    Description = "Choose 7 cookies that mix well and can make the base for a good fika. Don't forget to upload your written report on time.",
                    Color = "green"
                }
            };
            db.Activities.AddOrUpdate(a => a.ActivityId, activities);
            db.SaveChanges();


            //Seeding users
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser adminUser;

            //Students must be linked to courses, teachers must not
            var newUser = new[]
            {
                new NewUser //0
                {
                    Email = "admin@admin.se",
                    Rolestring = "Teacher",
                    GivenName = "Boris",
                    FamilyName= "Jeltsin",
                    CourseId = null
                },
                new NewUser //1
                {
                    Email = "Donald@duck.se",
                    Rolestring = "Teacher",
                    GivenName = "Donald",
                    FamilyName= "Duck",
                    CourseId = null,
                },
                new NewUser //2
                {
                    Email = "student@student.se",
                    Rolestring = "Student",
                    GivenName = "Kattis",
                    FamilyName= "Hoppsan",
                    CourseId = courses[0].CourseId
                },
                new NewUser //3
                {
                    Email = "Sten.Sture@svea.se",
                    Rolestring = "Student",
                    GivenName = "Sten Sture",
                    FamilyName= "Den Äldre",
                    CourseId = courses[0].CourseId
                },
                new NewUser //4
                {
                    Email = "Gorm@asa.dk",
                    Rolestring = "Student",
                    GivenName = "Gorm",
                    FamilyName= "Den Gamle",
                    CourseId = courses[0].CourseId
                },
                new NewUser //5
                {
                    Email = "MrCool@mail.com",
                    Rolestring = "Student",
                    GivenName = "Anders",
                    FamilyName= "Göransson",
                    CourseId = courses[0].CourseId
                },
                new NewUser //6
                {
                    Email = "bakihozab2984@yopmail.com",
                    Rolestring = "Student",
                    GivenName = "Boring",
                    FamilyName= "Larsson",
                    CourseId = courses[0].CourseId
                },
                new NewUser //7
                {
                    Email = "Exate1968@gustr.com",
                    Rolestring = "Student",
                    GivenName = "Regata",
                    FamilyName= "Gustavsson",
                    CourseId = courses[0].CourseId
                },
                new NewUser //8
                { 
                    Email = "Gamer@Lexicon.com",
                    Rolestring = "Student",
                    GivenName = "Dimitri",
                    FamilyName= "The Shadow",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Edlin.Pettiglio@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Edlin",
                    FamilyName= "Pettiglio",
                    CourseId = courses[0].CourseId
                },
                //10
                new NewUser
                {
                    Email = "Madelle.Leger@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Madelle",
                    FamilyName= "Leger",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Cross.Szporluk@brzy.ru",
                    Rolestring = "Student",
                    GivenName = "Cross",
                    FamilyName= "Szporluk",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Cody.Viglionese@yandex.com",
                    Rolestring = "Student",
                    GivenName = "Cody",
                    FamilyName= "Viglionese",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Vincenz.Henck@outlook.com",
                    Rolestring = "Student",
                    GivenName = "Vincenz",
                    FamilyName= "Henck",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Eddy.Meserve@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Eddy",
                    FamilyName= "Meserve",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Gian.Albran@protonmail.com",
                    Rolestring = "Student",
                    GivenName = "Gian",
                    FamilyName= "Albran",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Guenna.Jarrett@aol.com",
                    Rolestring = "Student",
                    GivenName = "Guenna",
                    FamilyName= "Jarrett",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Arel.Carvalho@aim.com",
                    Rolestring = "Student",
                    GivenName = "Arel",
                    FamilyName= "Carvalho",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Audra.Gustafson@aol.com",
                    Rolestring = "Student",
                    GivenName = "Audra",
                    FamilyName= "Gustafson",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Avram.Silvers@icloud.com",
                    Rolestring = "Student",
                    GivenName = "Audra",
                    FamilyName= "Gustafson",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Modestia.Michaud@icloud.com",
                    Rolestring = "Student",
                    GivenName = "Modestia",
                    FamilyName= "Michaud",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Boris.Cronan@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Boris",
                    FamilyName= "Cronan",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Kyrstin.Blake@mail.com",
                    Rolestring = "Student",
                    GivenName = "Kyrstin",
                    FamilyName= "Blake",
                    CourseId = courses[0].CourseId
                },
                new NewUser 
                {
                    Email = "Reinwald.Reggiani@gmx.com",
                    Rolestring = "Student",
                    GivenName = "Reinwald",
                    FamilyName= "Reggiani",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Katuscha.Mccabe@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Katuscha",
                    FamilyName= "Mccabe",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Gallard.Cripps@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Gallard",
                    FamilyName= "Cripps",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Anallise.Cedillo@aol.com",
                    Rolestring = "Student",
                    GivenName = "Anallise",
                    FamilyName= "Cedillo",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Gayelord.Roels@bredbandsbolaget.se",
                    Rolestring = "Student",
                    GivenName = "Gayelord",
                    FamilyName= "Roels",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Kelila.Von.hoffman@fastmail.se",
                    Rolestring = "Student",
                    GivenName = "Kelila",
                    FamilyName= "Von Hoffman",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Silvester.Heck@Hushmail.il",
                    Rolestring = "Student",
                    GivenName = "Kelila",
                    FamilyName= "Von Hoffman",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Anna.diana.Teitz@Lycos.com",
                    Rolestring = "Student",
                    GivenName = "Anna-Diana",
                    FamilyName= "Teitz",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Laird.Penkethman@mail.ru",
                    Rolestring = "Student",
                    GivenName = "Laird",
                    FamilyName= "Penkethman",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Lauralee.Salvemini@Mailfence.com",
                    Rolestring = "Student",
                    GivenName = "Lauralee",
                    FamilyName= "Salvemini",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Muhammad.Spaur@hotmail.com",
                    Rolestring = "Student",
                    GivenName = "Muhammad",
                    FamilyName= "Spaur",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Marrilee.Renzella@livemail.com",
                    Rolestring = "Student",
                    GivenName = "Marrilee",
                    FamilyName= "Renzella",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Lyn.Viglionese@livemail.com",
                    Rolestring = "Student",
                    GivenName = "Lyn",
                    FamilyName= "Viglionese",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Jacquette.Mcarthur@protonmail.com",
                    Rolestring = "Student",
                    GivenName = "Jacquette",
                    FamilyName= "Mcarthur",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Frasco.Jarrett@rackspace.com",
                    Rolestring = "Student",
                    GivenName = "Frasco",
                    FamilyName= "Jarrett",
                    CourseId = courses[0].CourseId
                },
                new NewUser
                {
                    Email = "Ante.Bante@topstudent.se",
                    Rolestring = "Student",
                    GivenName = "Ante",
                    FamilyName= "Bante",
                    CourseId = courses[1].CourseId
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
                            ProfileImageRef = "defaultImage.png", 
                            UserName = item.Email,
                            Email = item.Email,
                            TimeOfRegistration = DateTime.Now,
                            FirstTimeLogin = false,
                            isActive = true,
                            CourseId = item.CourseId
                        },
                        "Ante_007");
                    if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }
                }
                adminUser = userManager.FindByName(item.Email);
                userManager.AddToRole(adminUser.Id, item.Rolestring);
            }
            db.SaveChanges();

            //seed profile image
            var em = newUser[1].Email;
            var appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "Duck.png";
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            

            //Seeding documents
            string email;
            string appUserId;
            string documentName;
            Document doc;

            documentName = "MySevenCookies.txt";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[8].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/MySevenCookies.txt")),
                    ApplicationUserId = appUserId,
                    ActivityId = activities[3].ActivityId,
                    isHomework = true,
                    DocumentFileType = "text/plain",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "I did the best I could, please dont kick me from class"
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Disclaimer.pdf";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[0].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/Ansvarsfriskrivning.pdf")),
                    ApplicationUserId = appUserId,
                    CourseId = courses[0].CourseId,
                    isHomework = false,
                    DocumentFileType = "pdf",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Lexicon does not take responsibility for any negative health effects occuring during, or after, this course."
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Baking cookies from scratch";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[0].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/Baking cookies from scratch.docx")),
                    ApplicationUserId = appUserId,
                    ModuleId = modules[0].ModuleId,
                    isHomework = false,
                    DocumentFileType = "docx",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Detailed description of the basics of cookie-baking."
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            var messages = new[] {
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-10),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "Please buy me some icecream!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-9),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-8),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "Yes!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-7),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-6),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "Yes!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-5),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-4),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "Yes!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-3),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-2),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "Yes!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-1),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
            };
            if (db.Messages.Count() == 0)
            {
                db.Messages.AddOrUpdate(a => a.Id, messages);
                db.SaveChanges();
            }           
        }
    }
}
