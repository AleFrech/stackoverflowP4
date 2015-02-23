using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class NewQuestionModel
    {
        [Required(ErrorMessage = "*required")]
        [StringLength(50, ErrorMessage = "Body should be between 5 and 50 characters.", MinimumLength = 5)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*required")]
        [StringLength(250, ErrorMessage = "Body should be between 5 and 250 characters.", MinimumLength = 5)]
        public string Description { get; set; }


    }
}