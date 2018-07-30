using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_LMS.Models
{
    public class SendMessageViewModel
    {
        [Display(Name = "From")]
        public string SentFrom { get; set; }
        public string SentFromFullName { get; set; }

        [Display(Name = "To")]
        public string SentTo { get; set; }
        public string SentToFullName { get; set; }

        [Required]
        [Display(Name = "Topic")]
        [StringLength(100, MinimumLength = 2)]
        public string Topic { get; set; }

        [Required]
        [Display(Name = "Message")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Msg { get; set; }
    }
}