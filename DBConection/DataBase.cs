using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DBConection
{
    public class DataBase
    {
        private readonly string _connectionString = @"Data Source=localhost;Initial Catalog=udb_Аверьянов_Лощенко;User Id=sa; Password=qwerty12345";



        public DataSet GetTable(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM {tableName}";
                var adapter = new SqlDataAdapter(sql,connection);
                var ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public IEnumerable<string> getTableNames()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
                var adapter = new SqlDataAdapter(sql,connection);
                var ds = new DataSet();
                adapter.Fill(ds);

                return ParseTableNames(ds);
            }
        }

        private IEnumerable<string> ParseTableNames(DataSet ds)
        {
            var names = new List<string>();
            foreach (DataTable dataTable in ds.Tables)
            {
                names.Add(dataTable.TableName);
            }

            return names;
        }
    }
}
