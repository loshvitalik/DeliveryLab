using System.Windows;
using System.Windows.Input;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddRestaurant.xaml
	/// </summary>
	public partial class AddRestaurantWindow : Window
	{
		public AddRestaurantWindow()
		{
			InitializeComponent();
		}

		private void AddRestaurant()
		{
			if (textBox.Text.Contains("|"))
				new Alert("Неверное название",
					"Название не может содержать символ '|'.").Show();
			else
			{
				SessionManager.AddRestaurant(textBox.Text);
				Close();
			}
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			AddRestaurant();
		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
				AddRestaurant();
		}
	}
}
