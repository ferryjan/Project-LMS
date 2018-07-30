using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Event
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }
}