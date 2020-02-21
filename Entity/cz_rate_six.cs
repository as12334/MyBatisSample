/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_rate_six.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月21日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_rate_six
    /// </summary>
    [Serializable]
    public class cz_rate_six
    {
        #region 私有字段

        private string u_name;
        private string u_type;
        private string dl_name;
        private string zd_name;
        private string gd_name;
        private string fgs_name;
        private int? dl_rate;
        private int? zd_rate;
        private int? gd_rate;
        private int? fgs_rate;
        private int? zj_rate;


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


        public string DlName
        {
           get => dl_name;
           set => dl_name = value;
        }

        public void set_dl_name (string dl_name)
        {
            this.dl_name = dl_name;
        }
        public string get_dl_name()
        {
           return  this.dl_name;
        }


        public string ZdName
        {
           get => zd_name;
           set => zd_name = value;
        }

        public void set_zd_name (string zd_name)
        {
            this.zd_name = zd_name;
        }
        public string get_zd_name()
        {
           return  this.zd_name;
        }


        public string GdName
        {
           get => gd_name;
           set => gd_name = value;
        }

        public void set_gd_name (string gd_name)
        {
            this.gd_name = gd_name;
        }
        public string get_gd_name()
        {
           return  this.gd_name;
        }


        public string FgsName
        {
           get => fgs_name;
           set => fgs_name = value;
        }

        public void set_fgs_name (string fgs_name)
        {
            this.fgs_name = fgs_name;
        }
        public string get_fgs_name()
        {
           return  this.fgs_name;
        }


        public int? DlRate
        {
           get => dl_rate;
           set => dl_rate = value;
        }

        public void set_dl_rate (int? dl_rate)
        {
            this.dl_rate = dl_rate;
        }
        public int? get_dl_rate()
        {
           return  this.dl_rate;
        }


        public int? ZdRate
        {
           get => zd_rate;
           set => zd_rate = value;
        }

        public void set_zd_rate (int? zd_rate)
        {
            this.zd_rate = zd_rate;
        }
        public int? get_zd_rate()
        {
           return  this.zd_rate;
        }


        public int? GdRate
        {
           get => gd_rate;
           set => gd_rate = value;
        }

        public void set_gd_rate (int? gd_rate)
        {
            this.gd_rate = gd_rate;
        }
        public int? get_gd_rate()
        {
           return  this.gd_rate;
        }


        public int? FgsRate
        {
           get => fgs_rate;
           set => fgs_rate = value;
        }

        public void set_fgs_rate (int? fgs_rate)
        {
            this.fgs_rate = fgs_rate;
        }
        public int? get_fgs_rate()
        {
           return  this.fgs_rate;
        }


        public int? ZjRate
        {
           get => zj_rate;
           set => zj_rate = value;
        }

        public void set_zj_rate (int? zj_rate)
        {
            this.zj_rate = zj_rate;
        }
        public int? get_zj_rate()
        {
           return  this.zj_rate;
        }



        #endregion	
    }
}
