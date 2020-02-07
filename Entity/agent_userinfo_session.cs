using System;
using Entity;

namespace LotterySystem.Model
{
	public class agent_userinfo_session
	{
		private string u_id;
		private string u_name;
		private string u_psw;
		private string u_nicker;
		private string u_skin;
		private string sup_name;
		private string u_type;
		private string su_type;
		private DateTime add_date;
		private int? six_rate;
		private decimal six_credit;
		private decimal six_usable_credit;
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
		private double kc_credit;
		private double kc_usable_credit;
		private string kc_kind;
		private int? kc_allow_sale;
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
		private int? negative_sale;
		
		
		
		
		private string dl_name;
		private string zd_name;
		private string gd_name;
		private string fgs_name;

		private cz_users_child users_child_session;
		private string zjname;


		public string get_u_id()
		{
			return u_id;
		}

		public void set_u_id(string u_id)
		{
			this.u_id = u_id;
		}



		public string get_u_name()
		{
			return u_name;
		}

		public void set_u_name(string u_name)
		{
			this.u_name = u_name;
		}

		public string get_u_psw()
		{
			return u_psw;
		}
		public int? get_negative_sale() {
			return negative_sale;
		}

		public void set_negative_sale(int? negative_sale) {
			this.negative_sale = negative_sale;
		}
		public void set_u_psw(string u_psw)
		{
			this.u_psw = u_psw;
		}


		public string get_u_nicker()
		{
			return u_nicker;
		}

		public void set_u_nicker(string u_nicker)
		{
			this.u_nicker = u_nicker;
		}

		public string get_u_skin()
		{
			return u_skin;
		}

		public void set_u_skin(string u_skin)
		{
			this.u_skin = u_skin;
		}

		public string get_sup_name()
		{
			return sup_name;
		}

		public void set_sup_name(string sup_name)
		{
			this.sup_name = sup_name;
		}

		public string get_u_type()
		{
			return u_type;
		}

		public void set_u_type(string u_type)
		{
			this.u_type = u_type;
		}

		public string get_su_type()
		{
			return su_type;
		}

		public void set_su_type(string su_type)
		{
			this.su_type = su_type;
		}

		public DateTime get_add_date()
		{
			return add_date;
		}


		public void set_six_kind(string six_kind)
		{
			this.six_kind = six_kind;
		}
		public string get_six_kind()
		{
			return six_kind;
		}
		public int? get_a_state()
		{
			return a_state;
		}

		public void set_a_state(int? a_state)
		{
			this.a_state = a_state;
		}

		public int? get_allow_sale()
		{
			return allow_sale;
		}

		public void set_allow_sale(int? allow_sale)
		{
			this.allow_sale = allow_sale;
		}

		public int? get_allow_view_report()
		{
			return allow_view_report;
		}

		public void set_allow_view_report(int? allow_view_report)
		{
			this.allow_view_report = allow_view_report;
		}

		public int? get_six_allow_maxrate()
		{
			return six_allow_maxrate;
		}


		public void set_kc_kind(string kc_kind)
		{
			this.kc_kind = kc_kind;
		}
		public string get_kc_kind()
		{
			return kc_kind;
		}

		public int? get_kc_allow_sale()
		{
			return kc_allow_sale;
		}

		public void set_kc_allow_sale(int? kc_allow_sale)
		{
			this.kc_allow_sale = kc_allow_sale;
		}

		public int? get_kc_allow_maxrate()
		{
			return kc_allow_maxrate;
		}

		public void set_kc_allow_maxrate(int? kc_allow_maxrate)
		{
			this.kc_allow_maxrate = kc_allow_maxrate;
		}

		public int? get_kc_low_maxrate()
		{
			return kc_low_maxrate;
		}

		public void set_kc_low_maxrate(int? kc_low_maxrate)
		{
			this.kc_low_maxrate = kc_low_maxrate;
		}

		public int? get_kc_rate_owner()
		{
			return kc_rate_owner;
		}

		public void set_kc_rate_owner(int? kc_rate_owner)
		{
			this.kc_rate_owner = kc_rate_owner;
		}

		public int? get_kc_crash_payment()
		{
			return kc_crash_payment;
		}

		public void set_kc_crash_payment(int? kc_crash_payment)
		{
			this.kc_crash_payment = kc_crash_payment;
		}

		public int? get_kc_iscash()
		{
			return kc_iscash;
		}

		public void set_kc_iscash(int? kc_iscash)
		{
			this.kc_iscash = kc_iscash;
		}

		public int? get_six_op_odds()
		{
			return six_op_odds;
		}

		public void set_six_op_odds(int? six_op_odds)
		{
			this.six_op_odds = six_op_odds;
		}

		public int? get_kc_op_odds()
		{
			return kc_op_odds;
		}

		public void set_kc_op_odds(int? kc_op_odds)
		{
			this.kc_op_odds = kc_op_odds;
		}

		public void set_users_child_session(cz_users_child child)
		{
			this.users_child_session = child;
		}
		public cz_users_child get_users_child_session()
		{
			return users_child_session;
		}

		public void set_zjname(string zjname)
		{
			this.zjname = zjname;
		}
		public string get_zjname()
		{
			return this.zjname;
		}
		
		
		public string get_dl_name() {
			return dl_name;
		}

		public void set_dl_name(string dl_name) {
			this.dl_name = dl_name;
		}

		public string get_zd_name() {
			return zd_name;
		}

		public void set_zd_name(string zd_name) {
			this.zd_name = zd_name;
		}

		public string get_gd_name() {
			return gd_name;
		}

		public void set_gd_name(string gd_name) {
			this.gd_name = gd_name;
		}

		public string get_fgs_name() {
			return fgs_name;
		}

		public void set_fgs_name(string fgs_name) {
			this.fgs_name = fgs_name;
		}

	}

}