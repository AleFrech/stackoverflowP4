using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class AccountLoginModel
    {
        //[EmailDosentExist(ErrorMessage = "Email Dosent exsist")]
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}