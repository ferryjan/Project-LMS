using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class ActivityViewModels
    {
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

        //Navigational properties
        [Display(Name = "Module ID")]
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Display(Name = "Activity Type")]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        public string ModuleName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime ModuleStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime ModuleEndDate { get; set; }

        public string CourseName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CourseStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CourseEndDate { get; set; }
    }
}