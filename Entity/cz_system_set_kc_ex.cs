/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_system_set_kc_ex.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月16日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_system_set_kc_ex
    /// </summary>
    [Serializable]
    public class cz_system_set_kc_ex
    {
        #region 私有字段

        private int? recovery_mode;
        private int? is_hideuser;
        private int? decimal_number;
        private int? max_number;
        private string report_open_date;


        #endregion

        #region 公有属性


        public int? RecoveryMode
        {
           get => recovery_mode;
           set => recovery_mode = value;
        }

        public void set_recovery_mode (int? recovery_mode)
        {
            this.recovery_mode = recovery_mode;
        }
        public int? get_recovery_mode()
        {
           return  this.recovery_mode;
        }


        public int? IsHideuser
        {
           get => is_hideuser;
           set => is_hideuser = value;
        }

        public void set_is_hideuser (int? is_hideuser)
        {
            this.is_hideuser = is_hideuser;
        }
        public int? get_is_hideuser()
        {
           return  this.is_hideuser;
        }


        public int? DecimalNumber
        {
           get => decimal_number;
           set => decimal_number = value;
        }

        public void set_decimal_number (int? decimal_number)
        {
            this.decimal_number = decimal_number;
        }
        public int? get_decimal_number()
        {
           return  this.decimal_number;
        }


        public int? MaxNumber
        {
           get => max_number;
           set => max_number = value;
        }

        public void set_max_number (int? max_number)
        {
            this.max_number = max_number;
        }
        public int? get_max_number()
        {
           return  this.max_number;
        }


        public string ReportOpenDate
        {
           get => report_open_date;
           set => report_open_date = value;
        }

        public void set_report_open_date (string report_open_date)
        {
            this.report_open_date = report_open_date;
        }
        public string get_report_open_date()
        {
           return  this.report_open_date;
        }



        #endregion	
    }
}
