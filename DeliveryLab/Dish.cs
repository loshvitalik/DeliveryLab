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
		public Restaurant Restaurant { get; private set; }
		public string Name { get; private set; }

		public Dish(Restaurant restaurant, string name, double price)
		{
			Restaurant = restaurant;
			Name = name;
			Price = price;
		}

		public override string ToString()
		{
			return Name + "|" + Price;
		}
	}
}
