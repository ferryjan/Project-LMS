using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 2)]
        public string ActivityName { get; set; }

        [Display(Name = "Start")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Display(Name = "End")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Color { get; set; }

        //Navigational properties
        [Display(Name = "Module ID")]
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Display(Name = "Activity Type")]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [Display(Name = "Module Documents")]
        public virtual ICollection<Document> ActivityDocuments { get; set; }

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> res = new List<ValidationResult>();
        //    ApplicationDbContext db = new ApplicationDbContext();

        //    var result = db.Activities.FirstOrDefault(v => v.ActivityName == ActivityName && v.ModuleId == ModuleId);

        //    if (result != null && ActivityId == 0)
        //    {
        //        ValidationResult mss = new ValidationResult("There is already an activity with this name in this module");
        //        res.Add(mss);
        //    }
        //    //if (Start < DateTime.Now.Date && ActivityId == 0)
        //    //{
        //    //    ValidationResult mss = new ValidationResult("You cannot add an activity in the past!");
        //    //    res.Add(mss);
        //    //}
        //    //else if(Module != null && Start < Module.StartDate)
        //    //{
        //    //    ValidationResult mss = new ValidationResult("You cannot set an activity start ahead of the start of its module!");
        //    //    res.Add(mss);
        //    //}
        //    //else if (Start >= DateTime.Now.AddYears(5) && ActivityId == 0)
        //    //{
        //    //    ValidationResult mss = new ValidationResult("You cannot add an activity more than 5 years in the future!");
        //    //    res.Add(mss);
        //    //}
        //    if (End < Start)
        //    {
        //        ValidationResult mss = new ValidationResult("An activity must start before it ends");
        //        res.Add(mss);
        //    }
        //    //else if (Module != null && End > Module.EndDate)
        //    //{
        //    //    ValidationResult mss = new ValidationResult("You cannot set an activity endtime to later tan the end of its module");
        //    //    res.Add(mss);
        //    //}

        //    return res;
        //}


    }
}