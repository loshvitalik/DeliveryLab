using System.Windows;
using System.Windows.Input;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddItemWindow.xaml
	/// </summary>
	public partial class AddItemWindow
	{
		public AddItemWindow()
		{
			InitializeComponent();

		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				AddButtonClick(null, null);
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			var ids = textBox.Text.Split(';');
			((MainWindow)Application.Current.MainWindow).AddItem(ids);
			
		}
	}
}
