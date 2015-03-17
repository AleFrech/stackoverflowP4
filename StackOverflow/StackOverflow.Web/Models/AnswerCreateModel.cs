using System;
using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.Controllers
{
    public class AnswerCreateModel 
    {
        [Required(ErrorMessage = "*required")]
        [StringLength(250, ErrorMessage = "Body  should be between 5 and 250 characters.", MinimumLength = 5)]
        public string Description { get; set; }

    }
}