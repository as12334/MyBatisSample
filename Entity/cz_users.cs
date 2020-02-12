/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_users.cs
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
    /// 实体类cz_users
    /// </summary>
    [Serializable]
    public class cz_users
    {
        #region 私有字段

        private string u_id;
        private string u_name;
        private string salt;
        private string u_nicker;
        private string u_skin;
        private string sup_name;
        private string u_type;
        private string su_type;
        private DateTime? add_date;
        private DateTime? last_changedate;
        private int? six_rate;
        private double? six_credit;
        private double? six_usable_credit;
        private string six_kind;
        private int? a_state;
        private int? allow_sale;
        private int? allow_view_report;
        private int? six_allow_maxrate;
        private int? six_low_maxrate;
        private int? six_rate_owner;
        private int? six_iscash;
        private int? allow_opt;
        private int? is_changed;
        private int? kc_rate;
        private double? kc_credit;
        private double? kc_usable_credit;
        private string kc_kind;
        private int? kc_allow_sale;
        private string negative_sale;
        private int? kc_allow_maxrate;
        private int? kc_low_maxrate;
        private int? kc_rate_owner;
        private int? kc_crash_payment;
        private int? kc_iscash;
        private int? six_op_odds;
        private int? kc_op_odds;
        private int? kc_isauto_back;
        private int? six_isauto_back;
        private int? retry_times;
        private string u_psw;


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


        public string SupName
        {
           get => sup_name;
           set => sup_name = value;
        }

        public void set_sup_name (string sup_name)
        {
            this.sup_name = sup_name;
        }
        public string get_sup_name()
        {
           return  this.sup_name;
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


        public string SuType
        {
           get => su_type;
           set => su_type = value;
        }

        public void set_su_type (string su_type)
        {
            this.su_type = su_type;
        }
        public string get_su_type()
        {
           return  this.su_type;
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


        public int? SixRate
        {
           get => six_rate;
           set => six_rate = value;
        }

        public void set_six_rate (int? six_rate)
        {
            this.six_rate = six_rate;
        }
        public int? get_six_rate()
        {
           return  this.six_rate;
        }


        public double? SixCredit
        {
           get => six_credit;
           set => six_credit = value;
        }

        public void set_six_credit (double? six_credit)
        {
            this.six_credit = six_credit;
        }
        public double? get_six_credit()
        {
           return  this.six_credit;
        }


        public double? SixUsableCredit
        {
           get => six_usable_credit;
           set => six_usable_credit = value;
        }

        public void set_six_usable_credit (double? six_usable_credit)
        {
            this.six_usable_credit = six_usable_credit;
        }
        public double? get_six_usable_credit()
        {
           return  this.six_usable_credit;
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


        public int? AState
        {
           get => a_state;
           set => a_state = value;
        }

        public void set_a_state (int? a_state)
        {
            this.a_state = a_state;
        }
        public int? get_a_state()
        {
           return  this.a_state;
        }


        public int? AllowSale
        {
           get => allow_sale;
           set => allow_sale = value;
        }

        public void set_allow_sale (int? allow_sale)
        {
            this.allow_sale = allow_sale;
        }
        public int? get_allow_sale()
        {
           return  this.allow_sale;
        }


        public int? AllowViewReport
        {
           get => allow_view_report;
           set => allow_view_report = value;
        }

        public void set_allow_view_report (int? allow_view_report)
        {
            this.allow_view_report = allow_view_report;
        }
        public int? get_allow_view_report()
        {
           return  this.allow_view_report;
        }


        public int? SixAllowMaxrate
        {
           get => six_allow_maxrate;
           set => six_allow_maxrate = value;
        }

        public void set_six_allow_maxrate (int? six_allow_maxrate)
        {
            this.six_allow_maxrate = six_allow_maxrate;
        }
        public int? get_six_allow_maxrate()
        {
           return  this.six_allow_maxrate;
        }


        public int? SixLowMaxrate
        {
           get => six_low_maxrate;
           set => six_low_maxrate = value;
        }

        public void set_six_low_maxrate (int? six_low_maxrate)
        {
            this.six_low_maxrate = six_low_maxrate;
        }
        public int? get_six_low_maxrate()
        {
           return  this.six_low_maxrate;
        }


        public int? SixRateOwner
        {
           get => six_rate_owner;
           set => six_rate_owner = value;
        }

        public void set_six_rate_owner (int? six_rate_owner)
        {
            this.six_rate_owner = six_rate_owner;
        }
        public int? get_six_rate_owner()
        {
           return  this.six_rate_owner;
        }


        public int? SixIscash
        {
           get => six_iscash;
           set => six_iscash = value;
        }

        public void set_six_iscash (int? six_iscash)
        {
            this.six_iscash = six_iscash;
        }
        public int? get_six_iscash()
        {
           return  this.six_iscash;
        }


        public int? AllowOpt
        {
           get => allow_opt;
           set => allow_opt = value;
        }

        public void set_allow_opt (int? allow_opt)
        {
            this.allow_opt = allow_opt;
        }
        public int? get_allow_opt()
        {
           return  this.allow_opt;
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


        public int? KcRate
        {
           get => kc_rate;
           set => kc_rate = value;
        }

        public void set_kc_rate (int? kc_rate)
        {
            this.kc_rate = kc_rate;
        }
        public int? get_kc_rate()
        {
           return  this.kc_rate;
        }


        public double? KcCredit
        {
           get => kc_credit;
           set => kc_credit = value;
        }

        public void set_kc_credit (double? kc_credit)
        {
            this.kc_credit = kc_credit;
        }
        public double? get_kc_credit()
        {
           return  this.kc_credit;
        }


        public double? KcUsableCredit
        {
           get => kc_usable_credit;
           set => kc_usable_credit = value;
        }

        public void set_kc_usable_credit (double? kc_usable_credit)
        {
            this.kc_usable_credit = kc_usable_credit;
        }
        public double? get_kc_usable_credit()
        {
           return  this.kc_usable_credit;
        }


        public string KcKind
        {
           get => kc_kind;
           set => kc_kind = value;
        }

        public void set_kc_kind (string kc_kind)
        {
            this.kc_kind = kc_kind;
        }
        public string get_kc_kind()
        {
           return  this.kc_kind;
        }


        public int? KcAllowSale
        {
           get => kc_allow_sale;
           set => kc_allow_sale = value;
        }

        public void set_kc_allow_sale (int? kc_allow_sale)
        {
            this.kc_allow_sale = kc_allow_sale;
        }
        public int? get_kc_allow_sale()
        {
           return  this.kc_allow_sale;
        }


        public string NegativeSale
        {
           get => negative_sale;
           set => negative_sale = value;
        }

        public void set_negative_sale (string negative_sale)
        {
            this.negative_sale = negative_sale;
        }
        public string get_negative_sale()
        {
           return  this.negative_sale;
        }


        public int? KcAllowMaxrate
        {
           get => kc_allow_maxrate;
           set => kc_allow_maxrate = value;
        }

        public void set_kc_allow_maxrate (int? kc_allow_maxrate)
        {
            this.kc_allow_maxrate = kc_allow_maxrate;
        }
        public int? get_kc_allow_maxrate()
        {
           return  this.kc_allow_maxrate;
        }


        public int? KcLowMaxrate
        {
           get => kc_low_maxrate;
           set => kc_low_maxrate = value;
        }

        public void set_kc_low_maxrate (int? kc_low_maxrate)
        {
            this.kc_low_maxrate = kc_low_maxrate;
        }
        public int? get_kc_low_maxrate()
        {
           return  this.kc_low_maxrate;
        }


        public int? KcRateOwner
        {
           get => kc_rate_owner;
           set => kc_rate_owner = value;
        }

        public void set_kc_rate_owner (int? kc_rate_owner)
        {
            this.kc_rate_owner = kc_rate_owner;
        }
        public int? get_kc_rate_owner()
        {
           return  this.kc_rate_owner;
        }


        public int? KcCrashPayment
        {
           get => kc_crash_payment;
           set => kc_crash_payment = value;
        }

        public void set_kc_crash_payment (int? kc_crash_payment)
        {
            this.kc_crash_payment = kc_crash_payment;
        }
        public int? get_kc_crash_payment()
        {
           return  this.kc_crash_payment;
        }


        public int? KcIscash
        {
           get => kc_iscash;
           set => kc_iscash = value;
        }

        public void set_kc_iscash (int? kc_iscash)
        {
            this.kc_iscash = kc_iscash;
        }
        public int? get_kc_iscash()
        {
           return  this.kc_iscash;
        }


        public int? SixOpOdds
        {
           get => six_op_odds;
           set => six_op_odds = value;
        }

        public void set_six_op_odds (int? six_op_odds)
        {
            this.six_op_odds = six_op_odds;
        }
        public int? get_six_op_odds()
        {
           return  this.six_op_odds;
        }


        public int? KcOpOdds
        {
           get => kc_op_odds;
           set => kc_op_odds = value;
        }

        public void set_kc_op_odds (int? kc_op_odds)
        {
            this.kc_op_odds = kc_op_odds;
        }
        public int? get_kc_op_odds()
        {
           return  this.kc_op_odds;
        }


        public int? KcIsautoBack
        {
           get => kc_isauto_back;
           set => kc_isauto_back = value;
        }

        public void set_kc_isauto_back (int? kc_isauto_back)
        {
            this.kc_isauto_back = kc_isauto_back;
        }
        public int? get_kc_isauto_back()
        {
           return  this.kc_isauto_back;
        }


        public int? SixIsautoBack
        {
           get => six_isauto_back;
           set => six_isauto_back = value;
        }

        public void set_six_isauto_back (int? six_isauto_back)
        {
            this.six_isauto_back = six_isauto_back;
        }
        public int? get_six_isauto_back()
        {
           return  this.six_isauto_back;
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



        #endregion	
    }
}
