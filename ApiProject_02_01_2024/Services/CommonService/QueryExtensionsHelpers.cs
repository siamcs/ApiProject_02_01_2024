using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
namespace ApiProject_02_01_2024.Services.CommonService
{
    public static class QueryExtensionsHelpers
    {
        public static T QuerySingle<T>(string connectionString, string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                return conn.QueryFirstOrDefault<T>(sql, param, null, 0, commandType: commandType);

            }
        }

        public static IEnumerable<T> Query<T>(string connectionString, string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<T>(sql, param, null, true, 0, commandType: commandType);
            }
        }
    }
}
