/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				BaseService.cs
 *      Description:
 *				 ���ڷ������ݷ��ʳ������
 *      Author:
 *				֣����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020��02��06��
 *      History:
 *      
 ***********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
namespace Data
{
    using Data.Components;
    using MyBatis.DataMapper.MappedStatements;
    using MyBatis.DataMapper.Scope;
    using MyBatis.DataMapper.Session;
    using MyBatis.DataMapper;
    /// <summary>
    /// ���ڷ������ݷ��ʳ�����࣬��װ�˻������ݷ��ʲ���CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> : IBaseService<T> where T : new()
    {
        #region ˽���ֶ�

        private readonly string procedureName = "MesnacPaging";   //��ҳ�洢������ -for sql2000/2005/2008
        private readonly string pagerProductName = "PagerShow";      //����SQL����ҳ�Ĵ洢������--for sql2005/2008
        protected string tableName = String.Empty;         //��Ӧ���͵ı���
        protected string stmtPrefix = String.Empty;         //MyBatis����ʶǰ׺
        
        #endregion

        #region ���췽��

        public BaseService()
        {
            this.stmtPrefix = typeof(T).Name;
            this.tableName = this.stmtPrefix;
        }
        
        public BaseService(string language)
        {
            this.stmtPrefix = language + typeof(T).Name;
            this.tableName = this.stmtPrefix;
        }

        #endregion

        #region IBaseService<T> ��Ա
        /// <summary>
        /// Type������DBType����ת��
        /// </summary>
        /// <param name="t">Type����</param>
        /// <returns>����DBType</returns>
        public DbType TypeToDbType(Type t)
        {
            DbType dbt;
            try
            {
                dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
            }
            catch
            {
                dbt = DbType.Object;
            }
            return dbt;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="ids">���������б�</param>
        /// <returns></returns>
        public T GetById(int objId)
        {
            string stmtId = "GetById";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ObjId", objId);
            T entity = DbHelper.Instance.DataMapper.QueryForObject<T>(stmtId, parameters);
            return entity;
        }

        public T GetById(Dictionary<string, object> ids)
        {
            string stmtId = "GetById";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            T entity = DbHelper.Instance.DataMapper.QueryForObject<T>(stmtId, ids);
            return entity;
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhere(Dictionary<string, object> where)
        {
            string stmtId = "GetListByWhere";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, where);
            return resultList;
        }
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhere(string where)
        {
            string stmtId = "GetListByWhereStr";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("where", where);
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            return resultList;
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="entity">ָ����ʵ��������Ϊ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByEntity(T entity)
        {
            string stmtId = "GetListByWhere";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, entity);
            return resultList;
        }
        /// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhereAndOrder(Dictionary<string, object> where, string order)
        {
            string stmtId = "GetListByWhereAndOrder";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            where.Add("OrderBy", order);
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, where);
            return resultList;
        }
		/// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhereAndOrder(string where, string order)
        {
            string stmtId = "GetListByWhereAndOrderStr";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("where", where);
            if (!String.IsNullOrEmpty(order))
            {
                parameters.Add("OrderBy", order);
            }
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            return resultList;
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByWhere(Dictionary<string, object> where)
        {
            string stmtId = "GetDataSetByWhere";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            DataTable tableResult = DbHelper.Instance.DataMapper.QueryForDataTable(stmtId, where);
            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(tableResult);
            return dsResult;
        }
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByWhere(string where)
        {
            string stmtId = "GetDataSetByWhereStr";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("where", where);
            DataTable tableResult = DbHelper.Instance.DataMapper.QueryForDataTable(stmtId, parameters);
            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(tableResult);
            return dsResult;
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] parameters)
        {
            returnFields = String.IsNullOrEmpty(returnFields) ? "*" : returnFields;
            Dictionary<string, object> newParameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in parameters)
            {
                newParameters.Add(kv.Key, kv.Value);
            }
            return this.GetDataSetByFieldsAndParams(returnFields, newParameters);
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="parameters">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldsAndParams(string returnFields, Dictionary<string, object> parameters)
        {
            string stmtId = "GetDataSetByFieldsAndParams";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            returnFields = String.IsNullOrEmpty(returnFields) ? "*" : returnFields;
            parameters.Add("ColumnNames", returnFields);
            DataTable tableResult = DbHelper.Instance.DataMapper.QueryForDataTable(stmtId, parameters);
            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(tableResult);
            return dsResult;
        }
        /// <summary>
        /// ��ѯ���м�¼����List��ʽ����
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAllList()
        {
            string stmtId = "GetAllListOrder";      //��GetAllListOrder����ͬһ���
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, null);
            return resultList;
        }
        /// <summary>
        /// ���ұ��еļ�¼������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        public IList<T> GetAllListOrder(string order)
        {
            string stmtId = "GetAllListOrder";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(order))
            {
                parameters.Add("OrderBy", order);
            }
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            return resultList;
        }
        /// <summary>
        /// ����ָ�������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���еļ�¼��</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetTopNListOrder(int n, string order)
        {
            string stmtId = "GetTopNListWhereOrder";        //��GetTopNListWhereOrder����ͬһ���
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Top", n);
            if (!String.IsNullOrEmpty(order))
            {
                parameters.Add("OrderBy", order);
            }
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            return resultList;
        }
        /// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetTopNListWhereOrder(int n, Dictionary<string, object> where, string order)
        {
            string stmtId = "GetTopNListWhereOrder";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            where.Add("Top", n);
            if (!String.IsNullOrEmpty(order))
            {
                where.Add("OrderBy", order);
            }
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, where);
            return resultList;
        }
		/// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetTopNListWhereOrder(int n, string where, string order)
        {
            string stmtId = "GetTopNListWhereOrderStr";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("Top", n);
            parameters.Add("where", where);
            if (!String.IsNullOrEmpty(order))
            {
                parameters.Add("OrderBy", order);
            }
            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            return resultList;
        }
        /// <summary>
        /// ��ѯ���м�¼����DataSet��ʽ��������
        /// </summary>
        /// <returns>��DataSet��ʽ���ر�����������</returns>
        public DataSet GetAllDataSet()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            return this.GetDataSetByWhere(parameters);
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="stmtId">SQL���Id</param>
        /// <param name="values">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByStmt(string stmtId, params KeyValuePair<string,object>[] values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            if (values != null)
            {
                foreach (var kv in values)
                {
                    parameters.Add(kv.Key, kv.Value);
                }
            }
            DataSet dsResult = DbHelper.Instance.DataMapper.QueryForDataSet(stmtId, parameters);
            return dsResult;
        }
        /// <summary>
        /// ��ȡĳ�����ԣ������У������ֵ
        /// </summary>
        /// <param name="propertyItem">���ԣ������У�</param>
        /// <returns>���ش����ԣ������У���Ӧ�����ֵ</returns>
        public object GetMaxValueByProperty(string propertyItem)
        {
            string stmtId = "GetMaxValueByProperty";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ColumnName", propertyItem);
            object result = DbHelper.Instance.DataMapper.QueryForObject(stmtId, parameters);
            return (result as System.Collections.Hashtable)[""];
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageData(PageResult<T> pageResult)
        {
            pageResult.Data.Clear();
            string stmtId = "GetPageDataMesnacPagging";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            if (this.tableName.StartsWith("["))
            {
                parameters.Add("TableName", this.tableName);
            }
            else
            {
                parameters.Add("TableName", String.Format("[{0}]", this.tableName));
            }
            parameters.Add("ReturnFields", pageResult.ReturnFields);
            parameters.Add("PageSize", pageResult.PageSize);
            parameters.Add("PageIndex", pageResult.PageIndex);
            parameters.Add("Where", pageResult.Where);
            parameters.Add("Orderfld", pageResult.Orderfld);
            parameters.Add("OrderType", pageResult.OrderType);

            IList resultList = DbHelper.Instance.DataMapper.QueryForList(stmtId, parameters);
            pageResult.Data = resultList[0] as List<T>;
            pageResult.RecordCount = Convert.ToInt32((resultList[1] as List<Hashtable>)[0]["RecordCount"]);
            return pageResult;
        }
        /// <summary>
        /// ���û���SQL�Ĵ洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����,dataSet,recordCount</returns>
        public PageResult<T> GetPageDataBySql(PageResult<T> pageResult)
        {
            pageResult.Data.Clear();
            string stmtId = "GetPageDataPagerShow";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();

            parameters.Add("QueryStr", pageResult.QueryStr);
            parameters.Add("PageSize", pageResult.PageSize);
            parameters.Add("PageCurrent", pageResult.PageIndex);
            parameters.Add("FdShow", pageResult.ReturnFields);
            parameters.Add("FdOrder", pageResult.Orderfld);
            parameters.Add("Rows", 0);

            IList<T> resultList = DbHelper.Instance.DataMapper.QueryForList<T>(stmtId, parameters);
            pageResult.Data = resultList as List<T>;
            pageResult.RecordCount = Convert.ToInt32(parameters["Rows"]);

            return pageResult;
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            parameters.Add("ReturnFields", pageResult.ReturnFields);
            parameters.Add("PageSize", pageResult.PageSize);
            parameters.Add("PageIndex", pageResult.PageIndex);
            parameters.Add("Where", pageResult.Where);
            parameters.Add("Orderfld", pageResult.Orderfld);
            parameters.Add("OrderType", pageResult.OrderType);

            using (IDataReader reader = DbHelper.Instance.DataMapper.QueryForDataReader(stmtId, parameters))
            {
                int begin = pageResult.PageSize * (pageResult.PageIndex - 1) + 1;
                int count = 0;
                int pageSize = 0;
                DataTable table = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataColumn col = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    table.Columns.Add(col);
                }
                while (reader.Read())
                {
                    count++;
                    if (count >= begin && pageSize < pageResult.PageSize)
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                        pageSize++;
                    }
                }
                reader.Close();
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                pageResult.DataSet = ds;
                pageResult.RecordCount = count;
            }
            return pageResult;
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(PageResult<T> pageResult)
        {
            string stmtId = "GetPageDataByReader";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            return this.GetPageDataByReader(stmtId, pageResult);
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <param name="values">����</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult, params KeyValuePair<string, object>[] values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            if (values != null)
            {
                foreach (var kv in values)
                {
                    parameters.Add(kv.Key, kv.Value);
                }
            }

            using (IDataReader reader = DbHelper.Instance.DataMapper.QueryForDataReader(stmtId, parameters))
            {
                int begin = pageResult.PageSize * (pageResult.PageIndex - 1) + 1;
                int count = 0;
                int pageSize = 0;
                DataTable table = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataColumn col = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    table.Columns.Add(col);
                }
                while (reader.Read())
                {
                    count++;
                    if (count >= begin && pageSize < pageResult.PageSize)
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                        pageSize++;
                    }
                }
                reader.Close();
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                pageResult.DataSet = ds;
                pageResult.RecordCount = count;
            }
            return pageResult;
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <param name="parameters">����</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult, Dictionary<string, object>[] parameters)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }

            using (IDataReader reader = DbHelper.Instance.DataMapper.QueryForDataReader(stmtId, parameters))
            {
                int begin = pageResult.PageSize * (pageResult.PageIndex - 1) + 1;
                int count = 0;
                int pageSize = 0;
                DataTable table = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataColumn col = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    table.Columns.Add(col);
                }
                while (reader.Read())
                {
                    count++;
                    if (count >= begin && pageSize < pageResult.PageSize)
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                        pageSize++;
                    }
                }
                reader.Close();
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                pageResult.DataSet = ds;
                pageResult.RecordCount = count;
            }
            return pageResult;
        }
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ���ҳ�����ܼ�¼���Ľ���������ݼ�</returns>
        public DataSet GetPageDataSet(PageResult<T> pageResult)
        {
            pageResult.Data.Clear();
            string stmtId = "GetPageDataSet";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            if (this.tableName.StartsWith("["))
            {
                parameters.Add("TableName", this.tableName);
            }
            else
            {
                parameters.Add("TableName", String.Format("[{0}]", this.tableName));
            }
            parameters.Add("ReturnFields", pageResult.ReturnFields);
            parameters.Add("PageSize", pageResult.PageSize);
            parameters.Add("PageIndex", pageResult.PageIndex);
            parameters.Add("Where", pageResult.Where);
            parameters.Add("Orderfld", pageResult.Orderfld);
            parameters.Add("OrderType", pageResult.OrderType);

            DataSet ds = DbHelper.Instance.DataMapper.QueryForDataSet(stmtId, parameters);
            return ds;
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���ֵ</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string stmtId, params KeyValuePair<string, object>[] values)
        {
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Hashtable parameters = new Hashtable();
            foreach (KeyValuePair<string, object> kv in values)
            {
                parameters.Add(kv.Key, kv.Value);
            }
            DataSet ds = DbHelper.Instance.DataMapper.QueryForDataSet(stmtId, parameters);
            return ds;
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string stmtId, Dictionary<string, object> values)
        {
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            DataSet ds = DbHelper.Instance.DataMapper.QueryForDataSet(stmtId, values);
            return ds;
        }
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="stmtId">SQL������ID</param>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCount(string stmtId , params KeyValuePair<string,object>[] values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            Hashtable parameters = new Hashtable();
            foreach (KeyValuePair<string, object> kv in values)
            {
                parameters.Add(kv.Key, kv.Value);
            }
            int result = DbHelper.Instance.DataMapper.QueryForObject<int>(stmtId, parameters);
            return result;
        }
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCount(params KeyValuePair<string, object>[] values)
        {
            string stmtId = "GetRowCount";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            return this.GetRowCount(stmtId, values);
        }
		/// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCountByWhere(string where)
        {
            string stmtId = "GetRowCountByWhere";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            int result = DbHelper.Instance.DataMapper.QueryForObject<int>(stmtId, where);
            return result;
        }
        /// <summary>
        /// ����¼�¼
        /// </summary>
        /// <param name="entity">��Ӧ�¼�¼��ʵ������</param>
        /// <returns>����׷�Ӽ�¼������ֵ</returns>
        public int Insert(T entity)
        {
            string stmtId = "Insert";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            object result = DbHelper.Instance.DataMapper.Insert(stmtId, entity);
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// ��������¼�¼
        /// </summary>
        /// <param name="lst">��Ӧ��List��¼</param>
        /// <returns>������Ӱ��ļ�¼����</returns>
        public int BatchInsert(List<T> lst)
        {
            string stmtId = "Insert";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            foreach (T entity in lst)
            {
                object result = DbHelper.Instance.DataMapper.Insert(stmtId, entity);
            }
            return Convert.ToInt32(lst.Count);
        }
        /// <summary>
        /// ���¼�¼
        /// </summary>
        /// <param name="entity">��Ҫ���¼�¼��Ӧ��ʵ������</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int Update(T entity)
        {
            string stmtId = "Update";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            object result = DbHelper.Instance.DataMapper.Update(stmtId, entity);
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateByStmt(string stmtId, params KeyValuePair<string, object>[] values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            Hashtable parameters = new Hashtable();
            foreach (KeyValuePair<string, object> kv in values)
            {
                parameters.Add(kv.Key, kv.Value);
            }
            int result = DbHelper.Instance.DataMapper.Update(stmtId, parameters);
            return result;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateByStmt(string stmtId, Dictionary<string, object> values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            int result = DbHelper.Instance.DataMapper.Update(stmtId, values);
            return result;
        }
		/// <summary>
        /// ����������
        /// </summary>
        /// <param name="fieldSetValue">�ֶμ����µ�ֵ�����Զ��</param>
        /// <param name="where">�������ʽ</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateFields(string fieldSetValue, string where)
        {
            string stmtId = "UpdateFields";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("FieldSetValue", fieldSetValue);
            parameters.Add("where", where);
            int result = DbHelper.Instance.DataMapper.Update(stmtId, parameters);
            return result;
        }
        /// <summary>
        /// ɾ��������idֵ�ü�¼
        /// </summary>
        /// <param name="id">Ҫɾ����¼������ֵ</param>
        /// <returns>����ɾ���ļ�¼����</returns>
        public int Delete(object id)
        {
            string stmtId = "Delete";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            int result = DbHelper.Instance.DataMapper.Delete(stmtId, id);
            return result;
        }
        /// <summary>
        /// ɾ�������Ӧ�ļ�¼
        /// </summary>
        /// <param name="entity">��Ҫɾ����¼��Ӧ�Ķ���</param>
        public int Delete(T entity)
        {
            string stmtId = "DeleteByEntity";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            int result = DbHelper.Instance.DataMapper.Delete(stmtId, entity);
            return result;
        }
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByStmt(string stmtId, params KeyValuePair<string, object>[] values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            Hashtable parameters = new Hashtable();
            foreach (KeyValuePair<string, object> kv in values)
            {
                parameters.Add(kv.Key, kv.Value);
            }
            int result  = DbHelper.Instance.DataMapper.Delete(stmtId, parameters);
            return result;
        }
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByStmt(string stmtId, Dictionary<string, object> values)
        {
            if (stmtId.IndexOf(".") == -1)
            {
                stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            }
            int result = DbHelper.Instance.DataMapper.Delete(stmtId, values);
            return result;
        }
		/// <summary>
        /// ɾ��ָ�������ļ�¼
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByWhere(string where)
        {
            string stmtId = "DeleteByWhere";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            int result = DbHelper.Instance.DataMapper.Delete(stmtId, where);
            return result;
        }
        /// <summary>
        /// ����������м�¼
        /// </summary>
        public void ClearData()
        {
            string stmtId = "ClearData";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            DbHelper.Instance.DataMapper.QueryForObject(stmtId, null);
        }

        #endregion
        
        #region ��������

        /// <summary>
        /// �õ�����ʱibatis.net��̬���ɵ�SQL
        /// </summary>
        /// <param name="sqlMapper"></param>
        /// <param name="statementName"></param>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        protected virtual string GetRuntimeSql(string statementName, object paramObject)
        {
            string result = string.Empty;
            try
            {
                IMappedStatement statement = DbHelper.Instance.Engine.ModelStore.GetMappedStatement(statementName);
                if (null == DbHelper.Instance.Engine.ModelStore.SessionStore.CurrentSession)
                {
                    DbHelper.Instance.Engine.ModelStore.SessionFactory.OpenSession();
                }
                Console.WriteLine(DbHelper.Instance.Engine.ModelStore.SessionStore.CurrentSession.Connection.State);
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, DbHelper.Instance.Engine.ModelStore.SessionStore.CurrentSession);
                result = scope.PreparedStatement.PreparedSql;

                Console.WriteLine(result);

                foreach (IDbDataParameter para in scope.PreparedStatement.DbParameters)
                {
                    Console.WriteLine("{0} = {1}", para.ParameterName, para.Value);
                }
            }
            catch (Exception ex)
            {
                result = "��ȡSQL�������쳣:" + ex.Message;
                Console.Write(result);
            }
            return result;
        }

        #endregion
    }
}
