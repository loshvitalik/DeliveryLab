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
				loginBox.Text.Contains("{") || loginBox.Text.Contains("}") || loginBox.Text.Contains("[") || loginBox.Text.Contains("]"))
				new Alert("Неверный логин",
					"Логин не может содержать символы\n\",\", \":\", \", \"{\", \"}\", \"[\", \"]\"").Show();
			else
			{
				var group = Type.User;
				if (restCheckBox.IsChecked ?? false) group = Type.Restaurant;
				SessionManager.RegisterUser(group, loginBox.Text, passBox.Password);
			}
		}

		private void CheckIfRegistered()
		{
			bool userFound = false;
			foreach (User u in Users)
				if (loginBox.Text == u.Login)
					userFound = true;
			if (userFound)
			{
				regButton.Visibility = Visibility.Collapsed;
				restCheckBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				regButton.Visibility = Visibility.Visible;
				restCheckBox.Visibility = Visibility.Visible;
			}
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