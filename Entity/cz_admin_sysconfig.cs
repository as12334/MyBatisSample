/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				cz_admin_sysconfig.cs
 *      Description:
 *		
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月09日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 实体类cz_admin_sysconfig
    /// </summary>
    [Serializable]
    public class cz_admin_sysconfig
    {
        #region 私有字段

        private string agent_skin;
        private string hy_skin;


        #endregion

        #region 公有属性


        public string AgentSkin
        {
           get => agent_skin;
           set => agent_skin = value;
        }

        public void set_agent_skin (string agent_skin)
        {
            this.agent_skin = agent_skin;
        }
        public string get_agent_skin()
        {
           return  this.agent_skin;
        }


        public string HySkin
        {
           get => hy_skin;
           set => hy_skin = value;
        }

        public void set_hy_skin (string hy_skin)
        {
            this.hy_skin = hy_skin;
        }
        public string get_hy_skin()
        {
           return  this.hy_skin;
        }



        #endregion	
    }
}
