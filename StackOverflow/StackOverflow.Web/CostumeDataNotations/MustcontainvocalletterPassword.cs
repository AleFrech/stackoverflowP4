using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class MustcontainvocalletterPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var stringvalue = value.ToString();
            stringvalue=stringvalue.ToLower();
            for (int i = 0; i < stringvalue.Length; i++)
            {
                if (stringvalue[i].Equals('a') || stringvalue[i].Equals('e') || stringvalue[i].Equals('i') ||
                    stringvalue[i].Equals('o') || stringvalue[i].Equals('u'))
                    return true;
            }
            return false;
        }
    }
}