using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class NaviItem
    {
        public List<Course> Courses { get; set; }
        public List<Module> Modules { get; set; }
        public List<Document> Documents { get; set; }
    }
}