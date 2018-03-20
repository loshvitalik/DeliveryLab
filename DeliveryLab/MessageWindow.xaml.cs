using System.Windows;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MessageWindow.xaml
	/// </summary>
	public partial class MessageWindow : Window
	{
		public MessageWindow()
		{
			InitializeComponent();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
