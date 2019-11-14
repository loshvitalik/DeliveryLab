using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DBConection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase dataBase = new DataBase();

            var tableNames = dataBase.getTableNames();

            foreach (var tableName in tableNames)
            {
                Console.WriteLine(tableName);
            }


            Thread.Sleep(1000000);
        }
    }
}
