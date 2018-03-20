using System.Windows;
using System.Windows.Input;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{
			InitializeComponent();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void RegisterUser(object sender, RoutedEventArgs e)
		{
			var user = new User(loginBox.Text, passBox.Password, Type.User);
			MainWindow.Users.Add(user);
			MainWindow.LoginUser(loginBox.Text, passBox.Password);
			Close();
		}

		private void LoginUserButtonClick(object sender, RoutedEventArgs e)
		{
			MainWindow.LoginUser(loginBox.Text, passBox.Password);
			Close();
		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				MainWindow.LoginUser(loginBox.Text, passBox.Password);
				Close();
			}
		}

	}
}