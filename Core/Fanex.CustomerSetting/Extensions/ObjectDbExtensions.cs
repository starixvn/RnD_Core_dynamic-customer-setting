using Fanex.CustomerSetting.DataAccesses;
using Fanex.CustomerSetting.Models;
using Fanex.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Fanex.CustomerSetting.Extensions
{
    public static class ObjectDbExtensions
    {
        public static IEnumerable<T> QueryOnSiteDB<T>(this ObjectDb objdb, Hashtable parameters, string AGIP)
        {
            var oldConnectionString = objdb.DbSetting.ConnectionString;
            objdb.DbSetting.ConnectionString = BuildConnectionString(AGIP, oldConnectionString);
            objdb.DbConnection.ConnectionString = BuildConnectionString(AGIP, oldConnectionString);
            objdb.AssignParamValues(parameters);
            
            try
            {
                return objdb.Query<T>();
            }
            finally
            {
                objdb.DbSetting.ConnectionString = oldConnectionString;
            }
        }

        public static T QueryEntityOnSiteDB<T>(this ObjectDb objdb, Hashtable parameters, string AGIP)
        {
            var oldConnectionString = objdb.DbSetting.ConnectionString;
            objdb.DbSetting.ConnectionString = BuildConnectionString(AGIP, oldConnectionString);
            objdb.DbConnection.ConnectionString = BuildConnectionString(AGIP, oldConnectionString);
            objdb.AssignParamValues(parameters);
            T result;
            try
            {
                result = objdb.QueryEntity<T>();
            }
            finally
            {
                objdb.DbSetting.ConnectionString = oldConnectionString;
            }

            return result;
        }
        
        private static string BuildConnectionString(string serverIP, string connectionString)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                DataSource = serverIP
            };

            return sqlConnectionStringBuilder.ToString();
        }

        public static SiteDB GetSiteDBConfig(int custId)
        {
            using (var objectDb = ObjectDbFactory.ObjectDb("Acc_SiteDBConfigSel"))
            {
                var parameterValues = new Hashtable
                {
                    { "custid", custId }
                };

                return objectDb.QueryEntity<SiteDB>(parameterValues);
            }
        }
    }
}
