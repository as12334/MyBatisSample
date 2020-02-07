using System.Collections.Generic;

namespace LotterySystem.Model
{
    public class ReturnResult
    {
        private int success;
        private string tipinfo;
        private Dictionary<string, object> data;
        
        public int get_success() {
            return success;
        }

        public void set_success(int success) {
            this.success = success;
        }

        public string get_tipinfo() {
            return tipinfo;
        }

        public void set_tipinfo(string tipinfo) {
            this.tipinfo = tipinfo;
        }
        
        
        public string get_data() {
            return tipinfo;
        }

        public void set_data(Dictionary<string, object> data) {
            this.data = data;
        }

        public int Success
        {
            get => success;
            set => success = value;
        }

        public string Tipinfo
        {
            get => tipinfo;
            set => tipinfo = value;
        }

        public Dictionary<string, object> Data
        {
            get => data;
            set => data = value;
        }
    }
}