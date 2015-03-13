using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using StackOverflow.Web.CostumeDataNotations;

namespace StackOverflow.Web.Models
{
    public class AccountNewPasswordModel
    {

        [Required(ErrorMessage = "*required")]
        [StringLength(16, ErrorMessage = "Password should be between 8 and 20 characters.", MinimumLength = 8)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Password must contain only letters and numbers")]
        [MustcontainNumberPassword(ErrorMessage = "must contain a number")]
        [MustcontaincapitalletterPassword(ErrorMessage = "must contain a capital letter")]
        [MustcontainvocalletterPassword(ErrorMessage = "must contain a vocal letter ")]
        [MustNotrepeathenextLetter(ErrorMessage = "Cannot contain repeated leters Ex : aa")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password are not the same")]
        public string ConfirmPassword { get; set; }
    }
}