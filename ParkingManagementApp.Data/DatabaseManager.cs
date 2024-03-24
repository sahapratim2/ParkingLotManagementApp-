using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using ParkingManagementApp.Common.DataConstants;
using ParkingManagementApp.Common.Utilities;

namespace ParkingManagementApp.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly int _commandTimeout;

        private readonly string _connectionString;

        public DatabaseManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(AppSettingConstants.CONNECTION_STRING_CONSTANTS.DEFAULT_CONNECTION_STR);

            _commandTimeout = Convert.ToInt32(configuration[AppSettingConstants.APPSETTINGS_CONSTANTS.COMMAND_TIMEOUT]);

        }

        public SqlDataReader ExecuteReader(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                                CommandType dbType = CommandType.StoredProcedure)
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            return reader;

        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                               CommandType dbType = CommandType.StoredProcedure)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

            SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            return reader;

        }

        public int ExecuteNonQuery(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                                 CommandType dbType = CommandType.StoredProcedure)
        {
            int returnCount = 0;

            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {

                var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

                if (cmd != null)
                {
                    cmd.Transaction = transaction;

                    returnCount = cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                connection.Close();

                return returnCount;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                connection.Close();

                Console.WriteLine($"An error occurred while Database commiting: {ex.Message}");

                return returnCount;
            }
            finally
            {
                connection.Dispose();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                                CommandType dbType = CommandType.StoredProcedure)
        {

            int returnCount = 0;

            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            var transaction = connection.BeginTransaction();

            try
            {

                var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

                if (cmd != null)
                {
                    cmd.Transaction = transaction;

                    returnCount = await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                await connection.CloseAsync();

                return returnCount;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                await connection.CloseAsync();

                Console.WriteLine($"An error occurred while Database commiting: {ex.Message}");

                return returnCount;
            }
            finally
            {
                connection.Dispose();
            }
        }

        public T ExecuteScalar<T>(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                                CommandType dbType = CommandType.StoredProcedure)
        {
            object dbResult = null;

            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

            if (cmd != null) dbResult = cmd.ExecuteScalar();

            connection.Close();

            if (dbResult == null || DBNull.Value.Equals(dbResult)) return default;

            return Utilities.ConvertValue<T>(dbResult);

        }

        public async Task<T> ExecuteScalarAsync<T>(string dbObjectName, IEnumerable<SqlParameter> paramValues,
                                               CommandType dbType = CommandType.StoredProcedure)
        {
            object dbResult = null;

            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            var cmd = GetCommand(dbObjectName, dbType, connection, paramValues, _commandTimeout);

            if (cmd != null) dbResult = await cmd.ExecuteScalarAsync();

            await connection.CloseAsync();

            if (dbResult == null || DBNull.Value.Equals(dbResult)) return default;

            return Utilities.ConvertValue<T>(dbResult);

        }

        private static SqlCommand GetCommand(string dbObjectName, CommandType dbType, SqlConnection connection, IEnumerable<SqlParameter> paramValues, int commandTimeout)
        {
            var cmd = new SqlCommand(dbObjectName, connection);

            if (cmd == null)

                return null;

            cmd.CommandTimeout = commandTimeout;

            cmd.CommandText = dbObjectName;

            cmd.CommandType = dbType;

            cmd.Connection = connection;

            if (paramValues != null)
            {
                foreach (var val in paramValues)
                {
                    val.Value ??= DBNull.Value;

                    cmd.Parameters.Add(val);
                }
            }

            return cmd;
        }

    }
}