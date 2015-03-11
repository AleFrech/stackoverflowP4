using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class TermsandConditions : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            return value != null && value is bool && (bool)value;
        } 
    }
}