using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccessEF.Extensions
{
    public static class CustomExtensions
    {

        public static long GetNextSequenceValue(this PersonDbContext context, string name, string schema = null)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            context.Database.ExecuteSqlRaw($"SELECT @result = (NEXT VALUE FOR [{name}])", result);

            return (long)result.Value;
        }
    }

}
