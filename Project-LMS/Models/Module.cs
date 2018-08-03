using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{ 
    public class Module : IValidatableObject
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        [Display(Name = "Module Name")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Description")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //Navigational properties
        [Display(Name = "Course ID")]
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Module Activities")]
        public virtual ICollection<Activity> Activities { get; set; }

        [Display(Name = "Module Documents")]
        public virtual ICollection<Document> ModuleDocuments { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            ApplicationDbContext db = new ApplicationDbContext();

            var result = db.Modules.FirstOrDefault(v => v.Name == Name);
            if (result != null && ModuleId == 0)
            {
                ValidationResult mss = new ValidationResult("There is already a module by this name registered in this course");
                res.Add(mss);
            }
            //if (StartDate < DateTime.Now.Date && ModuleId == 0)
            //{
            //    ValidationResult mss = new ValidationResult("You cannot add a module in the past!");
            //    res.Add(mss);
            //}
            //else if (StartDate >= DateTime.Now.AddYears(5) && ModuleId == 0)
            //{
            //    ValidationResult mss = new ValidationResult("You cannot add a module more than 5 years in the future!");
            //    res.Add(mss);
            //}
            if (EndDate < StartDate)
            {
                ValidationResult mss = new ValidationResult("End date must be greater than start date");
                res.Add(mss);
            }
            return res;
        }
    }

    public class MoveModuleViewModel
    {
        public Module Module { get; set; }

        [Display(Name = "New startdate")]
        [DataType(DataType.Date)]
        public DateTime NewDate { get; set; }
    }
}