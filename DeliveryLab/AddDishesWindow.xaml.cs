using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using static DeliveryLab.MainWindow;
using Excel = Microsoft.Office.Interop.Excel.Application;

namespace DeliveryLab
{
	/// <summary>
	/// Логика взаимодействия для AddDishesWindow.xaml
	/// </summary>
	public partial class AddDishesWindow
	{
		public AddDishesWindow()
		{
			InitializeComponent();
		}

		private List<string> GetStringsFromText()
		{
			var text = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
			return text.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		private void ReplaceButtonClick(object sender, RoutedEventArgs e)
		{
			CurrentRestaurant.ReplaceDishes(GetStringsFromText());
			Close();
		}

		private void AddButtonClick(object sender, RoutedEventArgs e)
		{
			CurrentRestaurant.AddDishes(GetStringsFromText());
			Close();
		}

		private void ParseDishesFromExcel(string fileName)
		{
			textBox.Document.Blocks.Clear();
			var exApp = new Excel();
			var exSheet = (Worksheet) exApp.Workbooks.Open(fileName).Sheets[1];
			for (var i = 1; exSheet.Range["A" + i, "A" + i].Value != null; i++)
				textBox.Document.Blocks.Add(
					new Paragraph(new Run(exSheet.Range["A" + i, "A" + i].Value + ":" + exSheet.Range["B" + i, "B" + i].Value)));
			exApp.Quit();
		}

		private void CloseWindow(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void LoadFromExcelButtonClick(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
				Filter = "Книги Microsoft Excel(*.xls;*.xlsx)|*.xls;*.xlsx",
				Title = "Загрузить из книги Microsoft Excel"
			};
			if (dialog.ShowDialog() == true)
				ParseDishesFromExcel(dialog.FileName);
		}
	}
}