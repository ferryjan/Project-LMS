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

            var roleNames = new[] { "Teacher", "Student", "System" };
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
                new Project_LMS.Models.Module { //0
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate,
                    EndDate = courses[0].StartDate.AddDays(8),
                    Name = "Cookies, not just for web-pages",
                    Description = "An introduction to Cookies: the common combinations."
                },
                new Project_LMS.Models.Module { //1
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(7),
                    EndDate = courses[0].StartDate.AddDays(11),
                    Name = "Coffie, the black gold",
                    Description = "An introduction to Coffie: Brew or boil?"
                },
                new Project_LMS.Models.Module { //2
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(11),
                    EndDate = courses[0].StartDate.AddDays(15),
                    Name = "fika: rest, recreation or regulation",
                    Description = "A theoretical glance at the position of fika in todays society."
                },
                new Project_LMS.Models.Module { //3
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(15),
                    EndDate = courses[0].StartDate.AddDays(30),
                    Name = "Fika: the F-word in Sweden",
                    Description = "Fika is a coffee break in Sweden, but it is invested with considerable socio-cultural and symbolic significance."
                },
                new Project_LMS.Models.Module { //4
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate,
                    EndDate = courses[0].StartDate.AddDays(8),
                    Name = "Fika is polyvalent and paradoxical ",
                    Description = "It can be a signifier of employer recognition and generosity, and a challenge to the mind/body dualism in academic work cultures."
                },

                new Project_LMS.Models.Module { //5
                    CourseId = courses[0].CourseId,
                    StartDate = courses[0].StartDate.AddDays(8),
                    EndDate = courses[0].StartDate.AddDays(30),
                    Name = "Fika: research",
                    Description = "Our research interrogates and deconstructs fika in the context of the political economy of neoliberalism. "
                },
                    new Project_LMS.Models.Module { //6
                    CourseId = courses[1].CourseId,
                    StartDate = courses[1].StartDate.AddDays(0),
                    EndDate = courses[1].StartDate.AddDays(5),
                    Name = "Physical excercise",
                    Description = "After tipping a cow, it's important to be able to outrun it. "
                },
                    new Project_LMS.Models.Module { //7
                    CourseId = courses[1].CourseId,
                    StartDate = courses[1].StartDate.AddDays(6),
                    EndDate = courses[1].StartDate.AddDays(10),
                    Name = "Stealth",
                    Description = "The art fo sneaking up on a sleeping cow, without waking it"
                },
                    new Project_LMS.Models.Module { //8
                    CourseId = courses[1].CourseId,
                    StartDate = courses[1].StartDate.AddDays(6),
                    EndDate = courses[1].StartDate.AddDays(10),
                    Name = "Dry-run",
                    Description = "We practice on each other"
                },
                    new Project_LMS.Models.Module { //9
                    CourseId = courses[1].CourseId,
                    StartDate = courses[1].StartDate.AddDays(6),
                    EndDate = courses[1].StartDate.AddDays(10),
                    Name = "Starting small",
                    Description = "Calf-tilting"
                },
                    new Project_LMS.Models.Module { //10
                    CourseId = courses[1].CourseId,
                    StartDate = courses[1].StartDate.AddDays(6),
                    EndDate = courses[1].StartDate.AddDays(10),
                    Name = "The real deal",
                    Description = "We drive out on the countryside, in search of sleeping cows"
                }
            };
            db.Modules.AddOrUpdate(m => m.Name, modules);
            db.SaveChanges();

            //Seeding Activities
            var activities = new[] {
                //Lets seed a homework (activityTypes[3]). Lets also seed it so the start and end interferes with another activity.
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate,
                    End = modules[0].StartDate,
                    ActivityName = "The multiple interpretations of fika.",
                    ActivityTypeId = activityTypes[3].ActivityTypeId,
                    Description = "List, and explain the 5 common interpretations of fika",
                    Color = "navy"
                },
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate.AddDays(1),
                    End = modules[0].StartDate.AddDays(3),
                    ActivityName = "Fine dining",
                    ActivityTypeId = activityTypes[0].ActivityTypeId,
                    Description = "Eating cookies without leaving crumbles requires lots of training",
                    Color = "red"
                },
                new Activity {
                    ModuleId = modules[0].ModuleId,
                    Start = modules[0].StartDate.AddDays(4),
                    End = modules[0].StartDate.AddDays(8),
                    ActivityName = "Conversation",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "How to make fine conversation while eating cookies. Keeping your mouth shut until all crumbles have been swallowed and other essential skills.",
                    Color = "blue"
                },              
                new Activity {
                    ModuleId = modules[1].ModuleId,
                    Start = modules[0].StartDate.AddDays(7),
                    End = modules[0].StartDate.AddDays(10),
                    ActivityName = "Definition",
                    ActivityTypeId = activityTypes[2].ActivityTypeId,
                    Description = "Fika, as a noun, refers to the combination of coffee and usually some sort of sweet snack. But fika, as a verb, is the act of partaking in a Swedish social institution.",
                    Color = "green"
                },
                new Activity {
                    ModuleId = modules[2].ModuleId,
                    Start = modules[0].StartDate.AddDays(12),
                    End = modules[0].StartDate.AddDays(13),
                    ActivityName = "Choosing the right blend",
                    ActivityTypeId = activityTypes[3].ActivityTypeId,
                    Description = "Choose 7 cookies that mix well and can make the base for a good fika. To pass your written report must be uploaded in time.",
                    Color = "navy"
                },
                new Activity {
                    ModuleId = modules[2].ModuleId,
                    Start = modules[0].StartDate.AddDays(12),
                    End = modules[0].StartDate.AddDays(15),
                    ActivityName = "The classical utility of fika",
                    ActivityTypeId = activityTypes[5].ActivityTypeId,
                    Description = "Traditionally, fika has been used as a site for team-building, democratization, and well-being at work.",
                    Color = "gray"
                },
                new Activity {
                    ModuleId = modules[3].ModuleId,
                    Start = modules[0].StartDate.AddDays(16),
                    End = modules[0].StartDate.AddDays(19),
                    ActivityName = "The modern utility of fika",
                    ActivityTypeId = activityTypes[2].ActivityTypeId,
                    Description = "Can it be that in modern life the utility of fika have tranhsformed into a neoliberal surveillance and normalization technologies in which one's corporate loyalty and interpersonal skills are made visible for assessment?",
                    Color = "green"
                },
                new Activity {
                    ModuleId = modules[3].ModuleId,
                    Start = modules[0].StartDate.AddDays(20),
                    End = modules[0].StartDate.AddDays(24),
                    ActivityName = "Economy of fika in the workplace",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "Twice a day, in mornings and afternoons, Swedish workers can gather for a short break, in the internal fika-area. Employers are expected to bear most of the cost of breaks as an investment in their employees (Spross 2016 Spross, Linn. 2016. [A Dilemma of the Welfare State: Formulations State of the Working Hours Question 1919-2002]. Uppsala: Uppsala University.",
                    Color = "blue"
                },
                new Activity {
                    ModuleId = modules[3].ModuleId,
                    Start = modules[0].StartDate.AddDays(25),
                    End = modules[0].StartDate.AddDays(30),
                    ActivityName = "Exploring our feelings.",
                    ActivityTypeId = activityTypes[0].ActivityTypeId,
                    Description = "We noted an affective and gendered economy with fika eliciting feelings of pleasure in the social and recreational aspects, but shame and anger at what was perceived as coercion to perform a particular type of sociable subjectivity.",
                    Color = "red"
                },
                new Activity {
                    ModuleId = modules[4].ModuleId,
                    Start = modules[0].StartDate.AddDays(8),
                    End = modules[0].StartDate.AddDays(16),
                    ActivityName = "Picnic",
                    ActivityTypeId = activityTypes[2].ActivityTypeId,
                    Description = "Acronym for Problem in chair. Not in computer. Used by Sysadmins to covertly describe user error to each other. Some snickering usually involved.",
                    Color = "purple"
                },
                new Activity {
                    ModuleId = modules[6].ModuleId,
                    Start = modules[6].StartDate.AddDays(0),
                    End = modules[6].StartDate.AddDays(2),
                    ActivityName = "Speedwalking",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "In full gear, and backpack filled with sand we speedwalk to a place called \"Lapphelvetet\", and back again",
                    Color = "blue"
                },
                    new Activity {
                    ModuleId = modules[6].ModuleId,
                    Start = modules[6].StartDate.AddDays(3),
                    End = modules[6].StartDate.AddDays(3),
                    ActivityName = "R&D",
                    ActivityTypeId = activityTypes[4].ActivityTypeId,
                    Description = "A full day off",
                    Color = "white"
                },
                    new Activity {
                    ModuleId = modules[6].ModuleId,
                    Start = modules[6].StartDate.AddDays(4),
                    End = modules[6].StartDate.AddDays(5),
                    ActivityName = "obstacle Course",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "Getting around the old military obstacle base outside Falun as fast as possibel. Both during daytime and during night-time",
                    Color = "blue"
                },
                    new Activity {
                    ModuleId = modules[6].ModuleId,
                    Start = modules[6].StartDate.AddDays(0),
                    End = modules[6].StartDate.AddDays(5),
                    ActivityName = "Cowspotting",
                    ActivityTypeId = activityTypes[3].ActivityTypeId,
                    Description = "Hand in a list of 10 good places for cowtilting. Including driving directions, pictures and a description of the farmer who owns said cows.",
                    Color = "navy"
                },
                    new Activity {
                    ModuleId = modules[7].ModuleId,
                    Start = modules[7].StartDate.AddDays(0),
                    End = modules[7].StartDate.AddDays(2),
                    ActivityName = "Silent walk",
                    ActivityTypeId = activityTypes[1].ActivityTypeId,
                    Description = "We train by walking silently over japanese singing floors (student pays for flight tickets to japan themselves)",
                    Color = "blue"
                }
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
                new NewUser //9
                {
                    Email = "Edlin.Pettiglio@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Edlin",
                    FamilyName= "Pettiglio",
                    CourseId = courses[0].CourseId
                },
                new NewUser //10
                {
                    Email = "Madelle.Leger@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Madelle",
                    FamilyName= "Leger",
                    CourseId = courses[0].CourseId
                },
                new NewUser //11
                {
                    Email = "Cross.Szporluk@brzy.ru",
                    Rolestring = "Student",
                    GivenName = "Cross",
                    FamilyName= "Szporluk",
                    CourseId = courses[0].CourseId
                },
                new NewUser //12
                {
                    Email = "Cody.Viglionese@yandex.com",
                    Rolestring = "Student",
                    GivenName = "Cody",
                    FamilyName= "Viglionese",
                    CourseId = courses[0].CourseId
                },
                new NewUser //13
                {
                    Email = "Vincenz.Henck@outlook.com",
                    Rolestring = "Student",
                    GivenName = "Vincenz",
                    FamilyName= "Henck",
                    CourseId = courses[0].CourseId
                },
                new NewUser //14
                {
                    Email = "Eddy.Meserve@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Eddy",
                    FamilyName= "Meserve",
                    CourseId = courses[0].CourseId
                },
                new NewUser //15
                {
                    Email = "Gian.Albran@protonmail.com",
                    Rolestring = "Student",
                    GivenName = "Gian",
                    FamilyName= "Albran",
                    CourseId = courses[0].CourseId
                },
                new NewUser //16
                {
                    Email = "Guenna.Jarrett@aol.com",
                    Rolestring = "Student",
                    GivenName = "Guenna",
                    FamilyName= "Jarrett",
                    CourseId = courses[0].CourseId
                },
                new NewUser //17
                {
                    Email = "Arel.Carvalho@aim.com",
                    Rolestring = "Student",
                    GivenName = "Arel",
                    FamilyName= "Carvalho",
                    CourseId = courses[0].CourseId
                },
                new NewUser //18
                {
                    Email = "Audra.Gustafson@aol.com",
                    Rolestring = "Student",
                    GivenName = "Audra",
                    FamilyName= "Gustafson",
                    CourseId = courses[0].CourseId
                },
                new NewUser //19
                {
                    Email = "Avram.Silvers@icloud.com",
                    Rolestring = "Student",
                    GivenName = "Audra",
                    FamilyName= "Gustafson",
                    CourseId = courses[0].CourseId
                },
                new NewUser //20
                {
                    Email = "Modestia.Michaud@icloud.com",
                    Rolestring = "Student",
                    GivenName = "Modestia",
                    FamilyName= "Michaud",
                    CourseId = courses[0].CourseId
                },
                new NewUser //21
                {
                    Email = "Boris.Cronan@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Boris",
                    FamilyName= "Cronan",
                    CourseId = courses[0].CourseId
                },
                new NewUser //22
                {
                    Email = "Kyrstin.Blake@mail.com",
                    Rolestring = "Student",
                    GivenName = "Kyrstin",
                    FamilyName= "Blake",
                    CourseId = courses[0].CourseId
                },
                new NewUser //23
                {
                    Email = "Reinwald.Reggiani@gmx.com",
                    Rolestring = "Student",
                    GivenName = "Reinwald",
                    FamilyName= "Reggiani",
                    CourseId = courses[0].CourseId
                },
                new NewUser //24
                {
                    Email = "Katuscha.Mccabe@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Katuscha",
                    FamilyName= "Mccabe",
                    CourseId = courses[0].CourseId
                },
                new NewUser //25
                {
                    Email = "Gallard.Cripps@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Gallard",
                    FamilyName= "Cripps",
                    CourseId = courses[0].CourseId
                },
                new NewUser //26
                {
                    Email = "Anallise.Cedillo@aol.com",
                    Rolestring = "Student",
                    GivenName = "Anallise",
                    FamilyName= "Cedillo",
                    CourseId = courses[0].CourseId
                },
                new NewUser //27
                {
                    Email = "Gayelord.Roels@bredbandsbolaget.se",
                    Rolestring = "Student",
                    GivenName = "Gayelord",
                    FamilyName= "Roels",
                    CourseId = courses[0].CourseId
                },
                new NewUser //28
                {
                    Email = "Kelila.Von.hoffman@fastmail.se",
                    Rolestring = "Student",
                    GivenName = "Kelila",
                    FamilyName= "Von Hoffman",
                    CourseId = courses[0].CourseId
                },
                new NewUser //29
                {
                    Email = "Silvester.Heck@Hushmail.il",
                    Rolestring = "Student",
                    GivenName = "Kelila",
                    FamilyName= "Von Hoffman",
                    CourseId = courses[0].CourseId
                },
                new NewUser //30
                {
                    Email = "Anna.diana.Teitz@Lycos.com",
                    Rolestring = "Student",
                    GivenName = "Anna-Diana",
                    FamilyName= "Teitz",
                    CourseId = courses[0].CourseId
                },
                new NewUser //31
                {
                    Email = "Laird.Penkethman@mail.ru",
                    Rolestring = "Student",
                    GivenName = "Laird",
                    FamilyName= "Penkethman",
                    CourseId = courses[0].CourseId
                },
                new NewUser //32
                {
                    Email = "Lauralee.Salvemini@Mailfence.com",
                    Rolestring = "Student",
                    GivenName = "Lauralee",
                    FamilyName= "Salvemini",
                    CourseId = courses[0].CourseId
                },
                new NewUser //33
                {
                    Email = "Muhammad.Spaur@hotmail.com",
                    Rolestring = "Student",
                    GivenName = "Muhammad",
                    FamilyName= "Spaur",
                    CourseId = courses[0].CourseId
                },
                new NewUser //34
                {
                    Email = "Marrilee.Renzella@livemail.com",
                    Rolestring = "Student",
                    GivenName = "Marrilee",
                    FamilyName= "Renzella",
                    CourseId = courses[0].CourseId
                },
                new NewUser //35
                {
                    Email = "Lyn.Viglionese@livemail.com",
                    Rolestring = "Student",
                    GivenName = "Lyn",
                    FamilyName= "Viglionese",
                    CourseId = courses[0].CourseId
                },
                new NewUser //36
                {
                    Email = "Jacquette.Mcarthur@protonmail.com",
                    Rolestring = "Student",
                    GivenName = "Jacquette",
                    FamilyName= "Mcarthur",
                    CourseId = courses[1].CourseId
                },
                new NewUser //37
                {
                    Email = "Frasco.Jarrett@rackspace.com",
                    Rolestring = "Student",
                    GivenName = "Frasco",
                    FamilyName= "Jarrett",
                    CourseId = courses[0].CourseId
                },
                new NewUser //38
                {
                    Email = "Ante.Bante@topstudent.se",
                    Rolestring = "Student",
                    GivenName = "Ante",
                    FamilyName= "Bante",
                    CourseId = courses[0].CourseId
                },
                new NewUser //39
                {
                    Email = "System@lms.se",
                    Rolestring = "System",
                    GivenName = "LMS",
                    FamilyName= "System Notification",
                    CourseId = null
                },
                new NewUser //40
                {
                    Email = "adrian@tollyx.net",
                    Rolestring = "Student",
                    GivenName = "Adrian",
                    FamilyName= "Hedqvist",
                    CourseId = courses[1].CourseId
                },
                new NewUser //41 
                {
                    Email = "jamesmamac1986@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Ali",
                    FamilyName= "Salhab",
                    CourseId = courses[1].CourseId
                },
                new NewUser //42
                {
                    Email = "ante@inersjo.com",
                    Rolestring = "Student",
                    GivenName = "Ante",
                    FamilyName= "Inersjö",
                    CourseId = courses[1].CourseId
                },
                new NewUser //43
                {
                    Email = "bereisawa@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Bereket",
                    FamilyName= "Alemseged",
                    CourseId = courses[1].CourseId
                },
                new NewUser //44
                {
                    Email = "dennis.nilsson1111@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Dennis",
                    FamilyName= "Nilsson",
                    CourseId = courses[1].CourseId
                },
                new NewUser //45
                {
                    Email = "fredrik.ff@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Fredrik",
                    FamilyName= "Ekman",
                    CourseId = courses[1].CourseId
                },
                new NewUser //46
                {
                    Email = "hesamzandigh@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Hesam",
                    FamilyName= "Ghaleh",
                    CourseId = courses[1].CourseId
                },
                new NewUser //47
                {
                    Email = "jesper.furtenbach@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Jesper",
                    FamilyName= "Fürtenbach",
                    CourseId = courses[1].CourseId
                },
                new NewUser //48
                {
                    Email = "jerry_swe@hotmail.com",
                    Rolestring = "Student",
                    GivenName = "Jerry",
                    FamilyName= "Fan",
                    CourseId = courses[1].CourseId
                },
                new NewUser //49
                {
                    Email = "kristoffer.aberg@agileacademy.se",
                    Rolestring = "Student",
                    GivenName = "Kristoffer",
                    FamilyName= "Åberg",
                    CourseId = courses[1].CourseId
                },
                new NewUser //50
                {
                    Email = "andersson257@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Marcus",
                    FamilyName= "Andersson",
                    CourseId = courses[1].CourseId
                },
                new NewUser //51
                {
                    Email = "justanotherperson1@outlook.com",
                    Rolestring = "Student",
                    GivenName = "Michael",
                    FamilyName= "Hjertö",
                    CourseId = courses[1].CourseId
                },
                new NewUser //52
                {
                    Email = "mohamed.almohsen@outlook.com",
                    Rolestring = "Student",
                    GivenName = "Mohamed",
                    FamilyName= "Almohsen",
                    CourseId = courses[1].CourseId
                },
                new NewUser //53
                {
                    Email = "oscar.ljungdahl@hotmail.com",
                    Rolestring = "Student",
                    GivenName = "Oskar",
                    FamilyName= "Ljungdahl",
                    CourseId = courses[1].CourseId
                },
                new NewUser //54
                {
                    Email = "lmpatrick.frank@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Patrick",
                    FamilyName= "Andersson",
                    CourseId = courses[1].CourseId
                },
                new NewUser //55
                {
                    Email = "reza_zaman@yahoo.com",
                    Rolestring = "Student",
                    GivenName = "Reza",
                    FamilyName= "Dagleh",
                    CourseId = courses[1].CourseId
                },
                new NewUser //56
                {
                    Email = "robert.dubrovskis@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Robert",
                    FamilyName= "Dubrovskis",
                    CourseId = courses[1].CourseId
                },
                new NewUser //57
                {
                    Email = "robert@huseli.us",
                    Rolestring = "Student",
                    GivenName = "Robert",
                    FamilyName= "Huselius",
                    CourseId = courses[1].CourseId
                },
                new NewUser //58
                {
                    Email = "rolf.bjarenstam@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Rolf",
                    FamilyName= "Bjärenstam",
                    CourseId = courses[1].CourseId
                },
                new NewUser //59
                {
                    Email = "san337737@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Sandeep",
                    FamilyName= "Singh",
                    CourseId = courses[1].CourseId
                },
                new NewUser //60
                {
                    Email = "normansebastian2@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Sebastian",
                    FamilyName= "Norman",
                    CourseId = courses[1].CourseId
                },
                new NewUser //61
                {
                    Email = "teemu158@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Teemu",
                    FamilyName= "Mustaniemi",
                    CourseId = courses[1].CourseId
                },
                new NewUser //62
                {
                    Email = "tomas.risveden@gmail.com",
                    Rolestring = "Student",
                    GivenName = "Tomas",
                    FamilyName= "Risveden",
                    CourseId = courses[1].CourseId
                },
                new NewUser //63
                {
                    Email = "wameedh75@hotmail.com",
                    Rolestring = "Student",
                    GivenName = "Wameedh",
                    FamilyName= "Hashosh",
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

            em = newUser[2].Email;
            appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "72806-tombraidericon.png";
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            em = newUser[7].Email;
            appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "Blonde.png";
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            em = newUser[8].Email;
            appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "80298-Dimitri.png";
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            em = newUser[10].Email;
            appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "GenericGirl.png";
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            em = newUser[17].Email;
            appUser = db.Users.FirstOrDefault(a => a.Email == em);
            if (appUser != null)
            {
                appUser.ProfileImageRef = "blondGirl.png";
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

            documentName = "BestCookies.docx";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[38].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/BestCookies.docx")),
                    ApplicationUserId = appUserId,
                    ActivityId = activities[0].ActivityId,
                    isHomework = true,
                    DocumentFileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Each cookie cost 1M USD!",
                    FeedBack = "Well done!"
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Kursschema .NET ND18.pdf";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[0].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/Kursschema .NET ND18.pdf")),
                    ApplicationUserId = appUserId,
                    CourseId = courses[0].CourseId,
                    isHomework = false,
                    DocumentFileType = "application/pdf",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Course schedule for some other course, it could have been for you !!"
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Ansvarsfriskrivning.pdf";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[0].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/Kursschema .NET ND18.pdf")),
                    ApplicationUserId = appUserId,
                    CourseId = courses[0].CourseId,
                    isHomework = false,
                    DocumentFileType = "application/pdf",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Lexicon does not take responsibility for any negative health effects occuring during, or after, this course."
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Baking cookies from scratch.docx";
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
                    DocumentFileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "Detailed description of the basics of cookie-baking."
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            documentName = "Maintaining a conversation.pdf";
            if (!db.Documents.Any(u => u.DocumentName == documentName))
            {
                email = newUser[1].Email;
                appUserId = db.Users.FirstOrDefault(u => u.Email == email).Id;
                doc = new Document
                {
                    FileData = File.ReadAllBytes(MapPath("~/Resources/conversation_skills.pdf")),
                    ApplicationUserId = appUserId,
                    ActivityId = activities[2].ActivityId,
                    isHomework = false,
                    DocumentFileType = "application/pdf",
                    UploadingTime = DateTime.Now,
                    DocumentName = documentName,
                    Description = "How to maintain a conversation."
                };
                db.Documents.Add(doc);
                db.SaveChanges();
            }

            var messages = new[] {
                new Message {
                    SentFrom = newUser[2].Email,
                    SentFromFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
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
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
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
                    SentFromFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
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
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
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
                    SentFromFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
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
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
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
                    SentFromFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
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
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
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
                    SentFromFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
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
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[2].Email,
                    SentToFullName = newUser[2].GivenName + " " + newUser[2].FamilyName,
                    SentDate = DateTime.Now.AddDays(-1),
                    isRead = false,
                    Topic = "Buy me some icecream!",
                    Msg = "No!!!!!",
                    MessageBoxNumber = "abc123",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[8].Email,
                    SentFromFullName = newUser[8].GivenName + " " + newUser[8].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-6),
                    isRead = false,
                    Topic = "Help me to do the homework!",
                    Msg = "Please help me to do the homework!",
                    MessageBoxNumber = "abc456",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[8].Email,
                    SentToFullName = newUser[8].GivenName + " " + newUser[8].FamilyName,
                    SentDate = DateTime.Now.AddDays(-5),
                    isRead = false,
                    Topic = "Help me to do the homework!",
                    Msg = "You should do it by yourself!",
                    MessageBoxNumber = "abc456",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                 new Message {
                    SentFrom = newUser[8].Email,
                    SentFromFullName = newUser[8].GivenName + " " + newUser[8].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-4),
                    isRead = false,
                    Topic = "Help me to do the homework!",
                    Msg = "I know, but I can't!",
                    MessageBoxNumber = "abc456",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[38].Email,
                    SentFromFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentTo = newUser[8].Email,
                    SentToFullName = newUser[8].GivenName + " " + newUser[8].FamilyName,
                    SentDate = DateTime.Now.AddDays(-3),
                    isRead = false,
                    Topic = "Help me to do the homework!",
                    Msg = "It's not my problem! :D  Kidding, I will help you!",
                    MessageBoxNumber = "abc456",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                },
                new Message {
                    SentFrom = newUser[8].Email,
                    SentFromFullName = newUser[8].GivenName + " " + newUser[8].FamilyName,
                    SentTo = newUser[38].Email,
                    SentToFullName = newUser[38].GivenName + " " + newUser[38].FamilyName,
                    SentDate = DateTime.Now.AddDays(-4),
                    isRead = false,
                    Topic = "Help me to do the homework!",
                    Msg = "Thanks!",
                    MessageBoxNumber = "abc456",
                    FirstPersonLeft = null,
                    SecondPersonLeft = null,
                    isPublic = false
                }
            };
            if (db.Messages.FirstOrDefault() == null)
            {
                db.Messages.AddOrUpdate(a => a.Id, messages);
                db.SaveChanges();
            }           
        }
    }
}
