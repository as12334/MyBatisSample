using System;
using System.Data.Common;
using System.Data.SqlClient;
using BuilderDALSQL;

namespace LotterySystem.DBUtility
{
    public class CommandInfo
    {
        public string CommandText;

        public EffentNextType EffentNextType;

        public object OriginalData;

        public DbParameter[] Parameters;

        public object ShareObject;

        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                this._solicitationEvent += value;
            }
            remove
            {
                this._solicitationEvent -= value;
            }
        }

        public CommandInfo()
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }

        public void OnSolicitationEvent()
        {
            if (this._solicitationEvent != null)
            {
                this._solicitationEvent(this, new EventArgs());
            }
        }
    }
}