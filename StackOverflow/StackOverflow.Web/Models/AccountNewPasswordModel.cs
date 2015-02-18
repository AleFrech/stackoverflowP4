using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace StackOverflow.Web.Models
{
    public class AccountNewPasswordModel
    {
        [Required(ErrorMessage = "*required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password should be between 8 and 20 characters.", MinimumLength = 8)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Password must contain only letters and numbers")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password should be between 8 and 20 characters.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password are not the same")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Password must contain only letters and numbers")]
        public string ConfirmPassword { get; set; }
    }
}