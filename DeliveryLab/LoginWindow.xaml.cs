using System.Linq;
using System.Windows;
using System.Windows.Input;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow
	{
		public LoginWindow()
		{
			InitializeComponent();
		}

		private void RegisterUser(object sender, RoutedEventArgs e)
		{
			if (loginBox.Text.Contains(",") || loginBox.Text.Contains(":") || loginBox.Text.Contains("\"") ||
			    loginBox.Text.Contains("{") || loginBox.Text.Contains("}") || loginBox.Text.Contains("[") ||
			    loginBox.Text.Contains("]"))
			{
				new Alert("Неверный логин",
					"Логин не может содержать символы\n\",\", \":\", \"{\", \"}\", \"[\", \"]\" и \"").Show();
			}
			else if (Users.Any(u => loginBox.Text == u.Login))
			{
				new Alert("Неверный логин",
					"Такой пользователь уже\nзарегистрирован").Show();
			}
			else
			{
				var group = Type.User;
				if (restCheckBox.IsChecked ?? false) group = Type.Restaurant;
				SessionManager.RegisterUser(group, loginBox.Text, passBox.Password);
			}
		}

		private void CheckIfRegistered()
		{
		}

		private void LoginKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
				SessionManager.LogIn(loginBox.Text, passBox.Password);
			else
				CheckIfRegistered();
		}

		private void PassEnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
				SessionManager.LogIn(loginBox.Text, passBox.Password);
		}

		private void LoginUserButtonClick(object sender, RoutedEventArgs e)
		{
			SessionManager.LogIn(loginBox.Text, passBox.Password);
		}

		private void LoginBoxLostFocus(object sender, RoutedEventArgs e)
		{
			CheckIfRegistered();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}