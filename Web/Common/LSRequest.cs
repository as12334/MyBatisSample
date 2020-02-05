using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace LotterySystem.Common
{    public class LSRequest

    {

        public static string qq(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] != null)
            {
                if (IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
                {
                    return HttpContext.Current.Request.Form[strName];
                }
                

            }
            if (HttpContext.Current.Request.QueryString[strName] != null)
            {
                if (IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
                {
                    return HttpContext.Current.Request.QueryString[strName];
                }

            }
            return "unsafe string";
            
        }
        public static bool IsSafeSqlString(string targetString)
        {
            return !Regex.IsMatch(targetString, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        public static string GetIP()
        {
            throw new System.NotImplementedException();
        }
    }
}