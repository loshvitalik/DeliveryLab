using System.Windows;
using System.Windows.Input;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddRestaurant.xaml
	/// </summary>
	public partial class AddRestaurantWindow
	{
		public AddRestaurantWindow()
		{
			InitializeComponent();
		}

		private void AddRestaurant()
		{
			if (textBox.Text.Contains(",") || textBox.Text.Contains(":") || textBox.Text.Contains("\"") ||
			    textBox.Text.Contains("{") || textBox.Text.Contains("}") || textBox.Text.Contains("[") ||
			    textBox.Text.Contains("]"))
				new Alert("Неверный логин",
					"Логин не может содержать символы\n\",\", \":\", \", \"{\", \"}\", \"[\", \"]\"").Show();
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