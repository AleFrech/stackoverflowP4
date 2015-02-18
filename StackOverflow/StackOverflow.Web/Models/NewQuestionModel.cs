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
        public string Title { get; set; }
        [Required(ErrorMessage = "*required")]
        public string Description { get; set; }


    }
}