using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DBConection
{
    public class DataBase
    {
		// строка подключения к БД
        private readonly string _connectionString = @"Data Source=localhost;Initial Catalog=udb_Аверьянов_Лощенко;User Id=sa; Password=1";
        private string _sql;


	    // метод получения данных из таблицы по её названию
        public DataSet GetTable(string tableName)
        {
			// запрос данных выполняется с использованием подключения с заданной строкой
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM {tableName}"; // создаётся SQL-запрос SELECT, в который помещается переданное имя таблицы
                var adapter = new SqlDataAdapter(sql,connection);
                var ds = new DataSet(); 
                adapter.Fill(ds); // данные таблицы помещаются в класс DataSet и возвращаются

                _sql = sql;
                return ds;
            }
        }

		// метод получения названий всех таблиц в БД
        public IEnumerable<string> getTableNames()
		{
			// запрос данных выполняется с использованием подключения с заданной строкой
			using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"; // создаётся SQL-запрос SELECT из системной таблицы TABLES
                var adapter = new SqlDataAdapter(sql,connection);
                var ds = new DataSet();
                adapter.Fill(ds); // данные таблицы помещаются в класс DataSet и возвращаются

				return ParseTableNames(ds);
            }
        }

		// Метод выделения имён таблиц в виде строк из класса DataSet
        private IEnumerable<string> ParseTableNames(DataSet ds)
        {
			var names = new List<string>();
			// для каждой строки таблицы TABLES в список помещается имя таблицы, затем этот список возвращается
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                names.Add(dataRow[2].ToString());
            }

            return names;
        }

		// метод обновления данных
        public void Update(DataRow row, int index)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
					
				// создание подключение к БД
                var adapter = new SqlDataAdapter(_sql, connection);
                var ds = new DataSet();
                adapter.Fill(ds);

				// получение исходной строки таблицы
                var itemArray = ds.Tables[0].Rows[index].ItemArray;
				var newRow = row.ItemArray;
				// замена новыми значениями
                for (int i = 0; i < itemArray.Length; i++)
                {
                    itemArray[i] = newRow[i];
                }

				// обновление таблицы в БД
                ds.Tables[0].Rows[index].ItemArray = itemArray;
				var a = new SqlCommandBuilder(adapter);

                adapter.Update(ds);
            }
        }

		// метод удаления данных
        public void DeleteRow(int index)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter(_sql, connection);
                var ds = new DataSet();
                adapter.Fill(ds);
                ds.Tables[0].Rows[index].Delete(); // удаление строки из БД по ее индексу
                adapter.Update(ds);
            }
        }

		// метод добавления данных
        public void AddRow(DataRow row)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var adapter = new SqlDataAdapter(_sql, connection);
                var ds = new DataSet();
                adapter.Fill(ds);
                var dataRow = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dataRow); // новая строка добавляется в БД и вызывается обновление
                adapter.Update(ds);
            }
        }
    }
}
