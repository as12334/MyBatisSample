/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_bet_kc.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月16日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_bet_kc
    /// </summary>
    [Serializable]
    public class cz_bet_kc
    {
        #region 私有字段

        private string order_num;
        private string checkcode;
        private string u_type;
        private string u_name;
        private string u_nicker;
        private int? phase_id;
        private string phase;
        private DateTime? bet_time;
        private int? odds_id;
        private string category;
        private string play_name;
        private int? play_id;
        private string bet_val;
        private string bet_wt;
        private string odds;
        private decimal? amount;
        private decimal? profit;
        private decimal? hy_drawback;
        private decimal? dl_drawback;
        private decimal? zd_drawback;
        private decimal? gd_drawback;
        private decimal? fgs_drawback;
        private decimal? zj_drawback;
        private decimal? dl_rate;
        private decimal? zd_rate;
        private decimal? gd_rate;
        private decimal? fgs_rate;
        private decimal? zj_rate;
        private string dl_name;
        private string zd_name;
        private string gd_name;
        private string fgs_name;
        private int? is_payment;
        private int? sale_type;
        private int? m_type;
        private string kind;
        private string ip;
        private int? lottery_type;
        private string lottery_name;
        private string ordervalidcode;
        private string odds_zj;


        #endregion

        #region 公有属性


        public string OrderNum
        {
           get => order_num;
           set => order_num = value;
        }

        public void set_order_num (string order_num)
        {
            this.order_num = order_num;
        }
        public string get_order_num()
        {
           return  this.order_num;
        }


        public string Checkcode
        {
           get => checkcode;
           set => checkcode = value;
        }

        public void set_checkcode (string checkcode)
        {
            this.checkcode = checkcode;
        }
        public string get_checkcode()
        {
           return  this.checkcode;
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


        public int? PhaseId
        {
           get => phase_id;
           set => phase_id = value;
        }

        public void set_phase_id (int? phase_id)
        {
            this.phase_id = phase_id;
        }
        public int? get_phase_id()
        {
           return  this.phase_id;
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


        public DateTime? BetTime
        {
           get => bet_time;
           set => bet_time = value;
        }

        public void set_bet_time (DateTime? bet_time)
        {
            this.bet_time = bet_time;
        }
        public DateTime? get_bet_time()
        {
           return  this.bet_time;
        }


        public int? OddsId
        {
           get => odds_id;
           set => odds_id = value;
        }

        public void set_odds_id (int? odds_id)
        {
            this.odds_id = odds_id;
        }
        public int? get_odds_id()
        {
           return  this.odds_id;
        }


        public string Category
        {
           get => category;
           set => category = value;
        }

        public void set_category (string category)
        {
            this.category = category;
        }
        public string get_category()
        {
           return  this.category;
        }


        public string PlayName
        {
           get => play_name;
           set => play_name = value;
        }

        public void set_play_name (string play_name)
        {
            this.play_name = play_name;
        }
        public string get_play_name()
        {
           return  this.play_name;
        }


        public int? PlayId
        {
           get => play_id;
           set => play_id = value;
        }

        public void set_play_id (int? play_id)
        {
            this.play_id = play_id;
        }
        public int? get_play_id()
        {
           return  this.play_id;
        }


        public string BetVal
        {
           get => bet_val;
           set => bet_val = value;
        }

        public void set_bet_val (string bet_val)
        {
            this.bet_val = bet_val;
        }
        public string get_bet_val()
        {
           return  this.bet_val;
        }


        public string BetWt
        {
           get => bet_wt;
           set => bet_wt = value;
        }

        public void set_bet_wt (string bet_wt)
        {
            this.bet_wt = bet_wt;
        }
        public string get_bet_wt()
        {
           return  this.bet_wt;
        }


        public string Odds
        {
           get => odds;
           set => odds = value;
        }

        public void set_odds (string odds)
        {
            this.odds = odds;
        }
        public string get_odds()
        {
           return  this.odds;
        }


        public decimal? Amount
        {
           get => amount;
           set => amount = value;
        }

        public void set_amount (decimal? amount)
        {
            this.amount = amount;
        }
        public decimal? get_amount()
        {
           return  this.amount;
        }


        public decimal? Profit
        {
           get => profit;
           set => profit = value;
        }

        public void set_profit (decimal? profit)
        {
            this.profit = profit;
        }
        public decimal? get_profit()
        {
           return  this.profit;
        }


        public decimal? HyDrawback
        {
           get => hy_drawback;
           set => hy_drawback = value;
        }

        public void set_hy_drawback (decimal? hy_drawback)
        {
            this.hy_drawback = hy_drawback;
        }
        public decimal? get_hy_drawback()
        {
           return  this.hy_drawback;
        }


        public decimal? DlDrawback
        {
           get => dl_drawback;
           set => dl_drawback = value;
        }

        public void set_dl_drawback (decimal? dl_drawback)
        {
            this.dl_drawback = dl_drawback;
        }
        public decimal? get_dl_drawback()
        {
           return  this.dl_drawback;
        }


        public decimal? ZdDrawback
        {
           get => zd_drawback;
           set => zd_drawback = value;
        }

        public void set_zd_drawback (decimal? zd_drawback)
        {
            this.zd_drawback = zd_drawback;
        }
        public decimal? get_zd_drawback()
        {
           return  this.zd_drawback;
        }


        public decimal? GdDrawback
        {
           get => gd_drawback;
           set => gd_drawback = value;
        }

        public void set_gd_drawback (decimal? gd_drawback)
        {
            this.gd_drawback = gd_drawback;
        }
        public decimal? get_gd_drawback()
        {
           return  this.gd_drawback;
        }


        public decimal? FgsDrawback
        {
           get => fgs_drawback;
           set => fgs_drawback = value;
        }

        public void set_fgs_drawback (decimal? fgs_drawback)
        {
            this.fgs_drawback = fgs_drawback;
        }
        public decimal? get_fgs_drawback()
        {
           return  this.fgs_drawback;
        }


        public decimal? ZjDrawback
        {
           get => zj_drawback;
           set => zj_drawback = value;
        }

        public void set_zj_drawback (decimal? zj_drawback)
        {
            this.zj_drawback = zj_drawback;
        }
        public decimal? get_zj_drawback()
        {
           return  this.zj_drawback;
        }


        public decimal? DlRate
        {
           get => dl_rate;
           set => dl_rate = value;
        }

        public void set_dl_rate (decimal? dl_rate)
        {
            this.dl_rate = dl_rate;
        }
        public decimal? get_dl_rate()
        {
           return  this.dl_rate;
        }


        public decimal? ZdRate
        {
           get => zd_rate;
           set => zd_rate = value;
        }

        public void set_zd_rate (decimal? zd_rate)
        {
            this.zd_rate = zd_rate;
        }
        public decimal? get_zd_rate()
        {
           return  this.zd_rate;
        }


        public decimal? GdRate
        {
           get => gd_rate;
           set => gd_rate = value;
        }

        public void set_gd_rate (decimal? gd_rate)
        {
            this.gd_rate = gd_rate;
        }
        public decimal? get_gd_rate()
        {
           return  this.gd_rate;
        }


        public decimal? FgsRate
        {
           get => fgs_rate;
           set => fgs_rate = value;
        }

        public void set_fgs_rate (decimal? fgs_rate)
        {
            this.fgs_rate = fgs_rate;
        }
        public decimal? get_fgs_rate()
        {
           return  this.fgs_rate;
        }


        public decimal? ZjRate
        {
           get => zj_rate;
           set => zj_rate = value;
        }

        public void set_zj_rate (decimal? zj_rate)
        {
            this.zj_rate = zj_rate;
        }
        public decimal? get_zj_rate()
        {
           return  this.zj_rate;
        }


        public string DlName
        {
           get => dl_name;
           set => dl_name = value;
        }

        public void set_dl_name (string dl_name)
        {
            this.dl_name = dl_name;
        }
        public string get_dl_name()
        {
           return  this.dl_name;
        }


        public string ZdName
        {
           get => zd_name;
           set => zd_name = value;
        }

        public void set_zd_name (string zd_name)
        {
            this.zd_name = zd_name;
        }
        public string get_zd_name()
        {
           return  this.zd_name;
        }


        public string GdName
        {
           get => gd_name;
           set => gd_name = value;
        }

        public void set_gd_name (string gd_name)
        {
            this.gd_name = gd_name;
        }
        public string get_gd_name()
        {
           return  this.gd_name;
        }


        public string FgsName
        {
           get => fgs_name;
           set => fgs_name = value;
        }

        public void set_fgs_name (string fgs_name)
        {
            this.fgs_name = fgs_name;
        }
        public string get_fgs_name()
        {
           return  this.fgs_name;
        }


        public int? IsPayment
        {
           get => is_payment;
           set => is_payment = value;
        }

        public void set_is_payment (int? is_payment)
        {
            this.is_payment = is_payment;
        }
        public int? get_is_payment()
        {
           return  this.is_payment;
        }


        public int? SaleType
        {
           get => sale_type;
           set => sale_type = value;
        }

        public void set_sale_type (int? sale_type)
        {
            this.sale_type = sale_type;
        }
        public int? get_sale_type()
        {
           return  this.sale_type;
        }


        public int? MType
        {
           get => m_type;
           set => m_type = value;
        }

        public void set_m_type (int? m_type)
        {
            this.m_type = m_type;
        }
        public int? get_m_type()
        {
           return  this.m_type;
        }


        public string Kind
        {
           get => kind;
           set => kind = value;
        }

        public void set_kind (string kind)
        {
            this.kind = kind;
        }
        public string get_kind()
        {
           return  this.kind;
        }


        public string Ip
        {
           get => ip;
           set => ip = value;
        }

        public void set_ip (string ip)
        {
            this.ip = ip;
        }
        public string get_ip()
        {
           return  this.ip;
        }


        public int? LotteryType
        {
           get => lottery_type;
           set => lottery_type = value;
        }

        public void set_lottery_type (int? lottery_type)
        {
            this.lottery_type = lottery_type;
        }
        public int? get_lottery_type()
        {
           return  this.lottery_type;
        }


        public string LotteryName
        {
           get => lottery_name;
           set => lottery_name = value;
        }

        public void set_lottery_name (string lottery_name)
        {
            this.lottery_name = lottery_name;
        }
        public string get_lottery_name()
        {
           return  this.lottery_name;
        }


        public string Ordervalidcode
        {
           get => ordervalidcode;
           set => ordervalidcode = value;
        }

        public void set_ordervalidcode (string ordervalidcode)
        {
            this.ordervalidcode = ordervalidcode;
        }
        public string get_ordervalidcode()
        {
           return  this.ordervalidcode;
        }


        public string OddsZj
        {
           get => odds_zj;
           set => odds_zj = value;
        }

        public void set_odds_zj (string odds_zj)
        {
            this.odds_zj = odds_zj;
        }
        public string get_odds_zj()
        {
           return  this.odds_zj;
        }



        #endregion	
    }
}
