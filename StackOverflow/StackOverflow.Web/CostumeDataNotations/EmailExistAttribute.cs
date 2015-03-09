using System.ComponentModel.DataAnnotations;
using StackOverflow.Data;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.CostumeDataNotations
{
    public class EmailExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var stringEmail = value.ToString();
            var context = new StackOverflowContext();
            foreach (Account a in context.Accounts)
            {
                if (a.Email.Equals(stringEmail))
                    return false;
            }

            return true;
        }
    }
}