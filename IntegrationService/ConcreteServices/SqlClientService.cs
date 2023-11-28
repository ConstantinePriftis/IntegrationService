using IntegrationService.Data.DbContexts;
using IntegrationService.IServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteServices
{
    public class SqlClientService : ISqlClientService
    {
        private readonly string _connectionString;
        public SqlClientService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int ExecuteProcedure(string procedureName)
        {

            ApplicationDbContext ctx = new ApplicationDbContext();
            return ctx.Database.ExecuteSqlRaw($"EXEC {procedureName}");

            //using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            //{
            //    sqlConnection.Open();
            //    using (SqlCommand command = new SqlCommand($"EXEC {procedureName}", sqlConnection))
            //    {
            //        var result = command.ExecuteNonQuery();
            //        return result.ToString();
            //    }
            //}
        }
    }
}
