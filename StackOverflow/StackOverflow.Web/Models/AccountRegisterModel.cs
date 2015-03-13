using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Serialization;
using StackOverflow.Web.CostumeDataNotations;

namespace StackOverflow.Web.Models
{
    public class AccountRegisterModel
    {
        [Required(ErrorMessage = "*required") ]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "First name should be between 5 and 20 characters.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "*required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Last name should be between 5 and 20 characters.", MinimumLength = 2)]
        public string LastName { get; set; }
        [EmailExist(ErrorMessage = "Email already exist!")]
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Invalid Type")]
        [StringLength(50, ErrorMessage = "Email should be between 10 and 50 characters.", MinimumLength = 10)]
        public string Email { get; set; }
        [Required(ErrorMessage = "*required")]
        [StringLength(16, ErrorMessage = "Password should be between 8 and 16 characters.", MinimumLength = 8)]
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
        [TermsandConditions(ErrorMessage = "Accept Terms&Conditions")]
        public bool TermsAndConditions { get; set; }

    }
}