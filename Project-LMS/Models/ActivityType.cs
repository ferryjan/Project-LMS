using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeId { get; set; }

        public string Type { get; set; }

        //Navigational properties
        public virtual ICollection<Activity> Activities { get; set; }
    }
}