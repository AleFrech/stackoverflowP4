using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class MustcontainNumberPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var stringvalue = value.ToString();
            stringvalue = stringvalue.ToLower();
            return stringvalue.Any(char.IsDigit);
        }
    }
}