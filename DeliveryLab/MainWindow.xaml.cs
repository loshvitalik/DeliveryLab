﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static ObservableCollection<User> Users;
		public static ObservableCollection<Restaurant> Restaurants;
		public static ObservableCollection<Dish> Dishes;
		public static User CurrentUser;
		public static Restaurant CurrentRestaurant;
		public static bool AutoRefresh = true;

		public MainWindow()
		{
			InitializeComponent();
			Users = new ObservableCollection<User>();
			Restaurants = new ObservableCollection<Restaurant>();
			Dishes = new ObservableCollection<Dish>();
			SaveSystem.CreateFilesIfNotPresent();
			SaveSystem.LoadAll();
			table.ItemsSource = Dishes;
			ShowMenu("", new RoutedEventArgs());
			UpdateMenu();
		}

		public static void UpdateMenu()
		{
			Dishes.Clear();
			foreach (var rest in Restaurants)
				foreach (var dish in rest.Dishes)
					Dishes.Add(dish);
		}

		private void LoginButtonClick(object sender, RoutedEventArgs e)
		{
			if (CurrentUser == null)
				new LoginWindow().Show();
			else
				SessionManager.LogOut();
		}

		// Левая колонка
		private void ShowMenu(object sender, RoutedEventArgs e)
		{
			title.Content = "Меню";
			table.ItemsSource = Dishes;
			addRowToOrder.Visibility = Visibility.Visible;
			deleteRowButton.Visibility = CurrentUser?.Group == Type.Administrator ? Visibility.Visible : Visibility.Collapsed;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Ресторан",
				Binding = new Binding("Restaurant"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Стоимость",
				Binding = new Binding("Price"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
			});

		}

		private void ShowRestaurants(object sender, RoutedEventArgs e)
		{
			title.Content = "Рестораны";
			table.ItemsSource = Restaurants;
			addRowToOrder.Visibility = Visibility.Collapsed;
			deleteRowButton.Visibility = CurrentUser?.Group == Type.Administrator ? Visibility.Visible : Visibility.Collapsed;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridCheckBoxColumn()
			{
				Header = "Проверен",
				Binding = new Binding("IsVerified"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(
				new DataGridTextColumn()
				{
					Header = "Рейтинг",
					Binding = new Binding("Rating"),
					Width = new DataGridLength(25, DataGridLengthUnitType.Star),
				});
		}

		private void ShowOrder(object sender, RoutedEventArgs e)
		{
			//CurrentUser.Order = new Order();
			title.Content = "Заказ";
			table.ItemsSource = CurrentUser.Order.Items;
			addRowToOrder.Visibility = Visibility.Collapsed;
			deleteRowButton.Visibility = Visibility.Visible;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Item.Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Количество",
				Binding = new Binding("Count"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Стоимость",
				Binding = new Binding("Sum"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
			});

		}

		private void ShowUsers(object sender, RoutedEventArgs e)
		{
			title.Content = "Пользователи";
			table.ItemsSource = Users;
			addRowToOrder.Visibility = Visibility.Collapsed;
			deleteRowButton.Visibility = CurrentUser?.Group == Type.Administrator ? Visibility.Visible : Visibility.Collapsed;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "ID",
				Binding = new Binding("ID"),
				Width = new DataGridLength(10, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Логин",
				Binding = new Binding("Login"),
				Width = new DataGridLength(60, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Группа",
				Binding = new Binding("Group"),
				Width = new DataGridLength(30, DataGridLengthUnitType.Star),
			});
		}

		private void AddNewDish(object sender, RoutedEventArgs e)
		{
			if (CurrentUser?.Group == Type.Restaurant)
				CurrentRestaurant.UpdateDishes(new List<Dish>()
				{ new Dish("Рандомная еда", new Random().Next(0, 10000)) });
		}

		private void OpenCalcButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start("calc.exe");
		}

		// Меню
		private void VerifyAll(object sender, RoutedEventArgs e)
		{
			foreach (Restaurant r in Restaurants)
				r.IsVerified = true;
		}

		private void LoadUsersButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.SetUsersFileName();
		}

		private void LoadRestsButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.SetRestsFileName();
		}

		private void SaveAllButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.SaveAll();
		}

		private void ClearAllButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.ClearAll();
		}

		private void ToggleAutoRefresh(object sender, RoutedEventArgs e)
		{
			AutoRefresh = autoRefreshButton.IsChecked = !AutoRefresh;
		}

		private void UpdateMenuButtonClick(object sender, RoutedEventArgs e)
		{
			UpdateMenu();
		}

		private void ShowChangePasswordWindow(object sender, RoutedEventArgs e)
		{
			new ChangePasswordWindow().Show();
		}

		private void DeleteRestaurantButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.DeleteRestaurant();
		}

		private void DeleteAccountButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.DeleteAccount(CurrentUser);
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

		private void AddRowToOrderClick(object sender, RoutedEventArgs e)
		{
			CurrentUser.Order.Add((Dish)table.SelectedItem);
			table.Items.Refresh();
			SaveSystem.SaveUsersToFile();
		}

		private void DeleteRowButtonClick(object sender, RoutedEventArgs e)
		{
			var selectedItem = table.SelectedItem;
			if (selectedItem is Dish && CurrentUser.Group == Type.Administrator)
				Dishes.Remove((Dish)selectedItem);
			if (selectedItem is Restaurant && CurrentUser.Group == Type.Administrator)
				Restaurants.Remove((Restaurant)selectedItem);
			if (selectedItem is OrderItem)
				CurrentUser.Order.Remove(((OrderItem)selectedItem).Item);
			if (selectedItem is User && CurrentUser.Group == Type.Administrator)
				SessionManager.DeleteAccount((User)selectedItem);
			table.Items.Refresh();
			SaveSystem.SaveAll();

		}

		private void CloseAppButtonClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}