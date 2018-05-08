using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;
using Excel = Microsoft.Office.Interop.Excel.Application;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DeliveryLab
{
	internal class SaveSystem
	{
		private static string UsersDB = Path.Combine(Environment.CurrentDirectory, "data\\users.txt");
		private static string RestsDB = Path.Combine(Environment.CurrentDirectory, "data\\rests.txt");
		private static string OrdersDB = Path.Combine(Environment.CurrentDirectory, "data\\orders.txt");
		private static string DishesDB = Path.Combine(Environment.CurrentDirectory, "data\\dishes.txt");

		public static void CreateFilesIfNotPresent()
		{
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "data")))
				Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "data"));
			if (!File.Exists(UsersDB))
			{
				File.Create(UsersDB).Close();
				File.AppendAllText(UsersDB, @"[{""Id"":0,""Group"":0,""Login"":""loshvitalik"",""Password"":""d033e22ae348aeb5660fc2140aec35850c4da997""}]");
			}

			if (!File.Exists(RestsDB))
				File.Create(RestsDB).Close();
			if (!File.Exists(OrdersDB))
				File.Create(OrdersDB).Close();
			if (!File.Exists(DishesDB))
				File.Create(DishesDB).Close();
		}

		public static void LoadAll()
		{
			LoadUsersFromFile();
			LoadRestsFromFile();
			LoadDishesFromFile();
			LoadOrdersFromFile();
		}

		public static void SaveAll()
		{
			SaveUsersToFile();
			SaveRestsToFile();
			SaveDishesToFile();
			SaveOrdersToFile();
		}

		private static void LoadUsersFromFile()
		{
			Users.Clear();
			try
			{
				Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(File.ReadAllText(UsersDB, Encoding.Default))
				        ?? new ObservableCollection<User>();
			}
			catch (JsonReaderException)
			{
				new Alert("Неверный формат файла", "Файл должен быть в формате JSON").Show();
				UsersDB = Path.Combine(Environment.CurrentDirectory, "data\\users.txt");
			}
		}

		public static void SaveUsersToFile()
		{
			File.WriteAllBytes(UsersDB, new byte[0]);
			File.AppendAllText(UsersDB, JsonConvert.SerializeObject(Users), Encoding.Default);
		}

		public static void SetUsersFileName()
		{
			var dialog = new OpenFileDialog
			{
				InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
				Filter = "Текстовые файлы (*.txt)|*.txt",
				Title = "Загрузить из JSON-файла"
			};
			if (dialog.ShowDialog() == true)
				UsersDB = dialog.FileName;
			LoadUsersFromFile();
		}

		private static void LoadRestsFromFile()
		{
			Restaurants.Clear();
			try
			{
				Restaurants =
					JsonConvert.DeserializeObject<ObservableCollection<Restaurant>>(File.ReadAllText(RestsDB, Encoding.Default))
					?? new ObservableCollection<Restaurant>();
			}
			catch (JsonReaderException)
			{
				new Alert("Неверный формат файла", "Файл должен быть в формате JSON").Show();
				RestsDB = Path.Combine(Environment.CurrentDirectory, "data\\rests.txt");
			}
		}

		public static void SaveRestsToFile()
		{
			File.WriteAllBytes(RestsDB, new byte[0]);
			File.AppendAllText(RestsDB, JsonConvert.SerializeObject(Restaurants), Encoding.Default);
		}

		public static void SetRestsFileName()
		{
			var dialog = new OpenFileDialog
			{
				InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
				Filter = "Текстовые файлы (*.txt)|*.txt",
				Title = "Загрузить из JSON-файла"
			};
			if (dialog.ShowDialog() == true)
				RestsDB = dialog.FileName;
			LoadRestsFromFile();
		}

		public static void LoadDishesFromFile()
		{
			Dishes.Clear();
			try
			{
				Dishes = JsonConvert.DeserializeObject<ObservableCollection<Dish>>(File.ReadAllText(DishesDB, Encoding.Default))
				         ?? new ObservableCollection<Dish>();
			}
			catch (JsonReaderException)
			{
				new Alert("Неверный формат файла", "Файл должен быть в формате JSON").Show();
				DishesDB = Path.Combine(Environment.CurrentDirectory, "data\\dishes.txt");
			}
		}

		public static void SaveDishesToFile()
		{
			File.WriteAllBytes(DishesDB, new byte[0]);
			File.AppendAllText(DishesDB, JsonConvert.SerializeObject(Dishes), Encoding.Default);
		}

		public static void LoadOrdersFromFile()
		{
			Orders.Items.Clear();
			try
			{
				Orders = JsonConvert.DeserializeObject<OrderList>(File.ReadAllText(OrdersDB, Encoding.Default))
				         ?? new OrderList();
			}
			catch (JsonReaderException)
			{
				new Alert("Неверный формат файла", "Файл должен быть в формате JSON").Show();
				OrdersDB = Path.Combine(Environment.CurrentDirectory, "data\\orders.txt");
			}
		}

		public static void SaveOrdersToFile()
		{
			File.WriteAllBytes(OrdersDB, new byte[0]);
			File.AppendAllText(OrdersDB, JsonConvert.SerializeObject(Orders), Encoding.Default);
		}

		public static void ClearOrders()
		{
			Orders.Items.Clear();
			SaveOrdersToFile();
		}
	}
}