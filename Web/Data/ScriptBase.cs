/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				ScriptBase.cs
 *      Description:
 *				 ִ��SQL�ű��Ļ���������
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
using System.Linq;
using System.Text;
using System.IO;
using MyBatis.Common.Resources;
using MyBatis.Common.Data;
using MyBatis.Common.Utilities;

namespace Data
{
    /// <summary>
    /// ִ��SQL�ű��Ļ���������
    /// </summary>
    public abstract class ScriptBase
    {
        //ָ��SQL�ű�Ŀ¼
        protected string scriptDirectory = Path.Combine(Path.Combine(Path.Combine(Resources.ApplicationBase, ".."), ".."), "Scripts") + Path.DirectorySeparatorChar;

        /// <summary>
        /// ����SQL������
        /// </summary>
        /// <param name="datasource">����SQL������ʱ��Ӧ������Դ</param>
        /// <param name="script">SQL������ű�</param>
        public void InitScript(IDataSource datasource, string script)
        {
            InitScript(datasource, script, true);
        }

        /// <summary>
        /// ����SQL������
        /// </summary>
        /// <param name="datasource">����SQL������ʱ��Ӧ������Դ</param>
        /// <param name="script">SQL������ű�</param>
        /// <param name="doParse">�Ƿ��SQL�ű��ļ��н��������</param>
        private void InitScript(IDataSource datasource, string script, bool doParse)
        {
            ScriptRunner runner = new ScriptRunner();
            runner.RunScript(datasource, script, doParse);
        }
    }
}
