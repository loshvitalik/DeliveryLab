using System;

namespace DeliveryLab
{
	public class Dish
	{
		private double price;
		public double Price {
			get { return price; }
			private set
			{
				if (value < 0) throw new ArgumentException();
				else price = value;
			}
		}
		public string Name { get; private set; }

		public Dish( string name, double price)
		{
			Name = name;
			Price = price;
		}
	}
}
