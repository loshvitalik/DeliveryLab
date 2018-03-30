using System.Windows;
using System.Windows.Input;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для ChangePasswordWindow.xaml
	/// </summary>
	public partial class ChangePasswordWindow : Window
	{
		public ChangePasswordWindow()
		{
			InitializeComponent();
		}

		private void ChangePassword()
		{
			SessionManager.ChangePassword(oldPass.Password, newPass.Password);
			Close();
		}

		private void ChangePasswordButtonClick(object sender, RoutedEventArgs e)
		{
			ChangePassword();
		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
				ChangePassword();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
