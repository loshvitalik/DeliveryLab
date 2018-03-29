using System;
using System.Text;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;
using System.Collections.ObjectModel;

namespace DeliveryLab
{
	class SaveSystem
	{
		private static string UsersDB = Path.Combine(Environment.CurrentDirectory, "data\\users.txt");
		private static string RestsDB = Path.Combine(Environment.CurrentDirectory, "data\\rests.txt");

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
			Users.Clear();
			Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(File.ReadAllText(UsersDB, Encoding.Default))
				?? new ObservableCollection<User>();
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
				FileName = "users.txt", Filter = "Текстовые файлы (*.txt)|*.txt",
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
			Restaurants = JsonConvert.DeserializeObject<ObservableCollection<Restaurant>>(File.ReadAllText(RestsDB, Encoding.Default))
				?? new ObservableCollection<Restaurant>();
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
				FileName = "rests.txt", Filter = "Текстовые файлы (*.txt)|*.txt",
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
