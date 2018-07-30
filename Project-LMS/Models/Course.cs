using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Course : IValidatableObject
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(50, MinimumLength = 2)]
        public string CourseName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Course Description")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string CourseDescription { get; set; }


        //Navigational properties
        [Display(Name = "Attending Students")]
        public virtual ICollection<ApplicationUser> AttendingStudents { get; set; }

        [Display(Name = "Course Modules")]
        public virtual ICollection<Module> CourseModules { get; set; }

        [Display(Name = "Course Documents")]
        public virtual ICollection<Document> CourseDocuments { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            ApplicationDbContext db = new ApplicationDbContext();

            var result = db.Courses.FirstOrDefault(v => v.CourseName == CourseName);
            if (result != null && CourseId == 0)
            {
                ValidationResult mss = new ValidationResult("There is already a module by this name registered in this course");
                res.Add(mss);
            }
            if (StartDate < DateTime.Now.Date && CourseId == 0)
            {
                ValidationResult mss = new ValidationResult("You cannot add a course in the past!");
                res.Add(mss);
            }
            else if (StartDate >= DateTime.Now.AddYears(5) && CourseId == 0)
            {
                ValidationResult mss = new ValidationResult("You cannot add a course more than 5 years in the future!");
                res.Add(mss);
            }
            if (EndDate <= StartDate)
            {
                ValidationResult mss = new ValidationResult("End date must be greater than start date");
                res.Add(mss);
            }
            return res;
        }
    }
}