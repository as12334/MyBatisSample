/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				DBHelper.cs
 *      Description:
 *				 SQL���ݷ��ʸ�����
 *      Author:
 *				֣����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020��02��05��
 *      History:
 *      
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using MyBatis.DataMapper;
using MyBatis.DataMapper.Configuration;
using MyBatis.DataMapper.Configuration.Interpreters.Config.Xml;
using MyBatis.DataMapper.Session;
using MyBatis.DataMapper.Session.Stores;
namespace Data
{
    public delegate string KeyConvert(string key);

    /// <summary>
    /// ���ݷ��ʸ�����
    /// </summary>
    public class DbHelper : ScriptBase
    {
        #region ����ʵ��

        private static DbHelper _instance = null;
        private IConfigurationEngine _engine = null;
        private ISessionFactory _sessionFactory = null;
        private ISessionStore _sessionStore = null;
        private IDataMapper _dataMapper = null;
        private KeyConvert ConvertKey = null;

        private DbHelper()
        {
            string resource = "SqlMap.config";
            try
            {
                ConfigurationSetting configurationSetting = new ConfigurationSetting();
				configurationSetting.SessionStore = new HybridWebThreadSessionStore("test");        //�����Web�����п��̷߳���HttpContext������
                configurationSetting.Properties.Add("nullableInt", "int?");

                _engine = new DefaultConfigurationEngine(configurationSetting);
                _engine.RegisterInterpreter(new XmlConfigurationInterpreter(resource));

                IMapperFactory mapperFactory = _engine.BuildMapperFactory();
                _sessionFactory = _engine.ModelStore.SessionFactory;

                _dataMapper = ((IDataMapperAccessor)mapperFactory).DataMapper;
                _sessionStore = ((IModelStoreAccessor)_dataMapper).ModelStore.SessionStore;
                
            }
            catch (Exception ex)
            {
                Exception e = ex;
                while (e != null)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    e = e.InnerException;
                }
                throw;
            }

            if (_sessionFactory.DataSource.DbProvider.Id.IndexOf("PostgreSql") >= 0)
            {
                ConvertKey = new KeyConvert(Lower);
            }
            else if (_sessionFactory.DataSource.DbProvider.Id.IndexOf("oracle") >= 0)
            {
                ConvertKey = new KeyConvert(Upper);
            }
            else
            {
                ConvertKey = new KeyConvert(Normal);
            }
        }

        public static DbHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(DbHelper))
                    {
                        if (_instance == null)        //double-check
                        {
                            _instance = new DbHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// ����ӳ����
        /// </summary>
        public IDataMapper DataMapper
        {
            get
            {
                return this._dataMapper;
            }
        }

        /// <summary>
        /// �Ự����
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get
            {
                return this._sessionFactory;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public IConfigurationEngine Engine
        {
            get
            {
                return this._engine;
            }
        }

        #region ��������

        private static string Normal(string key)
        {
            return key;
        }

        private static string Upper(string key)
        {
            return key.ToUpper();
        }

        private static string Lower(string key)
        {
            return key.ToLower();
        }

        #endregion
    }
}
