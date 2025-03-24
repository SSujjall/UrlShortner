using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UrlShortner.Data.Filters
{
    public class GmailAttribute : ValidationAttribute
    {
        private const string GmailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string email = value.ToString();
            return Regex.IsMatch(email, GmailPattern);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid Gmail address.";
        }
    }
}
