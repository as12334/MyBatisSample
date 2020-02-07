using System;

namespace LotterySystem.Model
{
    public class cz_stat_online
    {
        private string u_name;
        private int is_out;
        private string u_type;
        private string ip;
        private DateTime? first_time;
        private DateTime? last_time;
        
        
        public string get_u_name() {
            return u_name;
        }

        public void set_u_name(string u_name) {
            this.u_name = u_name;
        }

        public int get_is_out() {
            return is_out;
        }

        public void set_is_out(int is_out) {
            this.is_out = is_out;
        }

        public string get_u_type() {
            return u_type;
        }

        public void set_u_type(string u_type) {
            this.u_type = u_type;
        }

        public string get_ip() {
            return ip;
        }

        public void set_ip(string ip) {
            this.ip = ip;
        }

        public DateTime? get_first_time() {
            return first_time;
        }

        public void set_first_time(DateTime? first_time) {
            this.first_time = first_time;
        }

        public DateTime? get_last_time() {
            return last_time;
        }

        public void set_last_time(DateTime? last_time) {
            this.last_time = last_time;
        }
    }
}