using System;
using System.Windows.Navigation;

namespace DeliveryLab
{
	public class Dish : IComparable
	{
		private double price;

		public double Price
		{
			get => price;
			private set
			{
				if (value < 0) price = 0;
				price = value;
			}
		}

		public int RestID { get; }
		public string RestName { get; }
		public string Name { get; }

		public Dish(int restID, string restName, string name, double price)
		{
			RestID = restID;
			RestName = restName;
			Name = name;
			Price = price;
		}

		public int CompareTo(object obj)
		{
			Dish dish = obj as Dish;
			if (RestID == dish?.RestID && Name == dish.Name && Price == dish.Price) return 0;
			return -1;
		}
	}
}