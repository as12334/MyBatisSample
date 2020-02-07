/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseService.cs
 *      Description:
 *				 ���ݷ��ʻ����ӿ�
 *      Author:
 *				֣����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020��02��06��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Data
{
    using Data.Components;
    public interface IBaseService<T> where T : new()
    {
        /// <summary>
        /// Type������DBType����ת��
        /// </summary>
        /// <param name="t">Type����</param>
        /// <returns>����DBType</returns>
        DbType TypeToDbType(Type t);
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="ids">���������б�</param>
        /// <returns>����������Ӧ�Ķ���</returns>
        T GetById(int objId);
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="ids">���������б�</param>
        /// <returns>������������Ķ���</returns>
        T GetById(Dictionary<string, object> ids);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetListByWhere(Dictionary<string,object> where);
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetListByWhere(string where);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="entity">ָ����ʵ��������Ϊ����</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetListByEntity(T entity);
        /// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetListByWhereAndOrder(Dictionary<string, object> where, string order);
		/// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetListByWhereAndOrder(string where, string order);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByWhere(Dictionary<string, object> where);
		/// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByWhere(string where);
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns>���ط��ϲ�ѯ����ָ���ֶ��б�Ľ����</returns>
        DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] parameters);
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="parameters">��ѯ�����б�</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByFieldsAndParams(string returnFields, Dictionary<string, object> parameters);
        /// <summary>
        /// ��ѯ���м�¼����List��ʽ����
        /// </summary>
        /// <returns></returns>
        IList<T> GetAllList();
        /// <summary>
        /// ���ұ��еļ�¼������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        IList<T> GetAllListOrder(string order);
        /// <summary>
        /// ����ָ�������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���еļ�¼��</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetTopNListOrder(int n, string order);
        /// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetTopNListWhereOrder(int n, Dictionary<string, object> where, string order);
		/// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        IList<T> GetTopNListWhereOrder(int n, string where, string order);
        /// <summary>
        /// ��ѯ���м�¼����DataSet��ʽ��������
        /// </summary>
        /// <returns></returns>
        DataSet GetAllDataSet();
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="stmtId">SQL���Id</param>
        /// <param name="values">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByStmt(string stmtId, params KeyValuePair<string,object>[] values);
        /// <summary>
        /// ��ȡĳ�����ԣ������У������ֵ
        /// </summary>
        /// <param name="propertyItem">���ԣ������У�</param>
        /// <returns>���ش����ԣ������У���Ӧ�����ֵ</returns>
        object GetMaxValueByProperty(string propertyItem);
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageData(PageResult<T> pageResult);
		/// <summary>
        /// ���û���SQL�Ĵ洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����,dataSet,recordCount</returns>
        PageResult<T> GetPageDataBySql(PageResult<T> pageResult);
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult);
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageDataByReader(PageResult<T> pageResult);
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <param name="values">����</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ���ô洢���̵ķ�ҳ��ѯ����
        /// </summary>
        /// <param name="stmtId">SQL������Id</param>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <param name="parameters">����</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageDataByReader(string stmtId, PageResult<T> pageResult, Dictionary<string, object>[] parameters);
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ���ҳ�����ܼ�¼���Ľ���������ݼ�</returns>
        DataSet GetPageDataSet(PageResult<T> pageResult);
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���ֵ</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        DataSet GetDataSetByStoreProcedure(string stmtId, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="stmtId">�洢���̵�������Id</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        DataSet GetDataSetByStoreProcedure(string stmtId, Dictionary<string, object> values);
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="stmtId">SQL������ID</param>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        int GetRowCount(string stmtId, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="values">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        int GetRowCount(params KeyValuePair<string, object>[] values);
		/// <summary>
        /// ��ȡ���������ļ�¼��
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>���ط��������ļ�¼��</returns>
        int GetRowCountByWhere(string where);
        /// <summary>
        /// ����¼�¼
        /// </summary>
        /// <param name="entity">��Ӧ�¼�¼��ʵ������</param>
        /// <returns>����׷�Ӽ�¼������ֵ</returns>
        int Insert(T entity);
        /// <summary>
        /// ��������¼�¼
        /// </summary>
        /// <param name="lst">��Ӧ��List��¼</param>
        /// <returns>������Ӱ��ļ�¼����</returns>
        int BatchInsert(List<T> lst);
        /// <summary>
        /// ���¼�¼
        /// </summary>
        /// <param name="entity">��Ҫ���¼�¼��Ӧ��ʵ������</param>
        /// <returns>���ظ��µļ�¼��</returns>
        int Update(T entity);
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        int UpdateByStmt(string stmtId, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="stmtId">����������Id</param>
        /// <param name="values">����</param>
        /// <returns>���ظ��µļ�¼��</returns>
        int UpdateByStmt(string stmtId, Dictionary<string, object> values);
		/// <summary>
        /// ����������
        /// </summary>
        /// <param name="fieldSetValue">�ֶμ����µ�ֵ�����Զ��</param>
        /// <param name="where">�������ʽ</param>
        /// <returns>���ظ��µļ�¼��</returns>
        int UpdateFields(string fieldSetValue, string where);
        /// <summary>
        /// ɾ��������idֵ�ü�¼
        /// </summary>
        /// <param name="id">Ҫɾ����¼������ֵ</param>
        /// <returns>����ɾ���ļ�¼����</returns>
        int Delete(object id);
        /// <summary>
        /// ɾ�������Ӧ�ļ�¼
        /// </summary>
        /// <param name="entity">��Ҫɾ����¼��Ӧ�Ķ���</param>
        int Delete(T entity);
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        int DeleteByStmt(string stmtId, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="stmtId">ɾ��������Id</param>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        int DeleteByStmt(string stmtId, Dictionary<string, object> values);
		/// <summary>
        /// ɾ��ָ�������ļ�¼
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        int DeleteByWhere(string where);
        /// <summary>
        /// ����������м�¼
        /// </summary>
        void ClearData();
    }
}
