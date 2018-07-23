using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class HomeworkViewModels
    {
        public List<ApplicationUser> Students { get; set; }
        public List<Document> Documents { get; set; }
    }
}