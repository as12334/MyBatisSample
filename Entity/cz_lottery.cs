/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_lottery.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月13日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_lottery
    /// </summary>
    [Serializable]
    public class cz_lottery
    {
        #region 私有字段

        private int? id;
        private string lottery_name;
        private int? night_flag;
        private string code;
        private int? master_id;


        #endregion

        #region 公有属性


        public int? Id
        {
           get => id;
           set => id = value;
        }

        public void set_id (int? id)
        {
            this.id = id;
        }
        public int? get_id()
        {
           return  this.id;
        }


        public string LotteryName
        {
           get => lottery_name;
           set => lottery_name = value;
        }

        public void set_lottery_name (string lottery_name)
        {
            this.lottery_name = lottery_name;
        }
        public string get_lottery_name()
        {
           return  this.lottery_name;
        }


        public int? NightFlag
        {
           get => night_flag;
           set => night_flag = value;
        }

        public void set_night_flag (int? night_flag)
        {
            this.night_flag = night_flag;
        }
        public int? get_night_flag()
        {
           return  this.night_flag;
        }


        public string Code
        {
           get => code;
           set => code = value;
        }

        public void set_code (string code)
        {
            this.code = code;
        }
        public string get_code()
        {
           return  this.code;
        }


        public int? MasterId
        {
           get => master_id;
           set => master_id = value;
        }

        public void set_master_id (int? master_id)
        {
            this.master_id = master_id;
        }
        public int? get_master_id()
        {
           return  this.master_id;
        }



        #endregion	
    }
}
