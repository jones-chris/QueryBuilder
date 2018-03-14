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

        public DataTable Query(string sql)
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

        public void UserSignIn()
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

        public DataTable GetDatabaseSchemas()
        {
            try
            {
                dbConnection.Open();

                return dbConnection.GetSchema("SCHEMATA");
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

        public DataTable GetColumns(string tableName)
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

        public DataTable GetSchemaTables(string schemaName)
        {
            try
            {
                dbConnection.Open();

                return dbConnection.GetSchema("Columns", new string[4] { null, null, schemaName, null });
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

        public DataTable GetSchemaViews(string schemaName)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTablesAndViews(string schemaName)
        {
            throw new NotImplementedException();
        }

        public DataTable GetUserTablesAndViews(string username)
        {
            throw new NotImplementedException();
        }
    }

}
