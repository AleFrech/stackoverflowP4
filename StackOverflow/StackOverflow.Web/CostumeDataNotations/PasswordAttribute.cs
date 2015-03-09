using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class PasswordAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var stringvalue = value.ToString();
            string stringvalue2 = stringvalue.ToLower();
            if (stringvalue.Equals(stringvalue2))
                return false;

            return true;
        }
    }
}