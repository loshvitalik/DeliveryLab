using System.Windows;
using System.Windows.Input;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddRestaurant.xaml
	/// </summary>
	public partial class AddRestaurant : Window
	{
		public AddRestaurant()
		{
			InitializeComponent();
		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				SessionManager.AddRestaurant(textBox.Text);
				Close();
			}
		}
		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.AddRestaurant(textBox.Text);
			Close();
		}
	}
}
