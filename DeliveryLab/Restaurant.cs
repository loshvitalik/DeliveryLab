using System.Collections.Generic;

namespace DeliveryLab
{
	public class Restaurant
	{
		private int rating;
		public int Rating
		{
			get { return rating; }
			set
			{
				if (value < 0) rating = 0;
				else if (value > 5) rating = 5;
				else rating = value;
			}
		}
		public string Name { get; private set; }
		public User Owner { get; private set; }
		public List<Dish> Dishes;

		public Restaurant(string name, User owner, int rating, Dish[] dishes)
		{
			Name = name;
			Owner = owner;
			Rating = rating;
			Dishes = new List<Dish>();
			UpdateDishes(dishes);
		}

		public void SetRating(int rating)
		{
			Rating = rating;
		}

		public void UpdateDishes(Dish[] dishes)
		{
			Dishes.Clear();
			foreach (Dish d in dishes)
				Dishes.Add(d);
		}
	}
}
