using System;
using System.IO;
using Agent.Web.WebBase;

namespace WebGame
{
    public class Global : System.Web.HttpApplication
    {
        private static System.Timers.Timer timer = null;
        private static string lastBackupDate = null;
        private static object lockBackupObj = new object();


        protected void Application_Start(object sender, EventArgs e)
        {
            CallBLL.Call();
//            BLLComm.Logger.SetPath(System.Web.HttpContext.Current.Server.MapPath("~/global.log"));
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 60 * 1000;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
               
//                BLLComm.Logger.Log("初始化定时作业成功...");
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
//            var now = DateTime.Now;
//            if (now.ToString("yyyy-MM-dd") != lastBackupDate)
//            {
//                if (now.TimeOfDay.Hours >= 6)
//                {
//                    try
//                    {
//                        lock (lockBackupObj)
//                        {
//                            BLLComm.Logger.Log("定时作业即将运行...");
//                            var ua = new UserApp();
//                            ua.BackupDetails();
//                            lastBackupDate = now.ToString("yyyy-MM-dd");
//                            BLLComm.Logger.Log("定时作业运行完成...");
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        BLLComm.Logger.Log("定时作业执行过程中出现错误：", ex);
//                    }
//                }
//            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }


}