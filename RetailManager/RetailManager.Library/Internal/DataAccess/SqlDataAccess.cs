using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RetailManager.Library.Models;

namespace RetailManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
        public string GetConnectionString(string name)
        {
            var foo = ConfigurationManager.ConnectionStrings[name];
            return foo.ConnectionString;
        }

        // T and U are generics, making Load/Write reusable by not specifying a type
        // T is the "first" generic, any letter can be used but T is the first, then U i guess?
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string foo = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(foo))
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

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool isClosed = false;

        public void StartTransaction(string connectionStringName) 
        {
            
            _connection = new SqlConnection(GetConnectionString(connectionStringName));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public void WriteDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            return _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
        }

        public void CommitTransaction()
        {
            // if sucessful, saves transaction
            _transaction.Commit();
            _connection.Close();
            isClosed = true;
        }

        public void RollbackTransaction()
        {
            // rollback on fail
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;

        }

        public void Dispose()
        {
            // commit or rollback
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                } catch
                {
                    // TODO - log out
                }
            }
            _transaction = null;
            _connection = null;
        }

        // open connect/start transaction method
        // load using the transaction
        // save using the transaction
        // close connect/stop using the transaction
        // dispose
    }
}
