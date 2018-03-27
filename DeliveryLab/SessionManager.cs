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
					if (w is LoginWindow)
						w.Close();
		}

		public static void LogOut()
		{
			CurrentUser = null;
			CurrentRestaurant = null;
			Window.Title = "Delivery Lab";
			Window.loginButton.Content = "Авторизация";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 59, 48));
			Window.deleteAccountButton.IsEnabled = false;
			Window.showOrders.Visibility = Visibility.Collapsed;
			Window.adminMenu.Visibility = Visibility.Collapsed;
			Window.addDishButton.Visibility = Visibility.Collapsed;
		}

		public static void DeleteAccount()
		{
			Users.Remove(CurrentUser);
			if (CurrentUser.Group == Type.Restaurant)
				Restaurants.RemoveAll(r => r.OwnerID == CurrentUser.ID);
			LogOut();
		}

		public static void AddRestaurant(string name)
		{
			var restautant = new Restaurant(name, CurrentUser);
			Restaurants.Add(restautant);
			CurrentRestaurant = restautant;
			SetRestaurantRights();
		}

		private static void SetCommonRights()
		{
			Window.loginButton.Content = CurrentUser.Login + " [Выйти]";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 255));
			Window.deleteAccountButton.IsEnabled = true;
			Window.showOrders.Visibility = Visibility.Visible;
		}

		private static void SetAdminRights()
		{
			Window.Title = "Delivery Lab — Администратор";
			Window.deleteAccountButton.IsEnabled = true;
			Window.adminMenu.Visibility = Visibility.Visible;
			Window.addDishButton.Visibility = Visibility.Visible;
		}

		private static void SetRestaurantRights()
		{
			foreach (var r in Restaurants)
				if (r.OwnerID == CurrentUser.ID) CurrentRestaurant = r;
			if (CurrentRestaurant == null)
				new AddRestaurantWindow().Show();
			else
			{
				Window.Title = "Delivery Lab — " + CurrentRestaurant.Name;
				if (!CurrentRestaurant.IsVerified)
					Window.Title += " (Не подтверждён)";
			}
		}

		public static string EncryptString(string password)
		{
			byte[] hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(password));
			var builder = new StringBuilder();
			foreach (var b in hash)
				builder.Append(b.ToString("x2"));
			return builder.ToString();
		}
	}
}