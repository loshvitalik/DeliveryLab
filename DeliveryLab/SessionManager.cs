using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	class SessionManager
	{
		private static MainWindow Window = Application.Current.MainWindow as MainWindow;

		public static void RegisterUser(Type group, string login, string password)
		{
			string encryptedPass = EncryptString(password);
			Users.Add(new User(group, login, encryptedPass));
			SaveSystem.SaveUsersToFile();
			LogIn(login, password);
		}

		public static void LogIn(string login, string password)
		{
			string encryptedPass = EncryptString(password);
			foreach (User u in Users)
				if (login == u.Login)
				{
					if (encryptedPass == u.Password)
					{
						CurrentUser = u;
						SetCommonRights();
						if (CurrentUser.Group == Type.Administrator)
							SetAdminRights();
						if (CurrentUser.Group == Type.Restaurant)
							SetRestaurantRights();
					}
					break;
				}
			if (CurrentUser == null)
				new Alert("Неверный пароль",
					"Неверный логин или пароль.\nПроверьте правильность\nвведённых данных").Show();
			else
				foreach (Window w in Application.Current.Windows)
					if (w is LoginWindow) w.Close();
		}

		public static void LogOut()
		{
			CurrentUser = null;
			CurrentRestaurant = null;
			Window.Title = "Delivery Lab";
			Window.loginButton.Content = "Авторизация";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 59, 48));
			Window.changePassButton.IsEnabled = false;
			Window.deleteRestaurantButton.Visibility = Visibility.Collapsed;
			Window.deleteAccountButton.IsEnabled = false;
			Window.adminMenu.Visibility = Visibility.Collapsed;
			Window.showOrder.Visibility = Visibility.Collapsed;
			Window.showUsers.Visibility = Visibility.Collapsed;
			Window.addDishButton.Visibility = Visibility.Collapsed;
			Window.addRowToOrder.Visibility = Visibility.Collapsed;
			Window.deleteRowButton.Visibility = Visibility.Collapsed;
		}

		public static void ChangePassword(string oldPassword, string newPassword)
		{
			if (EncryptString(oldPassword) == CurrentUser.Password)
			{
				CurrentUser.Password = Users.Where(u => u.ID == CurrentUser.ID).First().Password = EncryptString(newPassword);
				SaveSystem.SaveAll();
			}
			else
				new Alert("Пароли не совпадают", "Текущий пароль введён неверно").Show();
		}

		public static void DeleteAccount(User userToDelete)
		{
			if (userToDelete.ID == CurrentUser.ID)
				LogOut();
			Users.Remove(Users.Where(u => u.ID == userToDelete.ID).First());
			if (userToDelete.Group == Type.Restaurant)
			{
				foreach (Dish d in Dishes.ToList())
					if (d.Restaurant == CurrentRestaurant) Dishes.Remove(d);
				foreach (Restaurant r in Restaurants.ToList())
					if (r.OwnerID == userToDelete.ID) Restaurants.Remove(r);
			}
			SaveSystem.SaveAll();
		}

		public static void AddRestaurant(string name)
		{
			var restautant = new Restaurant(name, CurrentUser);
			Restaurants.Add(restautant);
			SaveSystem.SaveRestsToFile();
			CurrentRestaurant = restautant;
			SetRestaurantRights();
		}

		public static void DeleteRestaurant(Restaurant restaurantToDelete)
		{
			foreach (Dish d in Dishes.ToList())
				if (d.Restaurant == restaurantToDelete) Dishes.Remove(d);
			Restaurants.Remove(restaurantToDelete);
			SaveSystem.SaveRestsToFile();
			SaveSystem.SaveDishesToFile();
			CurrentRestaurant = Restaurants.Where(r => r.OwnerID == CurrentUser.ID).FirstOrDefault();
			if (CurrentRestaurant == null && restaurantToDelete.OwnerID == CurrentUser.ID)
				new AddRestaurantWindow().Show();
		}

		private static void SetCommonRights()
		{
			Window.loginButton.Content = CurrentUser.Login + " [Выйти]";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 255));
			Window.changePassButton.IsEnabled = true;
			Window.deleteAccountButton.IsEnabled = true;
			Window.showOrder.Visibility = Visibility.Visible;
			Window.addRowToOrder.Visibility = Visibility.Visible;
		}

		private static void SetAdminRights()
		{
			Window.Title = "Delivery Lab — Администратор";
			Window.adminMenu.Visibility = Visibility.Visible;
			Window.showUsers.Visibility = Visibility.Visible;
			Window.deleteRowButton.Visibility = Visibility.Visible;
		}

		private static void SetRestaurantRights()
		{
			CurrentRestaurant = Restaurants.Where(r => r.OwnerID == CurrentUser.ID).FirstOrDefault();
			if (CurrentRestaurant == null)
				new AddRestaurantWindow().Show();
			else
			{
				Window.Title = "Delivery Lab — " + CurrentRestaurant.Name;
				if (!CurrentRestaurant.IsVerified)
					Window.Title += " (Не подтверждён)";
			}
			Window.deleteRestaurantButton.Visibility = Visibility.Visible;
			Window.addDishButton.Visibility = Visibility.Visible;
		}

		public static string EncryptString(string password)
		{
			byte[] hash = SHA1.Create().ComputeHash(Encoding.Default.GetBytes(password));
			var builder = new StringBuilder();
			foreach (var b in hash)
				builder.Append(b.ToString("x2"));
			return builder.ToString();
		}
	}
}