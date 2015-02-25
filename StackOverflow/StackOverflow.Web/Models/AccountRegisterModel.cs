using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Serialization;

namespace StackOverflow.Web.Models
{
    public class AccountRegisterModel
    {
        [Required(ErrorMessage = "*required") ]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "First name should be between 5 and 20 characters.", MinimumLength = 5)]
        public string Name { get; set; }
        [Required(ErrorMessage = "*required")]
        [EmailAddress(ErrorMessage = "Invalid Type")]
        [StringLength(50, ErrorMessage = "Email should be between 10 and 50 characters.", MinimumLength = 10)]
        public string Email { get; set; }
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

        public string Error { set; get; }

        [Required(ErrorMessage = "*required")]
        public bool TermsAndConditions { get; set; }

    }
}