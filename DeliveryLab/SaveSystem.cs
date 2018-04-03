using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	class SaveSystem
	{
		private static string UsersDB = Path.Combine(Environment.CurrentDirectory, "data\\users.txt");
		private static string RestsDB = Path.Combine(Environment.CurrentDirectory, "data\\rests.txt");
		private static string DishesDB = Path.Combine(Environment.CurrentDirectory, "data\\dishes.txt");

		public static void CreateFilesIfNotPresent()
		{
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "data")))
				Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "data"));
			if (!File.Exists(UsersDB))
				File.Create(UsersDB).Close();
			if (!File.Exists(RestsDB))
				File.Create(RestsDB).Close();
			if (!File.Exists(DishesDB))
				File.Create(DishesDB).Close();
		}

		public static void LoadAll()
		{
			LoadUsersFromFile();
			LoadRestsFromFile();
			LoadDishesFromFile();
		}

		public static void SaveAll()
		{
			SaveUsersToFile();
			SaveRestsToFile();
			SaveDishesToFile();
		}

		public static void ClearAll()
		{
			ClearUsers();
			ClearRests();
			ClearDishes();
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
			var dialog = new OpenFileDialog()
			{
				InitialDirectory = Path.Combine(Environment.CurrentDirectory, "data"),
				FileName = "users.txt",
				Filter = "Текстовые файлы (*.txt)|*.txt",
				Title = "Загрузить из JSON-файла"
			};
			if (dialog.ShowDialog() == true)
				UsersDB = dialog.FileName;
			LoadUsersFromFile();
		}

		private static void ClearUsers()
		{
			Users.Clear();
			if (CurrentUser != null)
				Users.Add(CurrentUser);
			SaveUsersToFile();
		}

		private static void LoadRestsFromFile()
		{
			Restaurants.Clear();
			try
			{
				Restaurants = JsonConvert.DeserializeObject<ObservableCollection<Restaurant>>(File.ReadAllText(RestsDB, Encoding.Default))
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
			var dialog = new OpenFileDialog()
			{
				InitialDirectory = Path.Combine(Environment.CurrentDirectory, "data"),
				FileName = "rests.txt",
				Filter = "Текстовые файлы (*.txt)|*.txt",
				Title = "Загрузить из JSON-файла"
			};
			if (dialog.ShowDialog() == true)
				RestsDB = dialog.FileName;
			LoadRestsFromFile();
		}

		private static void ClearRests()
		{
			Restaurants.Clear();
			SaveRestsToFile();
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

		private static void ClearDishes()
		{
			Dishes.Clear();
			SaveDishesToFile();
		}
	}
}
