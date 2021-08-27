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

        public void StartTransaction(string connectionStringName) 
        {
            
            _connection = new SqlConnection(GetConnectionString(connectionStringName));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void WriteDataInTransaction<T>(string storedProcedure, T parameters)
        {
            //var dataTwo = _connection.Execute("INSERT into dbo.Product (Name, Description, RetailPrice, QuantityInStock, IsTaxable) VALUES('p5', 'd5', 500, 5, 1)", transaction: _transaction);
            //var foo = parameters.SaleDate.GetType();
            //var dataTwo = _connection.Execute($"INSERT into dbo.Sale (UserId, SaleDate, SubTotal, Tax, Total) VALUES('eaa2bb2f-f260-4536-9910-7b631492165f', {parameters.SaleDate}, 300, 20, 320)", transaction: _transaction);

            //// WHY DOESN'T THIS WORK!!!
            var data = _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
            return;
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
            return rows;
        }

        public void CommitTransaction()
        {
            // if sucessful, saves transaction
            _transaction.Commit();
            _connection.Close();
        }

        public void RollbackTransaction()
        {
            // rollback on fail
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            // commit or rollback
            CommitTransaction();
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
