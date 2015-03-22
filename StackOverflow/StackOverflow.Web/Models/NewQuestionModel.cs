using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StackOverflow.Web.CostumeDataNotations;

namespace StackOverflow.Web.Models
{
    public class NewQuestionModel
    {
        [Required(ErrorMessage = "*required")]
        [NewQuestionValidation(ErrorMessage = "Must have at least 3 words & a max of 50 caracters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*required")]
        [NewQuestionDescriptionValidation(ErrorMessage = "Must have at least 5 words")]
        public string Description { get; set; }

    }
}