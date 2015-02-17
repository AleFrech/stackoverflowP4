using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.Models
{
    public class AccountPasswordCodeModel
    {
        [Required(ErrorMessage = "*required")]
        public string Code { get; set; }
    }
}