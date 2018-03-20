using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static List<User> Users;
		public static List<Restaurant> Restaurants;
		public static ObservableCollection<Dish> Dishes;
		public static bool IsLoggedIn = false;
		public static bool AutoRefresh = true;

		public MainWindow()
		{
			InitializeComponent();
			Users = new List<User>();
			Restaurants = new List<Restaurant>();
			Dishes = new ObservableCollection<Dish>();

			// test
			Users.Add(new User("loshvitalik", "12345", "12345"));
			Restaurants.Add(new Restaurant("Ресторан", Users[0], 5, new[] { new Dish("Блюдо", 500) }));
			//

			UpdateMenu();
			table.ItemsSource = Dishes;
		}

		public static void LoginUser(string login, string password)
		{
			foreach (User u in Users)
				if (login == u.Login && password == u.Password)
				{
					IsLoggedIn = true;
					var window = GetWindow(Application.Current.MainWindow) as MainWindow;
					window.loginButton.Content = login;
					window.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 255));
					break;
				}
		}

		public void UpdateMenu()
		{
			Dishes.Clear();
			foreach (var rest in Restaurants)
				foreach (var dish in rest.Dishes)
					Dishes.Add(dish);
		}

		private void AddNewDish(object sender, RoutedEventArgs e)
		{
			Dishes.Insert(0, new Dish("Блюдо", new Random().Next(0, 1000)));
		}

		private void ToggleAutoRefresh(object sender, RoutedEventArgs e)
		{
			AutoRefresh = autoRefreshButton.IsChecked = !AutoRefresh;
		}

		private void ShowLoginWindow(object sender, RoutedEventArgs e)
		{
			if (!IsLoggedIn)
				new LoginWindow().Show();
		}

		private void ShowHelpWindow(object sender, RoutedEventArgs e)
		{
			var helpWindow = new MessageWindow() { Title = "Помощь — Delivery Lab" };
			helpWindow.label.Content = "1. Зарегистрируйтесь или авторизуйтесь\n2. Выберите блюда в меню справа\n3. Сделайте заказ и ждите доставки!";
			helpWindow.Show();
		}

		private void ShowAboutWindow(object sender, RoutedEventArgs e)
		{
			var aboutWindow = new MessageWindow { Title = "О программе \"Delivery Lab\"" };
			aboutWindow.label.Content = "Delivery Lab v. 0.3 alpha\n© 2018 loshvitalik, MrBlacktop";
			aboutWindow.Show();
		}

		private void UnloadAppFromEvent(object sender, EventArgs e)
		{
			UnloadApp();
		}

		private void UnloadApp()
		{
			Application.Current.Shutdown();
		}

		private void ShowMenu(object sender, RoutedEventArgs e)
		{
			title.Content = "Меню";
			Dishes.Clear();
		}

		private void ShowRestaurants(object sender, RoutedEventArgs e)
		{
			title.Content = "Рестораны";
			Dishes.Clear();
		}

		private void ShowOrders(object sender, RoutedEventArgs e)
		{
			title.Content = "Заказы";
			Dishes.Clear();
		}
	}
}