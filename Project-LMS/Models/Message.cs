using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Display(Name = "From")]
        public string SentFrom { get; set; }

        [Display(Name = "To")]
        public string SentTo { get; set; }

        [Display(Name = "Box Number")]
        public string MessageBoxNumber { get; set; }
        public bool isRead { get; set; }
        public bool isPublic { get; set; }
        public string FirstPersonLeft { get; set; }
        public string SecondPersonLeft { get; set; }
        public string SentFromFullName { get; set; }
        public string SentToFullName { get; set; }

        [Display(Name = "SentDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime SentDate { get; set; }

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