using System.Windows;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MessageWindow.xaml
	/// </summary>
	public partial class Alert
	{
		public Alert(string title, string content)
		{
			InitializeComponent();
			Title = title;
			label.Content = content;
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}