/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				PageResult.cs
 *      Description:
 *				 ��ҳ��
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
using System.Data;

namespace Data.Components
{
    /// <summary>
    /// ��ҳ���װ��ҳ��Ϣ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageResult<T> where T : new()
    {
        #region ˽���ֶ�

        private string queryStr = String.Empty;     //��ѯ���
        private string tableName = String.Empty;  //����
        private string returnFields = "*";          //�����ֶ��б�
        private string where = String.Empty;        //��������
        private int pageIndex = 1;                  //��ǰҳ������
        private int pageSize = 10;                  //ÿҳ��¼��
        private int recordCount = 0;                //�ܼ�¼��
        private string orderfld = String.Empty;             //�����ֶ�
        private int orderType = 0;                  //����ʽ,1Ϊ��������Ϊ����
        private List<T> data = new List<T>();
        private DataSet dataSet = new DataSet();

        private T obj = new T();

        #endregion

        #region ���췽��

        public PageResult()
        {
            this.orderfld = "getDate()";
            DataTable dt = new DataTable();
            this.dataSet.Tables.Add(dt);
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ѯ���
        /// </summary>
        public string QueryStr
        {
            get { return queryStr; }
            set { queryStr = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get 
            {
                return this.tableName; 
            }
            set { tableName = value; }
        }
        /// <summary>
        /// ���ؽ���������ֶ��б��м��ö��ŷָ���Ĭ��Ϊ*
        /// </summary>
        public string ReturnFields
        {
            get { return returnFields; }
            set { returnFields = value; }
        }
        /// <summary>
        /// ��ҳ������������Ĭ��Ϊ���ַ���
        /// </summary>
        public string Where
        {
            get { return where; }
            set { where = value; }
        }
        /// <summary>
        /// ��ǰҳ��������Ĭ��Ϊ1,�����һҳ
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        /// <summary>
        /// ÿҳ�ļ�¼����Ĭ��10
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// ������Χ���ܼ�¼��
        /// </summary>
        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.recordCount % this.PageSize == 0 ? this.recordCount / this.pageSize : this.recordCount / this.PageSize + 1;
            }
        }
        /// <summary>
        /// ��ҳ��ѯʱ�������ֶΣ����������
        /// </summary>
        public string Orderfld
        {
            get {
                return orderfld; }
            set { orderfld = value; }
        }
        /// <summary>
        /// �������ͣ�����ʽ,1Ϊ��������Ϊ����
        /// </summary>
        public int OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }
        /// <summary>
        /// ��Ӧ��ǰҳ������
        /// </summary>
        public List<T> Data
        {
            get { return data; }
            set { data = value; }
        }
        /// <summary>
        /// ��Ӧ��ǰҳ������
        /// </summary>
        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        #endregion
    }
}
