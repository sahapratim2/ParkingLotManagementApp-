using System.Data.SqlClient;
using System.Data;

namespace ParkingManagementApp.Data
{
    public interface IDatabaseManager
    {
        public SqlDataReader ExecuteReader(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                              CommandType dbType = CommandType.StoredProcedure);

        public Task<SqlDataReader> ExecuteReaderAsync(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                              CommandType dbType = CommandType.StoredProcedure);

        public int ExecuteNonQuery(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                                CommandType dbType = CommandType.StoredProcedure);

        public Task<int> ExecuteNonQueryAsync(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                               CommandType dbType = CommandType.StoredProcedure);

        public T ExecuteScalar<T>(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                              CommandType dbType = CommandType.StoredProcedure);

        public Task<T> ExecuteScalarAsync<T>(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                          CommandType dbType = CommandType.StoredProcedure);

        }
}
