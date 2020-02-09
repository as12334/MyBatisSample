/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_stat_online.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月10日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_stat_online
    /// </summary>
    [Serializable]
    public class cz_stat_online
    {
        #region 私有字段

        private string u_name;
        private int? is_out;
        private string u_type;
        private string ip;
        private DateTime? first_time;
        private DateTime? last_time;


        #endregion

        #region 公有属性


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


        public int? IsOut
        {
           get => is_out;
           set => is_out = value;
        }

        public void set_is_out (int? is_out)
        {
            this.is_out = is_out;
        }
        public int? get_is_out()
        {
           return  this.is_out;
        }


        public string UType
        {
           get => u_type;
           set => u_type = value;
        }

        public void set_u_type (string u_type)
        {
            this.u_type = u_type;
        }
        public string get_u_type()
        {
           return  this.u_type;
        }


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


        public DateTime? FirstTime
        {
           get => first_time;
           set => first_time = value;
        }

        public void set_first_time (DateTime? first_time)
        {
            this.first_time = first_time;
        }
        public DateTime? get_first_time()
        {
           return  this.first_time;
        }


        public DateTime? LastTime
        {
           get => last_time;
           set => last_time = value;
        }

        public void set_last_time (DateTime? last_time)
        {
            this.last_time = last_time;
        }
        public DateTime? get_last_time()
        {
           return  this.last_time;
        }



        #endregion	
    }
}
