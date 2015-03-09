using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class EditProfileModel
    {
        [Required(ErrorMessage = "*required") ]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "First name should be between 5 and 20 characters.", MinimumLength = 5)]
        public string Name { get; set; }
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Invalid Type")]
        [StringLength(50, ErrorMessage = "Email should be between 10 and 50 characters.", MinimumLength = 10)]
       public string Email { set; get; }

        public string ImageUrl { get; set; }
    }
}