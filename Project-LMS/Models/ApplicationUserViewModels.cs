﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.ViewModels
{
    public class ChangeProfileViewModels
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "Picture")]
        public string ProfileImageRef { get; set; }

        [StringLength(100)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}