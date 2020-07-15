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
            string text = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            string result;
            if (text == "::1")
            {
                result = "127.0.0.1";
            }
            else
            {
                result = text;
            }
            return result;
        }

        public static HttpCookie GetReportCookies()
        {
            throw new System.NotImplementedException();
        }
    }
}