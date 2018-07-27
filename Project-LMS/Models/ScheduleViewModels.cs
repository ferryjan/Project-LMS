using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class ScheduleViewModels
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }
        [Display(Name = "Modul")]
        public string Modul { get; set; }
        [Display(Name = "PM")]
        public String PM { get; set; }
        [Display(Name = "AM")]
        public String AM { get; set; }
        [Display(Name = "Extern")]
        public String Extern { get; set; }
    }
}