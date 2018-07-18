using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
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
        [Display(Name = "Course Description")]
        [StringLength(255)]
        public string CourseDescription { get; set; }


        //Navigational properties
        [Display(Name = "Attending Students")]        public virtual ICollection<ApplicationUser> AttendingStudents { get; set; }

        [Display(Name = "Course Modules")]        public virtual ICollection<Module> CourseModules { get; set; }

        [Display(Name = "Course Documents")]        public virtual ICollection<Document> CourseDocuments { get; set; }
    }
}