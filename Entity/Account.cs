/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				Account.cs
 *      Description:
 *		
 *      Author:
 *				֣����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020��02��05��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// ʵ����Account
    /// </summary>
    [Serializable]
    public class Account
    {
        #region ˽���ֶ�

        private int? _account_ID;
        private string _account_FirstName;
        private string _account_LastName;
        private string _account_Email;
        private string _account_Banner_Option;
        private int? _account_Cart_Option;


        #endregion

        #region ��������


        public int? Account_ID
        {
            set { this._account_ID = value; }
            get { return this._account_ID; }
        }


        public string Account_FirstName
        {
            set { this._account_FirstName = value; }
            get { return this._account_FirstName; }
        }


        public string Account_LastName
        {
            set { this._account_LastName = value; }
            get { return this._account_LastName; }
        }


        public string Account_Email
        {
            set { this._account_Email = value; }
            get { return this._account_Email; }
        }


        public string Account_Banner_Option
        {
            set { this._account_Banner_Option = value; }
            get { return this._account_Banner_Option; }
        }


        public int? Account_Cart_Option
        {
            set { this._account_Cart_Option = value; }
            get { return this._account_Cart_Option; }
        }



        #endregion	
    }
}
