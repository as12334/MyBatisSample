/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_jp_odds.cs
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
    /// 实体类cz_jp_odds
    /// </summary>
    [Serializable]
    public class cz_jp_odds
    {
        #region 私有字段

        private DateTime? add_time;
        private int? odds_id;
        private int? phase_id;
        private string play_name;
        private string put_amount;
        private string odds;
        private int? lottery_type;
        private string phase;
        private string old_odds;
        private decimal? new_odds;


        #endregion

        #region 公有属性


        public DateTime? AddTime
        {
           get => add_time;
           set => add_time = value;
        }

        public void set_add_time (DateTime? add_time)
        {
            this.add_time = add_time;
        }
        public DateTime? get_add_time()
        {
           return  this.add_time;
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


        public string PutAmount
        {
           get => put_amount;
           set => put_amount = value;
        }

        public void set_put_amount (string put_amount)
        {
            this.put_amount = put_amount;
        }
        public string get_put_amount()
        {
           return  this.put_amount;
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


        public string OldOdds
        {
           get => old_odds;
           set => old_odds = value;
        }

        public void set_old_odds (string old_odds)
        {
            this.old_odds = old_odds;
        }
        public string get_old_odds()
        {
           return  this.old_odds;
        }


        public decimal? NewOdds
        {
           get => new_odds;
           set => new_odds = value;
        }

        public void set_new_odds (decimal? new_odds)
        {
            this.new_odds = new_odds;
        }
        public decimal? get_new_odds()
        {
           return  this.new_odds;
        }



        #endregion	
    }
}
