using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	internal class SessionManager
	{
		private static readonly MainWindow Window = Application.Current.MainWindow as MainWindow;

		public static void RegisterUser(Type group, string login, string password)
		{
			string encryptedPass = EncryptString(password);
			Users.Add(new User(group, login, encryptedPass));
			SaveSystem.SaveUsersToFile();
			LogIn(login, password);
		}

		public static void LogIn(string login, string password)
		{
			var encryptedPass = EncryptString(password);
			foreach (var u in Users.Where(u => u.Login == login))
				if (encryptedPass == u.Password)
				{
					CurrentUser = u;
					SetCommonRights();
					if (CurrentUser.Group == Type.Administrator)
						SetAdminRights();
					if (CurrentUser.Group == Type.Restaurant)
						SetRestaurantRights();
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
			Window.ShowMenu("", new RoutedEventArgs());
			Window.Title = "Delivery Lab";
			Window.loginButton.Content = "Авторизация";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 59, 48));
			Window.settingsMenu.Visibility = Visibility.Collapsed;
			Window.adminMenu.Visibility = Visibility.Collapsed;
			Window.restaurantMenu.Visibility = Visibility.Collapsed;
			Window.showOrder.Visibility = Visibility.Collapsed;
			Window.showUsers.Visibility = Visibility.Collapsed;
		}

		public static void ChangePassword(string oldPassword, string newPassword)
		{
			if (EncryptString(oldPassword) == CurrentUser.Password)
			{
				CurrentUser.Password = Users.First(u => u.Id == CurrentUser.Id).Password = EncryptString(newPassword);
				SaveSystem.SaveUsersToFile();
			}
			else
				new Alert("Пароли не совпадают", "Текущий пароль введён неверно").Show();
		}

		public static void DeleteAccount(User userToDelete)
		{
			if (userToDelete.Group == Type.Restaurant)
			{
				foreach (var r in Restaurants.Where(r => r.OwnerId == userToDelete.Id).ToArray())
				{
					foreach (var d in Dishes.Where(d => d.RestId == r.Id).ToArray())
						Dishes.Remove(d);
					Restaurants.Remove(r);
				}
			}

			if (userToDelete.Id == CurrentUser.Id)
				LogOut();
			Users.Remove(Users.First(u => u.Id == userToDelete.Id));
			foreach (var i in Orders.Items.Where(i => i.UserId == userToDelete.Id))
				Orders.Items.Remove(i);
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
			foreach (var i in Orders.Items.Where(i => i.Item.RestId == restaurantToDelete.Id))
				Orders.Items.Remove(i);
			foreach (var d in Dishes.Where(d => d.RestId == restaurantToDelete.Id).ToList())
				Dishes.Remove(d);
			Restaurants.Remove(restaurantToDelete);
			SaveSystem.SaveAll();
			CurrentRestaurant = Restaurants.FirstOrDefault(r => r.OwnerId == CurrentUser.Id);
			if (CurrentRestaurant == null && restaurantToDelete.OwnerId == CurrentUser.Id)
				new AddRestaurantWindow().Show();
		}

		private static void SetCommonRights()
		{
			Window.ShowMenu("", new RoutedEventArgs());
			Window.loginButton.Content = CurrentUser.Login + " [Выйти]";
			Window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 255));
			Window.showOrder.Content = "Заказ";
			Window.settingsMenu.Visibility = Visibility.Visible;
			Window.showOrder.Visibility = Visibility.Visible;
			Window.clearOrderButton.Visibility = Visibility.Visible;
		}

		private static void SetAdminRights()
		{
			Window.Title = "Delivery Lab — Администратор";
			Window.showOrder.Content = "Заказы";
			Window.adminMenu.Visibility = Visibility.Visible;
			Window.showUsers.Visibility = Visibility.Visible;
		}

		private static void SetRestaurantRights()
		{
			CurrentRestaurant = Restaurants.FirstOrDefault(r => r.OwnerId == CurrentUser.Id);
			if (CurrentRestaurant == null)
				new AddRestaurantWindow().Show();
			else
			{
				Window.Title = "Delivery Lab — " + CurrentRestaurant.Name;
				if (!CurrentRestaurant.IsVerified)
					Window.Title += " (Не подтверждён)";
				Window.showOrder.Content = "Заказы";
				Window.restaurantMenu.Visibility = Visibility.Visible;
				Window.clearOrderButton.Visibility = Visibility.Collapsed;
				if (Orders.Items.Any(i => i.Item.RestId == CurrentRestaurant.Id && !i.IsReady))
				{
					new Alert("Есть новые заказы!", "У вас есть новые заказы,\nожидающие доставки").Show();
					Window.ShowOrder("", new RoutedEventArgs());
				}
			}
		}

		public static string EncryptString(string password)
		{
			var hash = SHA1.Create().ComputeHash(Encoding.Default.GetBytes(password));
			var builder = new StringBuilder();
			foreach (var b in hash)
				builder.Append(b.ToString("x2"));
			return builder.ToString();
		}
	}
}