using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using QueryBuilder.Config;

namespace QueryBuilder.DatabaseConnections
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private DbConnection dbConnection;
        private IDbCommand dbCommand;


        public DatabaseConnection(DbConnection dbConnection, IDbCommand dbCommand = null)
        {
            this.dbConnection = dbConnection;
            this.dbCommand = dbCommand;
        }

        public DataTable query(string sql)
        {
            try
            {
                dbConnection.Open();

                dbCommand.CommandText = sql;
                dbCommand.Connection = dbConnection;

                var dataReader = dbCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void userSignIn()
        {
            try
            {
                dbConnection.Open();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public DataTable getSchema(string tableName)
        {
            try
            {
                dbConnection.Open();

                return dbConnection.GetSchema("Columns", new string[4] { null, null, tableName, null });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }

}
