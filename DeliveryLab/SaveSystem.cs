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

		public static void CreateFilesIfNotPresent()
		{
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "data")))
				Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "data"));
			if (!File.Exists(UsersDB))
				File.Create(UsersDB).Close();
			if (!File.Exists(RestsDB))
				File.Create(RestsDB).Close();
		}

		public static void LoadAll()
		{
			LoadUsersFromFile();
			LoadRestsFromFile();
		}

		public static void SaveAll()
		{
			SaveUsersToFile();
			SaveRestsToFile();
		}

		public static void ClearAll()
		{
			ClearUsers();
			ClearRests();
		}

		private static void LoadUsersFromFile()
		{
			if (!File.Exists(UsersDB)) File.Create(UsersDB);
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
				LoadUsersFromFile();
			}
		}

		public static void SaveUsersToFile()
		{
			if (!File.Exists(UsersDB)) File.Create(UsersDB);
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
			if (!File.Exists(RestsDB)) File.Create(RestsDB);
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
				LoadRestsFromFile();
			}
		}

		public static void SaveRestsToFile()
		{
			if (!File.Exists(RestsDB)) File.Create(RestsDB);
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
	}
}
