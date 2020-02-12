/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				zk_subsys.cs
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
    /// 实体类zk_subsys
    /// </summary>
    [Serializable]
    public class zk_subsys
    {
        #region 私有字段

        private string sys_id;
        private string sync;


        #endregion

        #region 公有属性


        public string SysId
        {
           get => sys_id;
           set => sys_id = value;
        }

        public void set_sys_id (string sys_id)
        {
            this.sys_id = sys_id;
        }
        public string get_sys_id()
        {
           return  this.sys_id;
        }


        public string Sync
        {
           get => sync;
           set => sync = value;
        }

        public void set_sync (string sync)
        {
            this.sync = sync;
        }
        public string get_sync()
        {
           return  this.sync;
        }



        #endregion	
    }
}
