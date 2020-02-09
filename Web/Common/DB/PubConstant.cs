using System;
using System.Configuration;
using Data;

namespace BuilderDALSQL
{
	public class PubConstant
	{
		public static string DBConnectionString
		{
			get
			{
				var dataSourceConnectionString = DbHelper.Instance.SessionFactory.DataSource.ConnectionString;
				return dataSourceConnectionString;
			}
		}
	}
}
