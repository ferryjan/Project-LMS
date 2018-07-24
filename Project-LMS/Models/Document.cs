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

        [Display(Name = "Document Name")]
        [StringLength(50, MinimumLength = 2)]
        public string DocumentName { get; set; }

        [Display(Name = "Uploading Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UploadingTime { get; set; }

        [Display(Name = "Document URL")]
        public string DocumentRef { get; set; }

        [Display(Name = "Document File Type")]
        public string DocumentFileType { get; set; }

        public byte[] FileData { get; set; }

        public string FeedBack { get; set; }

        public bool isHomework { get; set; }

        public string Description { get; set; }

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

        [Display(Name = "Application User ID")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}