using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static DeliveryLab.MainWindow;

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
		}

		public static void LoadUsersFromFile()
		{
			Users.Clear();
			string[] lines = File.ReadAllLines(UsersDB);
			foreach (string u in lines)
				Users.Add(User.FromString(u));
		}

		public static void SaveUsersToFile()
		{
			File.WriteAllBytes(UsersDB, new byte[0]);
			foreach (User u in Users)
				File.AppendAllText(UsersDB, u.ToString() + "\n");
		}

		public static void LoadRestsFromFile()
		{
			Restaurants.Clear();
			string[] lines = File.ReadAllLines(RestsDB);
			foreach (string r in lines)
				Restaurants.Add(Restaurant.FromString(r));
		}

		public static void SaveRestsToFile()
		{
			File.WriteAllBytes(RestsDB, new byte[0]);
			foreach (Restaurant r in Restaurants)
				File.AppendAllText(RestsDB, r.ToString() + "\n");
		}

		private static void ClearUsers()
		{
			Users.Clear();
			File.WriteAllBytes(UsersDB, new byte[0]);
		}
	}
}
