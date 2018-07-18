using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Document Name")]
        [StringLength(50, MinimumLength = 2)]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Document Description")]
        [StringLength(255)]
        public string CourseDescription { get; set; }

        [Display(Name = "Uploading Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UploadingTime { get; set; }

        [Display(Name = "Document URL")]
        public string DocumentRef { get; set; }

        //Navigational properties
        [Display(Name = "Course ID")]
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Module ID")]
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Display(Name = "Module ID")]
        public int? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        [Display(Name = "Application User ID")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}