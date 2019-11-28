using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DBConection;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddItemWindow.xaml
	/// </summary>
	public partial class AddItemWindow
	{
		// 4-10
		private DataBase dataBase;
		private DataRow row;
		private ObservableCollection<DataGridColumn> columns;

		public AddItemWindow(DataBase dataBase, DataRow row, ObservableCollection<DataGridColumn> columns)
		{
			InitializeComponent();
			this.dataBase = dataBase;
			this.row = row;
			this.columns = columns;
			label0.Content = columns[0].Header;
			label1.Content = columns[1].Header;
			label2.Content = columns[2].Header;
			label3.Content = columns[3].Header;
			if (columns.Count >= 6)
			{
				label4.Content = columns[4].Header;
				label5.Content = columns[5].Header;
				textBox4.Visibility = Visibility.Visible;
				textBox5.Visibility = Visibility.Visible;
			}

			if (columns.Count >= 9)
			{
				label6.Content = columns[6].Header;
				label7.Content = columns[7].Header;
				label8.Content = columns[8].Header;
				textBox6.Visibility = Visibility.Visible;
				textBox7.Visibility = Visibility.Visible;
				textBox8.Visibility = Visibility.Visible;
			}

			if (columns.Count == 10)
			{
				label9.Content = columns[9].Header;
				textBox9.Visibility = Visibility.Visible;
			}
		}

		private void EnterKeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				AddButtonClick(null, null);
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			var items = GetValues();
			for (var i = 0; i < items.Length; i++)
			{
				if (row[i] is DateTime)
					row[i] = DateTime.ParseExact(items[i], "dd.mm.yyyy", null);
				else
					row[i] = items[i];
			}

			dataBase.AddRow(row);
		}

		private string[] GetValues()
		{
			var values = new List<string> {textBox0.Text, textBox1.Text, textBox2.Text, textBox3.Text};
			if (columns.Count >= 6)
			{
				values.Add(textBox4.Text);
				values.Add(textBox5.Text);
			}

			if (columns.Count >= 9)
			{
				values.Add(textBox6.Text);
				values.Add(textBox7.Text);
				values.Add(textBox8.Text);
			}

			if (columns.Count == 10)
			{
				values.Add(textBox9.Text);
			}

			return values.ToArray();
		}
	}
}