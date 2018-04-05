using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddDishesWindow.xaml
	/// </summary>
	public partial class AddDishesWindow
	{
		public AddDishesWindow()
		{
			InitializeComponent();
		}

		private List<string> GetStingsFromText()
		{
			var text = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
			return text.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		private void ReplaceButtonClick(object sender, RoutedEventArgs e)
		{
			CurrentRestaurant.ReplaceDishes(GetStingsFromText());
			Close();
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			CurrentRestaurant.AddDishes(GetStingsFromText());
			Close();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}