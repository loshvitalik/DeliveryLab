using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DBConection;
using Binding = System.Windows.Data.Binding;
using DataGrid = System.Windows.Controls.DataGrid;
using TextBox = System.Windows.Controls.TextBox;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private static DataBase dataBase;
		private static DataSet ds;

		public MainWindow()
		{
			InitializeComponent();
		}

		// метод обработки изменения текущей активной таблицы
		private void ShowTable(object sender, SelectionChangedEventArgs e)
		{
			var tableName = tableList.SelectedItem.ToString(); // имя активной таблицы берётся из элемента ListBox
			ds = dataBase.GetTable(tableName); // вызывается метод получения данных нужной таблицы
			table.DataContext = ds;
			table.ItemsSource =
				ds.Tables[0].DefaultView; // устанавливаются контекст и источник данных для элемента DataGrid
			title.Content = tableName; // заголовок окна меняется на название выбранной таблицы
			var columns = ds.Tables[0].Columns; // из класса DataSet выделяется коллекция столбцов таблицы
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
			var data = ((TextBox) e.EditingElement).Text;
			var index = ((DataGrid) sender).ItemContainerGenerator.IndexFromContainer(e.Row);
			var row = (((DataRowView) table.SelectedItem).Row);
			var columnIndex = e.Column.DisplayIndex;
			row[columnIndex] = data;
			dataBase.Update(row, index);
		}

		private void CheckButtonClick(object sender, MouseButtonEventArgs e)
		{
			var columnIndex = table.CurrentCell.Column.DisplayIndex;
			var data = ((DataRowView) table.SelectedItem).Row.ItemArray[columnIndex].ToString();
			var index = table.Items.IndexOf(table.CurrentItem);
			var row = ((DataRowView) table.SelectedItem).Row;
			row[columnIndex] = data;
			dataBase.Update(row, index);
		}

		private void RemoveButtonClick(object sender, MouseButtonEventArgs e)
		{
			var index = table.Items.IndexOf(table.CurrentItem);
			dataBase.DeleteRow(index);
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			if (title.Content == null) return;
			var row = ds.Tables[0].Rows[0];
			new AddItemWindow(dataBase, row, table.Columns).Show();
			table.Items.Refresh();
		}

		public void AddItem(string[] ids)
		{
			var name = title.Content.ToString();
		}

		private void AboutButtonClick(object sender, RoutedEventArgs e)
		{
			new Alert("О программе \"Database Lab\"",
				"Database Lab v. 2.3\n© 2019 loshvitalik, MrBlacktop").Show();
		}
	}
}