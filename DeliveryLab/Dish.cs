namespace DeliveryLab
{
	public class Dish
	{
		public Dish(int restId, string restName, string name, double price)
		{
			RestId = restId;
			RestName = restName;
			Name = name;
			Price = price;
		}

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

		public int RestId { get; }
		public string RestName { get; }
		public string Name { get; }
	}
}