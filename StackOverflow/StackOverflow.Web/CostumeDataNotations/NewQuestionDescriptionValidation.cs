using System.ComponentModel.DataAnnotations;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class NewQuestionDescriptionValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var stringvalue = value.ToString();
            System.Text.RegularExpressions.MatchCollection wordColl = System.Text.RegularExpressions.Regex.Matches(stringvalue, @"[\S]+");
            if (wordColl.Count < 5 )
            {
                return false;
            }

            return true;
        }
    }
}