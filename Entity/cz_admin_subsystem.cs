/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_admin_subsystem.cs
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
    /// 实体类cz_admin_subsystem
    /// </summary>
    [Serializable]
    public class cz_admin_subsystem
    {
        #region 私有字段

        private int? sys_id;
        private string conn;
        private int? flag;


        #endregion

        #region 公有属性


        public int? SysId
        {
            get => sys_id;
            set => sys_id = value;
        }

        public void set_sys_id (int? sys_id)
        {
            this.sys_id = sys_id;
        }
        public int? get_sys_id()
        {
            return  this.sys_id;
        }


        public string Conn
        {
            get => conn;
            set => conn = value;
        }

        public void set_conn (string conn)
        {
            this.conn = conn;
        }
        public string get_conn()
        {
            return  this.conn;
        }


        public int? Flag
        {
            get => flag;
            set => flag = value;
        }

        public void set_flag (int? flag)
        {
            this.flag = flag;
        }
        public int? get_flag()
        {
            return  this.flag;
        }



        #endregion	
    }
}