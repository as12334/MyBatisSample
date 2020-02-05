/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseManager.cs
 *      Description:
 *				 业务逻辑基础接口
 *      Author:
 *				郑立兵
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2020年02月05日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    using Data;
    public interface IBaseManager<T> : IBaseService<T> where T : new()
    {

    }
}
