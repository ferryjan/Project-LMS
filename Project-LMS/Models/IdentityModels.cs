using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project_LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        public string ProfileImageRef { get; set; }
        public DateTime TimeOfRegistration { get; set; }
        public string FirstTimeLogin { get; set; }

        //Navigational properties
        [Display(Name = "Course ID")]
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Users' Documents")]
        public virtual ICollection<Document> UserDocuments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            if (GivenName != null) userIdentity.AddClaim(new Claim("GivenName", this.GivenName.ToString()));
            if (FamilyName != null) userIdentity.AddClaim(new Claim("FamilyName", this.FamilyName.ToString()));
            if (ProfileImageRef != null) userIdentity.AddClaim(new Claim("ProfileImageRef", this.ProfileImageRef.ToString()));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Module> Modules { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}