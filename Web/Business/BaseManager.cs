/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				BaseLottery.cs
 *      Description:
 *				 ҵ���߼��������
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
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Business
{
    using Data.Components;
    using Data;
    public abstract class BaseLottery<T> : IBaseLottery<T> where T : new()
    {
        private IBaseService<T> baseService;

        public IBaseService<T> BaseService
        {
            set { baseService = value; }
        }

        #region ��װ���ݷ��ʲ�ĳ������ݷ��ʷ���
		/// <summary>
        /// Type������DBType����ת��
        /// </summary>
        /// <param name="t">Type����</param>
        /// <returns>����DBType</returns>
        public DbType TypeToDbType(Type t)
        {
            return this.baseService.TypeToDbType(t);
        }
        /// <summary>
        /// ��ס�����ʶ�в��ң�ֻ���ǵ��ֶ�����������ϼ���ʱ�Ű���������
        /// </summary>
        /// <param name="ida">��Ӧ���Ҽ�¼������ֵ���ʶֵ</param>
        /// <returns>���ض�Ӧ��¼��ʵ����Ϣ</returns>
        public T GetById(int objId)
        {
            return this.baseService.GetById(objId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public T GetById(Dictionary<string, object> ids)
        {
            return this.baseService.GetById(ids);
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhere(Dictionary<string,object> where)
        {
            return this.baseService.GetListByWhere(where);
        }
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhere(string where)
        {
            return this.baseService.GetListByWhere(where);
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="entity">ָ����ʵ��������Ϊ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByEntity(T entity)
        {
			return this.baseService.GetListByEntity(entity);
        }
        /// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhereAndOrder(Dictionary<string, object> where, string order)
        {
            return this.baseService.GetListByWhereAndOrder(where, order);
        }
		/// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetListByWhereAndOrder(string where, string order)
        {
            return this.baseService.GetListByWhereAndOrder(where, order);
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByWhere(Dictionary<string, object> where)
        {
            return this.baseService.GetDataSetByWhere(where);
        }
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByWhere(string where)
        {
            return this.baseService.GetDataSetByWhere(where);
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="parameters">��ѯ�����б�KeyValuePair��Key���ֶ���,KeyValuePair��Value���ֶ�ֵ</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] parameters)
        {
            return this.baseService.GetDataSetByFieldsAndParams(returnFields, parameters);
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="parameters">��ѯ�����б�</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldsAndParams(string returnFields, Dictionary<string, object> parameters)
        {
            return this.baseService.GetDataSetByFieldsAndParams(returnFields, parameters);
        }
        /// <summary>
        /// ���ұ������м�¼
        /// </summary>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        public IList<T> GetAllList()
        {
            return this.baseService.GetAllList();
        }
        /// <summary>
        /// ���ұ��еļ�¼������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        public IList<T> GetAllListOrder(string order)
        {
            return this.baseService.GetAllListOrder(order);
        }
        /// <summary>
        /// ����ָ�������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public IList<T> GetTopNListOrder(int n, string order)
        {
            return this.baseService.GetTopNListOrder(n, order);
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
            return this.baseService.GetTopNListWhereOrder(n, where, order);
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
            return this.baseService.GetTopNListWhereOrder(n, where, order);
        }
        /// <summary>
        /// ���ұ������м�¼
        /// </summary>
        /// <returns>���ض�Ӧ�����ݼ�</returns>
        public DataSet GetAllDataSet()
        {
            return this.baseService.GetAllDataSet();
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="stmtId">SQL���Id</param>
        /// <param name="values">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByStmt(string stmtId, params KeyValuePair<string,object>[] values)
        {
			return this.baseService.GetDataSetByStmt(stmtId, values);
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageData(PageResult<T> pageResult)
        {
            return this.baseService.GetPageData(pageResult);
        }
        /// <summary>
        /// ��ȡĳ�����ԣ������У������ֵ
        /// </summary>
        /// <param name="propertyItem">���ԣ������У�</param>
        /// <returns>���ش����ԣ������У���Ӧ�����ֵ</returns>
        public object GetMaxValueByProperty(string propertyItem)
        {
            return this.baseService.GetMaxValueByProperty(propertyItem);
        }
		/// <summary>
        /// ���û���SQL�Ĵ洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����,dataSet,recordCount</returns>
        public PageResult<T> GetPageDataBySql(PageResult<T> pageResult)
        {
            return this.baseService.GetPageDataBySql(pageResult);
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult)
        {
            return this.baseService.GetPageDataByReader(stmtId, pageResult);
        }
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageDataByReader(PageResult<T> pageResult)
        {
            return this.baseService.GetPageDataByReader(pageResult);
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
			return this.baseService.GetPageDataByReader(stmtId, pageResult, values);
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
			return this.baseService.GetPageDataByReader(stmtId, pageResult, parameters);
        }
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ���ҳ�����ܼ�¼���Ľ���������ݼ�</returns>
        public DataSet GetPageDataSet(PageResult<T> pageResult)
        {
            return this.baseService.GetPageDataSet(pageResult);
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���ֵ</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string stmtId, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetDataSetByStoreProcedure(stmtId, values);
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string stmtId, Dictionary<string, object> values)
        {
            return this.baseService.GetDataSetByStoreProcedure(stmtId, values);
        }
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="stmtId">SQL������ID</param>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCount(string stmtId, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetRowCount(stmtId, values);
        }
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCount(params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetRowCount(values);
        }
		/// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        public int GetRowCountByWhere(string where)
        {
            return this.baseService.GetRowCountByWhere(where);
        }
        /// <summary>
        /// �����׷��һ����¼
        /// </summary>
        /// <param name="entity">��װ��¼��ʵ��</param>
        /// <returns>����������У��򷵻ض�Ӧ�������е�ֵ�����򷵻���Ӱ�������</returns>
        public int Insert(T entity)
        {
            return this.baseService.Insert(entity);
        }
        /// <summary>
        /// ���������׷�Ӽ�¼
        /// </summary>
        /// <param name="lst">��װ��¼��List����</param>
        /// <returns>������Ӱ�������</returns>
        public int BatchInsert(List<T> lst)
        {
            return this.baseService.BatchInsert(lst);
        }
        /// <summary>
        /// ���±��е�һ����¼
        /// </summary>
        /// <param name="entity">��װ��¼��ʵ��</param>
        /// <returns>������Ӱ�������</returns>
        public int Update(T entity)
        {
            return this.baseService.Update(entity);
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateByStmt(string stmtId, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.UpdateByStmt(stmtId, values);
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateByStmt(string stmtId, Dictionary<string, object> values)
        {
            return this.baseService.UpdateByStmt(stmtId, values);
        }
		/// <summary>
        /// ����������
        /// </summary>
        /// <param name="fieldSetValue">�ֶμ����µ�ֵ�����Զ��</param>
        /// <param name="where">�������ʽ</param>
        /// <returns>���ظ��µļ�¼��</returns>
        public int UpdateFields(string fieldSetValue, string where)
        {
            return this.baseService.UpdateFields(fieldSetValue, where);
        }
        /// <summary>
        /// ɾ�����е�һ����¼
        /// </summary>
        /// <param name="id">Ҫɾ����¼������ֵ�ͱ�ʶֵ</param>
        /// <returns>������Ӱ�������</returns>
        public int Delete(object id)
        {
            return this.baseService.Delete(id);
        }
        /// <summary>
        /// ɾ�������Ӧ�ļ�¼
        /// </summary>
        /// <param name="entity">��Ҫɾ����¼��Ӧ�Ķ���</param>
        public int Delete(T entity)
        {
            return this.baseService.Delete(entity);
        }
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByStmt(string stmtId, KeyValuePair<string, object>[] values)
        {
            return this.baseService.DeleteByStmt(stmtId, values);
        }
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByStmt(string stmtId, Dictionary<string, object> values)
        {
            return this.baseService.DeleteByStmt(stmtId, values);
        }
		/// <summary>
        /// ɾ��ָ�������ļ�¼
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByWhere(string where)
        {
            return this.baseService.DeleteByWhere(where);
        }
        /// <summary>
        /// ��ձ��е�����
        /// </summary>
        /// <returns>��������ļ�¼��</returns>
        public void ClearData()
        {
            this.baseService.ClearData();
        }
        #endregion
    }
}
