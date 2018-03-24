using System.Windows;
using System.Windows.Media;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для ConfirmWindow.xaml
	/// </summary>
	public partial class LogoutWindow : Window
	{
		public LogoutWindow()
		{
			InitializeComponent();
		}

		private void LogOut(object sender, RoutedEventArgs e)
		{
				var w = GetWindow(Application.Current.MainWindow) as MainWindow;
				w.Title = "Delivery Lab";
				w.adminMenu.Visibility = Visibility.Collapsed;
				w.addDishButton.Visibility = Visibility.Collapsed;
			CurrentUser = null;
			w.loginButton.Content = "Войти";
			w.loginButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 59, 48));
			Close();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
