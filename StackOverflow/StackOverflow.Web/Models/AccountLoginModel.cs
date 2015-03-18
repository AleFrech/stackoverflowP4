using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StackOverflow.Web.CostumeDataNotations;

namespace StackOverflow.Web.Models
{
    public class AccountLoginModel
    {
        [EmailVerify(ErrorMessage = "Must Confirm Email")]
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}