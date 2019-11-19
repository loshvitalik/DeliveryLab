using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

		// метод обработки изменения текущей активной таблицы
		private void ShowTable(object sender, SelectionChangedEventArgs e)
		{
			var tableName = tableList.SelectedItem.ToString(); // имя активной таблицы берётся из элемента ListBox
			var data = dataBase.GetTable(tableName); // вызывается метод получения данных нужной таблицы
			table.DataContext = data;
			table.ItemsSource =
				data.Tables[0].DefaultView; // устанавливаются контекст и источник данных для элемента DataGrid
			title.Content = tableName; // заголовок окна меняется на название выбранной таблицы
			var columns = data.Tables[0].Columns; // из класса DataSet выделяется коллекция столбцов таблицы
			var columnWidth = 100.0 / columns.Count; // все столбцы получают одинаковую равную ширину
			table.Columns.Clear(); // элемент DataGrid очищается от предыдущих данных
			// для каждого столбца из ранее полученного списка создаётся столбец в элементе DataGrid,
			// заголовком столбца становится его название и выполоняется привязка к соответствующим данным
			foreach (DataColumn column in columns)
			{
				if (column.DataType == typeof(bool)) // для логических значений БД создаётся CheckBoxColumn
				{
					table.Columns.Add(new DataGridCheckBoxColumn
					{
						Header = column.ColumnName,
						Binding = new Binding(column.ColumnName),
						Width = new DataGridLength(columnWidth, DataGridLengthUnitType.Star),
						CellStyle = FindResource("checkCell") as Style
					});
				}
				else // для всех остальных - TextColumn
				{
					table.Columns.Add(new DataGridTextColumn
					{
						Header = column.ColumnName,
						Binding = new Binding(column.ColumnName),
						Width = new DataGridLength(columnWidth, DataGridLengthUnitType.Star)
					});
				}
			}

			table.Columns.Add(new DataGridTextColumn
			{
				Header = "remove",
				Width = new DataGridLength(columnWidth, DataGridLengthUnitType.Star),
				CellStyle = FindResource("removeCell") as Style
			});
		}

		// метод соединения с БД
		private void ConnectButtonClick(object sender, RoutedEventArgs e)
		{
			dataBase = new DataBase(); // создаётся новый экземпяр класса DataBase
			var tableNames = dataBase.getTableNames(); // вызывается метод получения названий всех таблиц
			tableList.ItemsSource = tableNames; // этот список названий помещается в элемент ListBox
			connectButton.Content = "udb__Аверьянов__Лощенко"; // заголовок окна меняется на название базы данных
		}

		private void DataGridTextEdited(object sender, DataGridCellEditEndingEventArgs e)
		{
			var name = title.Content.ToString();
			var column = e.Column.Header.ToString();
			var data = ((TextBox) e.EditingElement).Text;
			dataBase.UpdateItem(name, GetItemId(), column, data);
		}

		private void CheckButtonClick(object sender, MouseButtonEventArgs e)
		{
			var name = title.Content.ToString();
			var column = table.CurrentCell.Column.Header.ToString();
			var index = table.CurrentCell.Column.DisplayIndex;
			var data = ((DataRowView) table.SelectedItem).Row.ItemArray[index].ToString();
			dataBase.UpdateItem(name, GetItemId(), column, data);
		}

		private void RemoveButtonClick(object sender, MouseButtonEventArgs e)
		{
			var name = title.Content.ToString();
			dataBase.DeleteItem(name, GetItemId());
			table.Items.Refresh();
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			if (title.Content == null) return;
			new AddItemWindow().Show();
			table.Items.Refresh();
		}

		public void AddItem(string[] ids)
		{
			var name = title.Content.ToString();
			dataBase.AddItem(name, ids);
			table.Items.Refresh();
		}

		private string[] GetItemId()
		{
			var id = ((DataRowView) table.SelectedItem).Row.ItemArray[0].ToString();
			return title.Content.ToString() == "title_author" ? new[] {id, ((DataRowView) table.SelectedItem).Row.ItemArray[1].ToString()} : new[] {id};
		}

		private void AboutButtonClick(object sender, RoutedEventArgs e)
		{
			new Alert("О программе \"Database Lab\"",
				"Database Lab v. 2.2\n© 2019 loshvitalik, MrBlacktop").Show();
		}
	}
}