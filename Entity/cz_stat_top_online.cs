/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_stat_top_online.cs
 *      Description:
 *		
 *      Author:
 *				֣����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020��02��10��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// ʵ����cz_stat_top_online
    /// </summary>
    [Serializable]
    public class cz_stat_top_online
    {
        #region ˽���ֶ�

        private int? top_cnt;
        private DateTime? update_time;


        #endregion

        #region ��������


        public int? TopCnt
        {
           get => top_cnt;
           set => top_cnt = value;
        }

        public void set_top_cnt (int? top_cnt)
        {
            this.top_cnt = top_cnt;
        }
        public int? get_top_cnt()
        {
           return  this.top_cnt;
        }


        public DateTime? UpdateTime
        {
           get => update_time;
           set => update_time = value;
        }

        public void set_update_time (DateTime? update_time)
        {
            this.update_time = update_time;
        }
        public DateTime? get_update_time()
        {
           return  this.update_time;
        }



        #endregion	
    }
}
