/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseLottery.cs
 *      Description:
 *				 ҵ���߼������ӿ�
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

namespace Business
{
    using Data;
    public interface IBaseLottery<T> : IBaseService<T> where T : new()
    {

    }
}
