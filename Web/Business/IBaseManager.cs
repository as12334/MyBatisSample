/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseManager.cs
 *      Description:
 *				 ҵ���߼������ӿ�
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

namespace Business
{
    using Data;
    public interface IBaseManager<T> : IBaseService<T> where T : new()
    {

    }
}
