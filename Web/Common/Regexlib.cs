using System.Text.RegularExpressions;

namespace LotterySystem.Common
{
    public class Regexlib
    {
        public static bool IsValidPassword(string trim, string getGetPasswordLu)
        {
            if (getGetPasswordLu.Equals(1))
            {
                return Regex.IsMatch(trim, @"^[a-z0-9A-Z][a-z0-9A-Z]{7,20}$");
            }
            else
            {
                return Regex.IsMatch(trim, @"^[a-z0-9][a-z0-9]{7,20}$");
            }
        }
    }
}