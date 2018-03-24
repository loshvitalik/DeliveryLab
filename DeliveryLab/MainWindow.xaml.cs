using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
		public static User CurrentUser;
		public static Restaurant CurrentRestaurant;
		public static bool AutoRefresh = true;

		public MainWindow()
		{
			InitializeComponent();
			Users = new List<User>();
			Restaurants = new List<Restaurant>();
			Dishes = new ObservableCollection<Dish>();

			// test
			Users.Add(new User("loshvitalik", "12345", Type.Administrator));
			Restaurants.Add(new Restaurant("Ресторан", Users.Where(u => u.Login == "loshvitalik").First()));
			Restaurants[0].UpdateDishes(new[] { new Dish("Пельмени", 150) });
			//

			UpdateMenu();
			table.ItemsSource = Dishes;
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
			if (CurrentUser?.Group == Type.Administrator)
				Dishes.Insert(0, new Dish(CurrentUser.ToString(), new Random().Next(0, 1000)));
			var a = Restaurants[0].ToString();
		}

		private void ToggleAutoRefresh(object sender, RoutedEventArgs e)
		{
			AutoRefresh = autoRefreshButton.IsChecked = !AutoRefresh;
		}

		private void LoginButtonClick(object sender, RoutedEventArgs e)
		{
			if (CurrentUser == null)
				new LoginWindow().Show();
			else
				SessionManager.LogOut();
		}

		private void DeleteAccountButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.DeleteAccount();
		}

		private void ShowHelpWindow(object sender, RoutedEventArgs e)
		{
			new Alert("Помощь — Delivery Lab",
				"1. Зарегистрируйтесь или авторизуйтесь\n2. Выберите блюда в меню справа\n3. Сделайте заказ и ждите доставки!")
				.Show();
		}

		private void ShowAboutWindow(object sender, RoutedEventArgs e)
		{
			new Alert("О программе \"Delivery Lab\"",
				"Delivery Lab v. 0.3 alpha\n© 2018 loshvitalik, MrBlacktop").Show();
		}

		private void CloseApp(object sender, EventArgs e)
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