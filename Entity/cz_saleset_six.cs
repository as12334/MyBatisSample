/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_saleset_six.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月12日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_saleset_six
    /// </summary>
    [Serializable]
    public class cz_saleset_six
    {
        #region 私有字段

        private string u_id;
        private string u_name;
        private string u_nicker;
        private string six_kind;
        private DateTime? add_date;


        #endregion

        #region 公有属性


        public string UId
        {
           get => u_id;
           set => u_id = value;
        }

        public void set_u_id (string u_id)
        {
            this.u_id = u_id;
        }
        public string get_u_id()
        {
           return  this.u_id;
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


        public string UNicker
        {
           get => u_nicker;
           set => u_nicker = value;
        }

        public void set_u_nicker (string u_nicker)
        {
            this.u_nicker = u_nicker;
        }
        public string get_u_nicker()
        {
           return  this.u_nicker;
        }


        public string SixKind
        {
           get => six_kind;
           set => six_kind = value;
        }

        public void set_six_kind (string six_kind)
        {
            this.six_kind = six_kind;
        }
        public string get_six_kind()
        {
           return  this.six_kind;
        }


        public DateTime? AddDate
        {
           get => add_date;
           set => add_date = value;
        }

        public void set_add_date (DateTime? add_date)
        {
            this.add_date = add_date;
        }
        public DateTime? get_add_date()
        {
           return  this.add_date;
        }



        #endregion	
    }
}
