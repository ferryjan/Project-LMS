using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class ScheduleViewModels
    {
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }
        [Display(Name = "Modul")]
        public string Modul { get; set; }
        [Display(Name = "AM (08:00 - 12:00)")]
        public String AM { get; set; }
        [Display(Name = "PM (13:00 - 17:00)")]
        public String PM { get; set; }
        [Display(Name = "Comments")]
        public String Extern { get; set; }

        public string Year { get; set; }
        public int DayOfYear { get; set; }
        public DateTime ActuallDate { get; set; }
        public string bgColor { get; set; }
        public List<string> ErrModul { get; set; }
        public List<string> ErrAM { get; set; }
        public List<string> ErrPM { get; set; }


    }

    public class ScheduleHeadViewModels
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public ScheduleViewModels ViewModels { get; set; }
        public List<ScheduleViewModels> myList;
    }
}