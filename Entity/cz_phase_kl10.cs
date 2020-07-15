/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_phase_kl10.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月21日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_phase_kl10
    /// </summary>
    [Serializable]
    public class cz_phase_kl10
    {
        #region 私有字段

        private DateTime? open_date;
        private DateTime? stop_date;
        private string phase;
        private string isopen;
        private string openning;
        private DateTime? endtime;
        private string n1;
        private string n2;
        private string n3;
        private string n4;
        private string n5;
        private string n6;
        private string n7;
        private string n8;


        #endregion

        #region 公有属性


        public DateTime? OpenDate
        {
           get => open_date;
           set => open_date = value;
        }

        public void set_open_date (DateTime? open_date)
        {
            this.open_date = open_date;
        }
        public DateTime? get_open_date()
        {
           return  this.open_date;
        }


        public DateTime? StopDate
        {
           get => stop_date;
           set => stop_date = value;
        }

        public void set_stop_date (DateTime? stop_date)
        {
            this.stop_date = stop_date;
        }
        public DateTime? get_stop_date()
        {
           return  this.stop_date;
        }


        public string Phase
        {
           get => phase;
           set => phase = value;
        }

        public void set_phase (string phase)
        {
            this.phase = phase;
        }
        public string get_phase()
        {
           return  this.phase;
        }


        public string Isopen
        {
           get => isopen;
           set => isopen = value;
        }

        public void set_isopen (string isopen)
        {
            this.isopen = isopen;
        }
        public string get_isopen()
        {
           return  this.isopen;
        }


        public string Openning
        {
           get => openning;
           set => openning = value;
        }

        public void set_openning (string openning)
        {
            this.openning = openning;
        }
        public string get_openning()
        {
           return  this.openning;
        }


        public DateTime? Endtime
        {
           get => endtime;
           set => endtime = value;
        }

        public void set_endtime (DateTime? endtime)
        {
            this.endtime = endtime;
        }
        public DateTime? get_endtime()
        {
           return  this.endtime;
        }


        public string N1
        {
           get => n1;
           set => n1 = value;
        }

        public void set_n1 (string n1)
        {
            this.n1 = n1;
        }
        public string get_n1()
        {
           return  this.n1;
        }


        public string N2
        {
           get => n2;
           set => n2 = value;
        }

        public void set_n2 (string n2)
        {
            this.n2 = n2;
        }
        public string get_n2()
        {
           return  this.n2;
        }


        public string N3
        {
           get => n3;
           set => n3 = value;
        }

        public void set_n3 (string n3)
        {
            this.n3 = n3;
        }
        public string get_n3()
        {
           return  this.n3;
        }


        public string N4
        {
           get => n4;
           set => n4 = value;
        }

        public void set_n4 (string n4)
        {
            this.n4 = n4;
        }
        public string get_n4()
        {
           return  this.n4;
        }


        public string N5
        {
           get => n5;
           set => n5 = value;
        }

        public void set_n5 (string n5)
        {
            this.n5 = n5;
        }
        public string get_n5()
        {
           return  this.n5;
        }


        public string N6
        {
           get => n6;
           set => n6 = value;
        }

        public void set_n6 (string n6)
        {
            this.n6 = n6;
        }
        public string get_n6()
        {
           return  this.n6;
        }


        public string N7
        {
           get => n7;
           set => n7 = value;
        }

        public void set_n7 (string n7)
        {
            this.n7 = n7;
        }
        public string get_n7()
        {
           return  this.n7;
        }


        public string N8
        {
           get => n8;
           set => n8 = value;
        }

        public void set_n8 (string n8)
        {
            this.n8 = n8;
        }
        public string get_n8()
        {
           return  this.n8;
        }



        #endregion	
    }
}
