/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_users_child.cs
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
    /// 实体类cz_users_child
    /// </summary>
    [Serializable]
    public class cz_users_child
    {
        #region 私有字段

        private string u_id;
        private string u_name;
        private string salt;
        private string u_nicker;
        private string u_skin;
        private string u_psw;
        private string parent_u_name;
        private DateTime? add_date;
        private DateTime? last_changedate;
        private int? status;
        private string permissions_name;
        private int? retry_times;
        private int? is_admin;
        private int? is_changed;


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


        public string Salt
        {
           get => salt;
           set => salt = value;
        }

        public void set_salt (string salt)
        {
            this.salt = salt;
        }
        public string get_salt()
        {
           return  this.salt;
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


        public string USkin
        {
           get => u_skin;
           set => u_skin = value;
        }

        public void set_u_skin (string u_skin)
        {
            this.u_skin = u_skin;
        }
        public string get_u_skin()
        {
           return  this.u_skin;
        }


        public string UPsw
        {
           get => u_psw;
           set => u_psw = value;
        }

        public void set_u_psw (string u_psw)
        {
            this.u_psw = u_psw;
        }
        public string get_u_psw()
        {
           return  this.u_psw;
        }


        public string ParentUName
        {
           get => parent_u_name;
           set => parent_u_name = value;
        }

        public void set_parent_u_name (string parent_u_name)
        {
            this.parent_u_name = parent_u_name;
        }
        public string get_parent_u_name()
        {
           return  this.parent_u_name;
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


        public DateTime? LastChangedate
        {
           get => last_changedate;
           set => last_changedate = value;
        }

        public void set_last_changedate (DateTime? last_changedate)
        {
            this.last_changedate = last_changedate;
        }
        public DateTime? get_last_changedate()
        {
           return  this.last_changedate;
        }


        public int? Status
        {
           get => status;
           set => status = value;
        }

        public void set_status (int? status)
        {
            this.status = status;
        }
        public int? get_status()
        {
           return  this.status;
        }


        public string PermissionsName
        {
           get => permissions_name;
           set => permissions_name = value;
        }

        public void set_permissions_name (string permissions_name)
        {
            this.permissions_name = permissions_name;
        }
        public string get_permissions_name()
        {
           return  this.permissions_name;
        }


        public int? RetryTimes
        {
           get => retry_times;
           set => retry_times = value;
        }

        public void set_retry_times (int? retry_times)
        {
            this.retry_times = retry_times;
        }
        public int? get_retry_times()
        {
           return  this.retry_times;
        }


        public int? IsAdmin
        {
           get => is_admin;
           set => is_admin = value;
        }

        public void set_is_admin (int? is_admin)
        {
            this.is_admin = is_admin;
        }
        public int? get_is_admin()
        {
           return  this.is_admin;
        }


        public int? IsChanged
        {
           get => is_changed;
           set => is_changed = value;
        }

        public void set_is_changed (int? is_changed)
        {
            this.is_changed = is_changed;
        }
        public int? get_is_changed()
        {
           return  this.is_changed;
        }



        #endregion	
    }
}
