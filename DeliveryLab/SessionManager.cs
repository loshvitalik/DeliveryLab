using System.Windows;
using System.Windows.Media;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	class SessionManager
	{
		private static MainWindow Window = Application.Current.MainWindow as MainWindow;

		public static void RegisterUser(string login, string password, Type group)
		{
			Users.Add(new User(login, password, group));
			LogIn(login, password);
		}

		public static void LogIn(string login, string password)
		{
			foreach (User u in Users)
				if (login == u.Login)
				{
					if (User.EncryptString(password) == u.Password)
					{
						CurrentUser = u;
						if (CurrentUser.Group == Type.Administrator)
							SetAdminRights();
						if (CurrentUser.Group == Type.Restaurant)
							SetRestaurantRights();
						SetCommonRights();
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
			Window.showOrders.Visibility = Visibility.Collapsed;
			Window.adminMenu.Visibility = Visibility.Collapsed;
			Window.addDishButton.Visibility = Visibility.Collapsed;
		}

		public static void DeleteAccount()
		{
			Users.Remove(CurrentUser);
			Restaurants.RemoveAll(r => r.Owner == CurrentUser.Login);
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
			Window.showOrders.Visibility = Visibility.Visible;
		}

		private static void SetAdminRights()
		{
			Window.Title = "Delivery Lab — Администратор";
			Window.adminMenu.Visibility = Visibility.Visible;
			Window.addDishButton.Visibility = Visibility.Visible;
		}

		private static void SetRestaurantRights()
		{
			foreach (var r in Restaurants)
				if (r.Owner == CurrentUser.Login) CurrentRestaurant = r;
			if (CurrentRestaurant == null)
				new AddRestaurant().Show();
			else
			{
				Window.Title = "Delivery Lab — " + CurrentRestaurant.Name;
				if (!CurrentRestaurant.IsVerified)
					Window.Title += " (Не подтверждён)";
			}
		}
	}
}