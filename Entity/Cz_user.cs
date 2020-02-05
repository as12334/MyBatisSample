/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				Cz_user.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月05日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类Cz_user
    /// </summary>
    [Serializable]
    public class Cz_user
    {
        #region 私有字段

        private string _u_id;
        private string _u_name;
        private string _salt;
        private string _u_nicker;
        private string _u_skin;
        private string _sup_name;
        private string _u_type;
        private string _su_type;
        private DateTime? _add_date;
        private DateTime? _last_changedate;
        private int? _six_rate;
        private double? _six_credit;
        private double? _six_usable_credit;
        private string _six_kind;
        private int? _a_state;
        private int? _allow_sale;
        private int? _allow_view_report;
        private int? _six_allow_maxrate;
        private int? _six_low_maxrate;
        private int? _six_rate_owner;
        private int? _six_iscash;
        private int? _allow_opt;
        private int? _is_changed;
        private int? _kc_rate;
        private double? _kc_credit;
        private double? _kc_usable_credit;
        private string _kc_kind;
        private int? _kc_allow_sale;
        private int? _negative_sale;
        private int? _kc_allow_maxrate;
        private int? _kc_low_maxrate;
        private int? _kc_rate_owner;
        private int? _kc_crash_payment;
        private int? _kc_iscash;
        private int? _six_op_odds;
        private int? _kc_op_odds;
        private int? _kc_isauto_back;
        private int? _six_isauto_back;
        private int? _retry_times;
        private string _u_psw;


        #endregion

        #region 公有属性


        public string U_id
        {
            set { this._u_id = value; }
            get { return this._u_id; }
        }


        public string U_name
        {
            set { this._u_name = value; }
            get { return this._u_name; }
        }


        public string Salt
        {
            set { this._salt = value; }
            get { return this._salt; }
        }


        public string U_nicker
        {
            set { this._u_nicker = value; }
            get { return this._u_nicker; }
        }


        public string U_skin
        {
            set { this._u_skin = value; }
            get { return this._u_skin; }
        }


        public string Sup_name
        {
            set { this._sup_name = value; }
            get { return this._sup_name; }
        }


        public string U_type
        {
            set { this._u_type = value; }
            get { return this._u_type; }
        }


        public string Su_type
        {
            set { this._su_type = value; }
            get { return this._su_type; }
        }


        public DateTime? Add_date
        {
            set { this._add_date = value; }
            get { return this._add_date; }
        }


        public DateTime? Last_changedate
        {
            set { this._last_changedate = value; }
            get { return this._last_changedate; }
        }


        public int? Six_rate
        {
            set { this._six_rate = value; }
            get { return this._six_rate; }
        }


        public double? Six_credit
        {
            set { this._six_credit = value; }
            get { return this._six_credit; }
        }


        public double? Six_usable_credit
        {
            set { this._six_usable_credit = value; }
            get { return this._six_usable_credit; }
        }


        public string Six_kind
        {
            set { this._six_kind = value; }
            get { return this._six_kind; }
        }


        public int? A_state
        {
            set { this._a_state = value; }
            get { return this._a_state; }
        }


        public int? Allow_sale
        {
            set { this._allow_sale = value; }
            get { return this._allow_sale; }
        }


        public int? Allow_view_report
        {
            set { this._allow_view_report = value; }
            get { return this._allow_view_report; }
        }


        public int? Six_allow_maxrate
        {
            set { this._six_allow_maxrate = value; }
            get { return this._six_allow_maxrate; }
        }


        public int? Six_low_maxrate
        {
            set { this._six_low_maxrate = value; }
            get { return this._six_low_maxrate; }
        }


        public int? Six_rate_owner
        {
            set { this._six_rate_owner = value; }
            get { return this._six_rate_owner; }
        }


        public int? Six_iscash
        {
            set { this._six_iscash = value; }
            get { return this._six_iscash; }
        }


        public int? Allow_opt
        {
            set { this._allow_opt = value; }
            get { return this._allow_opt; }
        }


        public int? Is_changed
        {
            set { this._is_changed = value; }
            get { return this._is_changed; }
        }


        public int? Kc_rate
        {
            set { this._kc_rate = value; }
            get { return this._kc_rate; }
        }


        public double? Kc_credit
        {
            set { this._kc_credit = value; }
            get { return this._kc_credit; }
        }


        public double? Kc_usable_credit
        {
            set { this._kc_usable_credit = value; }
            get { return this._kc_usable_credit; }
        }


        public string Kc_kind
        {
            set { this._kc_kind = value; }
            get { return this._kc_kind; }
        }


        public int? Kc_allow_sale
        {
            set { this._kc_allow_sale = value; }
            get { return this._kc_allow_sale; }
        }


        public int? Negative_sale
        {
            set { this._negative_sale = value; }
            get { return this._negative_sale; }
        }


        public int? Kc_allow_maxrate
        {
            set { this._kc_allow_maxrate = value; }
            get { return this._kc_allow_maxrate; }
        }


        public int? Kc_low_maxrate
        {
            set { this._kc_low_maxrate = value; }
            get { return this._kc_low_maxrate; }
        }


        public int? Kc_rate_owner
        {
            set { this._kc_rate_owner = value; }
            get { return this._kc_rate_owner; }
        }


        public int? Kc_crash_payment
        {
            set { this._kc_crash_payment = value; }
            get { return this._kc_crash_payment; }
        }


        public int? Kc_iscash
        {
            set { this._kc_iscash = value; }
            get { return this._kc_iscash; }
        }


        public int? Six_op_odds
        {
            set { this._six_op_odds = value; }
            get { return this._six_op_odds; }
        }


        public int? Kc_op_odds
        {
            set { this._kc_op_odds = value; }
            get { return this._kc_op_odds; }
        }


        public int? Kc_isauto_back
        {
            set { this._kc_isauto_back = value; }
            get { return this._kc_isauto_back; }
        }


        public int? Six_isauto_back
        {
            set { this._six_isauto_back = value; }
            get { return this._six_isauto_back; }
        }


        public int? Retry_times
        {
            set { this._retry_times = value; }
            get { return this._retry_times; }
        }


        public string U_psw
        {
            set { this._u_psw = value; }
            get { return this._u_psw; }
        }



        #endregion	
    }
}
