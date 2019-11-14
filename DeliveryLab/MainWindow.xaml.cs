using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DBConection;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private static DataBase dataBase;

		public MainWindow()
		{
			InitializeComponent();
			
		}

		private void ShowTable(object sender, SelectionChangedEventArgs e)
		{
			var tableName = tableList.SelectedItem.ToString();
			var data = dataBase.GetTable(tableName);
			table.DataContext = data;
			table.ItemsSource = data.Tables[0].DefaultView;
			title.Content = tableName;
			var columns = data.Tables[0].Columns;
			var columnWidth = 100.0 / columns.Count;
			table.Columns.Clear();
			foreach (DataColumn column in columns)
			{
				if (column.DataType == typeof(bool))
				{
					table.Columns.Add(new DataGridCheckBoxColumn
					{
						Header = column.ColumnName,
						Binding = new Binding(column.ColumnName),
						Width = new DataGridLength(columnWidth, DataGridLengthUnitType.Star)
					});
				}
				else
				{
					table.Columns.Add(new DataGridTextColumn
					{
						Header = column.ColumnName,
						Binding = new Binding(column.ColumnName),
						Width = new DataGridLength(columnWidth, DataGridLengthUnitType.Star)
					});
				}
			}
		}

		private void ConnectButtonClick(object sender, RoutedEventArgs e)
		{
			dataBase = new DataBase();
			var tableNames = dataBase.getTableNames();
			tableList.ItemsSource = tableNames;
			tableList.SelectedItem = tableNames.FirstOrDefault();

			ShowTable("", null);
		}

		private void AboutButtonClick(object sender, RoutedEventArgs e)
		{
			new Alert("О программе \"Database Lab\"",
				"Database Lab v. 2.0\n© 2019 loshvitalik, MrBlacktop").Show();
		}
	}
}