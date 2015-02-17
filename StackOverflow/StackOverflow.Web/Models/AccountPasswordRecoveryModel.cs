using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class AccountPasswordRecoveryModel{
    
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
       
    }
}