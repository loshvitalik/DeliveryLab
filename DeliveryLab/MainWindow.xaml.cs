﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public static ObservableCollection<User> Users;
		public static ObservableCollection<Restaurant> Restaurants;
		public static ObservableCollection<Dish> Dishes;
		public static OrderList Orders;
		public static User CurrentUser;
		public static Restaurant CurrentRestaurant;

		public MainWindow()
		{
			InitializeComponent();
			Users = new ObservableCollection<User>();
			Restaurants = new ObservableCollection<Restaurant>();
			Orders = new OrderList();
			Dishes = new ObservableCollection<Dish>();
			SaveSystem.CreateFilesIfNotPresent();
			SaveSystem.LoadAll();
			table.ItemsSource = Dishes;
			ShowMenu("", new RoutedEventArgs());
		}

		// Авторизация
		private void LoginButtonClick(object sender, RoutedEventArgs e)
		{
			if (CurrentUser == null)
				new LoginWindow().Show();
			else
				SessionManager.LogOut();
		}

		// Верхнее меню

		// -Администрирование
		private void VerifyAllButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (var r in Restaurants.Where(r => !r.IsVerified))
				r.IsVerified = true;
			SaveSystem.SaveRestsToFile();
			table.Items.Refresh();
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

		private void ClearAllOrdersButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.ClearOrders();
			table.Items.Refresh();
		}

		// -Ресторан
		private void CompleteAllOrdersButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (var item in Orders.Items.Where(i => i.Item.RestId == CurrentRestaurant.Id))
				item.IsReady = true;
			SaveSystem.SaveOrdersToFile();
			table.Items.Refresh();
		}

		private void AddDishesButtonClick(object sender, RoutedEventArgs e)
		{
			new AddDishesWindow().Show();
		}

		// -Настройки
		private void ClearOrderButtonClick(object sender, RoutedEventArgs e)
		{
			foreach (var item in Orders.Items.Where(i => i.UserId == CurrentUser.Id).ToArray())
				Orders.Items.Remove(item);
			table.Items.Refresh();
		}

		private void ChangePasswordButtonClick(object sender, RoutedEventArgs e)
		{
			new ChangePasswordWindow().Show();
		}

		private void DeleteRestaurantButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.DeleteRestaurant(CurrentRestaurant);
		}

		private void DeleteAccountButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.DeleteAccount(CurrentUser);
		}

		// -О программе
		private void AboutButtonClick(object sender, RoutedEventArgs e)
		{
			new Alert("О программе \"Delivery Lab\"",
				"Delivery Lab v. 1.0\n© 2018 loshvitalik, MrBlacktop").Show();
		}

		// Левое меню

		public void ShowMenu(object sender, RoutedEventArgs e)
		{
			title.Content = "Меню";
			table.ItemsSource = Dishes;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(45, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Ресторан",
				Binding = new Binding("RestName"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Стоимость",
				Binding = new Binding("Price"),
				Width = new DataGridLength(20, DataGridLengthUnitType.Star)
			});
			if (CurrentUser?.Group == Type.User || CurrentUser?.Group == Type.Administrator)
				table.Columns.Add(new DataGridTextColumn
				{
					Header = "Заказ",
					CellStyle = FindResource("addCell") as Style,
					Width = new DataGridLength(10, DataGridLengthUnitType.Star)
				});
			if (CurrentUser?.Group == Type.Administrator || CurrentUser?.Group == Type.Restaurant)
				table.Columns.Add(new DataGridTextColumn
				{
					Header = "Удалить",
					CellStyle = FindResource("removeCell") as Style,
					Width = new DataGridLength(10, DataGridLengthUnitType.Star)
				});
		}

		private void ShowRestaurants(object sender, RoutedEventArgs e)
		{
			title.Content = "Рестораны";
			table.ItemsSource = Restaurants;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(45, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Рейтинг",
				Binding = new Binding("Rating"),
				CellStyle = FindResource("ratingCell") as Style,
				Width = new DataGridLength(25, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridCheckBoxColumn
			{
				Header = "Проверен",
				Binding = new Binding("IsVerified"),
				CellStyle = FindResource("checkCell") as Style,
				Width = new DataGridLength(20, DataGridLengthUnitType.Star)
			});
			if (CurrentUser?.Group != Type.User)
				table.Columns.Add(new DataGridTextColumn
				{
					Header = "Удалить",
					CellStyle = FindResource("removeCell") as Style,
					Width = new DataGridLength(10, DataGridLengthUnitType.Star)
				});
		}

		public void ShowOrder(object sender, RoutedEventArgs e)
		{
			table.ItemsSource = Orders.Items.Where(i =>
				CurrentUser.Group == Type.User
					? i.UserId == CurrentUser.Id
					: CurrentUser.Group == Type.Administrator || i.Item.RestId == CurrentRestaurant.Id);
			title.Content = CurrentUser.Group == Type.User ? "Заказ" : "Заказы";
			table.Columns.Clear();
			if (CurrentUser.Group != Type.User)
				table.Columns.Add(new DataGridTextColumn
				{
					Header = "Пользователь",
					Binding = new Binding("UserName"),
					Width = new DataGridLength(25, DataGridLengthUnitType.Star)
				});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Заказ",
				Binding = new Binding("Item.Name"),
				Width = new DataGridLength(45, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Кол-во",
				Binding = new Binding("Count"),
				Width = new DataGridLength(15, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Стоимость",
				Binding = new Binding("Sum"),
				Width = new DataGridLength(20, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridCheckBoxColumn
			{
				Header = "Готов",
				Binding = new Binding("IsReady"),
				CellStyle = FindResource("checkCell") as Style,
				Width = new DataGridLength(10, DataGridLengthUnitType.Star)
			});
			if (CurrentUser.Group != Type.Restaurant)
				table.Columns.Add(new DataGridTextColumn
				{
					Header = "Удалить",
					CellStyle = FindResource("removeCell") as Style,
					Width = new DataGridLength(10, DataGridLengthUnitType.Star)
				});
		}

		private void ShowUsers(object sender, RoutedEventArgs e)
		{
			title.Content = "Пользователи";
			table.ItemsSource = Users;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Логин",
				Binding = new Binding("Login"),
				Width = new DataGridLength(45, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "ID",
				Binding = new Binding("Id"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Группа",
				Binding = new Binding("Group"),
				Width = new DataGridLength(20, DataGridLengthUnitType.Star)
			});
			table.Columns.Add(new DataGridTextColumn
			{
				Header = "Удалить",
				CellStyle = FindResource("removeCell") as Style,
				Width = new DataGridLength(10, DataGridLengthUnitType.Star)
			});
		}

		private void OpenCalcButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start("calc.exe");
		}

		// Кнопки в таблице

		private void UpdateRatingButtonClick(object sender, MouseButtonEventArgs e)
		{
			if (CurrentUser?.Group == Type.Administrator)
			{
				((Restaurant) table.SelectedItem).Rating++;
				table.Items.Refresh();
				SaveSystem.SaveRestsToFile();
			}
		}

		private void CheckButtonClick(object sender, MouseButtonEventArgs e)
		{
			switch (table.SelectedItem)
			{
				case Restaurant ritem when CurrentUser?.Group == Type.Administrator:
					var restaurant = Restaurants.First(r => r.Id == ritem.Id);
					restaurant.IsVerified = !restaurant.IsVerified;
					SaveSystem.SaveRestsToFile();
					break;
				case OrderItem oitem when CurrentUser?.Group == Type.Restaurant && oitem.Item.RestId == CurrentRestaurant.Id:
					var orderItem = Orders.Items.First(i => i == oitem);
					orderItem.IsReady = !orderItem.IsReady;
					SaveSystem.SaveOrdersToFile();
					break;
			}

			table.Items.Refresh();
		}

		private void AddButtonClick(object sender, MouseButtonEventArgs e)
		{
			Orders.Add((Dish) table.SelectedItem);
			SaveSystem.SaveOrdersToFile();
		}

		private void RemoveButtonClick(object sender, MouseButtonEventArgs e)
		{
			switch (table.SelectedItem)
			{
				case Dish dish when CurrentUser.Group == Type.Administrator || dish.RestId == CurrentRestaurant.Id:
					Dishes.Remove(dish);
					SaveSystem.SaveDishesToFile();
					break;
				case Restaurant restaurant
					when CurrentUser.Group == Type.Administrator || restaurant.OwnerId == CurrentUser.Id:
					SessionManager.DeleteRestaurant(restaurant);
					break;
				case OrderItem orderItem:
					Orders.Remove(orderItem.Item);
					table.ItemsSource = Orders.Items.Where(i =>
						CurrentUser.Group == Type.User
							? i.UserId == CurrentUser.Id
							: CurrentUser.Group == Type.Administrator || i.Item.RestId == CurrentRestaurant.Id);
					SaveSystem.SaveOrdersToFile();
					break;
				case User user:
					SessionManager.DeleteAccount(user);
					break;
			}

			table.Items.Refresh();
		}
	}
}