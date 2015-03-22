using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class NewQuestionValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var stringvalue = value.ToString();
            System.Text.RegularExpressions.MatchCollection wordColl = System.Text.RegularExpressions.Regex.Matches(stringvalue, @"[\S]+");
            System.Text.RegularExpressions.MatchCollection charColl = System.Text.RegularExpressions.Regex.Matches(stringvalue, @".");
            if (wordColl.Count < 3 || charColl.Count > 50)
            {
                return false;
            }

            return true;
        }
    }
}