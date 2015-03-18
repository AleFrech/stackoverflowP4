using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Models
{
    public class CommentCreateModel
    {
        [Required(ErrorMessage = "*required")]
        [StringLength(250, ErrorMessage = "Body  should be between 5 and 250 characters.", MinimumLength = 5)]
        public string Description { get; set; }
    }
}