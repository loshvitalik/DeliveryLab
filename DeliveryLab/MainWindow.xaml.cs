using System;
using System.Collections.Generic;
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

		private void LoginButtonClick(object sender, RoutedEventArgs e)
		{
			if (CurrentUser == null)
				new LoginWindow().Show();
			else
				SessionManager.LogOut();
		}

		// Левая колонка
		public void ShowMenu(object sender, RoutedEventArgs e)
		{
			title.Content = "Меню";
			table.ItemsSource = Dishes;
			addRowToOrder.Visibility = CurrentUser?.Group != Type.Restaurant ? Visibility.Visible : Visibility.Collapsed;
			deleteRowButton.Visibility = CurrentUser?.Group == Type.Administrator ? Visibility.Visible : Visibility.Collapsed;
			table.Columns.Clear();
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Ресторан",
				Binding = new Binding("RestName"),
				Width = new DataGridLength(25, DataGridLengthUnitType.Star),
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
			deleteRowButton.Visibility = CurrentUser?.Group != Type.User ? Visibility.Visible : Visibility.Collapsed;
			table.Columns.Clear();
			table.Columns.Add(new DataGridCheckBoxColumn()
			{
				Header = "Проверен",
				Binding = new Binding("IsVerified"),
				Width = new DataGridLength(15, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(
				new DataGridTextColumn()
				{
					Header = "Рейтинг",
					Binding = new Binding("Rating"),
					Width = new DataGridLength(35, DataGridLengthUnitType.Star),
				});
		}

		private void ShowOrder(object sender, RoutedEventArgs e)
		{
			table.ItemsSource = Orders.Items.Where(i =>
				(CurrentUser.Group == Type.User)
					? i.UserID == CurrentUser.ID
					: (CurrentUser.Group != Type.Restaurant) || i.Item.RestID == CurrentRestaurant.ID);
			title.Content = CurrentUser.Group == Type.User ? "Заказ" : "Заказы";
			addRowToOrder.Visibility = Visibility.Collapsed;
			deleteRowButton.Visibility = Visibility.Visible;
			table.Columns.Clear();
			table.Columns.Add(new DataGridCheckBoxColumn()
			{
				Header = "Готовность",
				Binding = new Binding("IsReady"),

				Width = new DataGridLength(15, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Название",
				Binding = new Binding("Item.Name"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Кол-во",
				Binding = new Binding("Count"),
				Width = new DataGridLength(10, DataGridLengthUnitType.Star),
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
				Width = new DataGridLength(15, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Логин",
				Binding = new Binding("Login"),
				Width = new DataGridLength(50, DataGridLengthUnitType.Star),
			});
			table.Columns.Add(new DataGridTextColumn()
			{
				Header = "Группа",
				Binding = new Binding("Group"),
				Width = new DataGridLength(35, DataGridLengthUnitType.Star),
			});
		}

		private void AddNewDish(object sender, RoutedEventArgs e)
		{
			if (CurrentUser?.Group == Type.Restaurant)
				CurrentRestaurant.AddDishes(new List<string>()
					{"Рандомная еда|" + new Random().Next(0, 10000)});
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

		private void ClearOrdersButtonClick(object sender, RoutedEventArgs e)
		{
			SaveSystem.ClearOrders();
		}

		private void ShowChangePasswordWindow(object sender, RoutedEventArgs e)
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

		private void TableLeftClick(object sender, MouseButtonEventArgs e)
		{
			if (title.Content.ToString() == "Заказы" && CurrentUser.Group != Type.User)
			{
				var selectedItem = table.SelectedItem;
				if (selectedItem is OrderItem item)
				{
					var orderItem = Orders.Items.First(i => i == item);
					orderItem.IsReady = !orderItem.IsReady;
					table.Items.Refresh();
					SaveSystem.SaveOrdersToFile();
				}
			}
		}

		private void AddRowToOrderClick(object sender, RoutedEventArgs e)
		{
			Orders.Add((Dish) table.SelectedItem);
			table.Items.Refresh();
			SaveSystem.SaveOrdersToFile();
		}

		private void DeleteRowButtonClick(object sender, RoutedEventArgs e)
		{
			var selectedItem = table.SelectedItem;
			if (selectedItem is Dish dish && CurrentUser.Group == Type.Administrator)
			{
				Dishes.Remove(dish);
				SaveSystem.SaveDishesToFile();
			}

			if (selectedItem is Restaurant restaurant &&
			    (CurrentUser.Group == Type.Administrator ||
			     CurrentUser.Group == Type.Restaurant && CurrentRestaurant.OwnerID == CurrentUser.ID))
			{
				SessionManager.DeleteRestaurant(restaurant);
				SaveSystem.SaveAll();
			}

			if (selectedItem is OrderItem orderItem && CurrentUser.Group != Type.Restaurant)
			{
				Orders.Remove(orderItem.Item);
				SaveSystem.SaveOrdersToFile();
			}

			if (selectedItem is User user && CurrentUser.Group == Type.Administrator)
			{
				SessionManager.DeleteAccount(user);
				SaveSystem.SaveAll();
			}

			table.Items.Refresh();
		}

		private void CloseAppButtonClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}