/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_login_log.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月07日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_login_log
    /// </summary>
    [Serializable]
    public class cz_login_log
    {
        #region 私有字段

        private string ip;
        private DateTime? login_time;
        private string u_name;
        private string browser_type;


        #endregion

        #region 公有属性


        public string Ip
        {
           get => ip;
           set => ip = value;
        }

        public void set_ip (string ip)
        {
            this.ip = ip;
        }
        public string get_ip()
        {
           return  this.ip;
        }


        public DateTime? LoginTime
        {
           get => login_time;
           set => login_time = value;
        }

        public void set_login_time (DateTime? login_time)
        {
            this.login_time = login_time;
        }
        public DateTime? get_login_time()
        {
           return  this.login_time;
        }


        public string UName
        {
           get => u_name;
           set => u_name = value;
        }

        public void set_u_name (string u_name)
        {
            this.u_name = u_name;
        }
        public string get_u_name()
        {
           return  this.u_name;
        }


        public string BrowserType
        {
           get => browser_type;
           set => browser_type = value;
        }

        public void set_browser_type (string browser_type)
        {
            this.browser_type = browser_type;
        }
        public string get_browser_type()
        {
           return  this.browser_type;
        }



        #endregion	
    }
}
