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
        [Display(Name = "Activity Name")]
        [StringLength(50, MinimumLength = 2)]
        public string CourseName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Activity Description")]
        [StringLength(255)]
        [DataType(DataType.MultilineText)]
        public string ActivityDescription { get; set; }

        [Display(Name = "Deadline")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        //Navigational properties
        [Display(Name = "Module ID")]
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Display(Name = "Activity Type")]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [Display(Name = "Module Documents")]        public virtual ICollection<Document> ActivityDocuments { get; set; }
    }
}