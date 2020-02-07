/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_user_psw_err_log.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月08日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_user_psw_err_log
    /// </summary>
    [Serializable]
    public class cz_user_psw_err_log
    {
        #region 私有字段

        private string u_name;
        private int? err_times;
        private DateTime? update_date;


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


        public int? ErrTimes
        {
           get => err_times;
           set => err_times = value;
        }

        public void set_err_times (int? err_times)
        {
            this.err_times = err_times;
        }
        public int? get_err_times()
        {
           return  this.err_times;
        }


        public DateTime? UpdateDate
        {
           get => update_date;
           set => update_date = value;
        }

        public void set_update_date (DateTime? update_date)
        {
            this.update_date = update_date;
        }
        public DateTime? get_update_date()
        {
           return  this.update_date;
        }



        #endregion	
    }
}
