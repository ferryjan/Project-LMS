using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        [Display(Name = "Module Name")]
        [StringLength(50, MinimumLength = 2)]
        public string CourseName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Module Description")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string ModuleDescription { get; set; }

        //Navigational properties
        [Display(Name = "Course ID")]
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Module Activities")]        public virtual ICollection<Activity> Activities { get; set; }

        [Display(Name = "Module Documents")]        public virtual ICollection<Document> ModuleDocuments { get; set; }
    }
}