using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace RetailManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        // T and U are generics, making Load/Write reusable by not specifying a type
        // T is the "first" generic, any letter can be used but T is the first, then U i guess?
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public void WriteData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(connectionStringName)))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
