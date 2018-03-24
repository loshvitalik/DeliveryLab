using System.Collections.Generic;
using System.Text;

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
		public string Owner { get; private set; }
		public bool IsVerified { get; private set; }
		public List<Dish> Dishes;

		public Restaurant(string name, User owner)
		{
			Name = name;
			Owner = owner.Login;
			Rating = 0;
			Dishes = new List<Dish>();
		}

		public void Verify(int rating)
		{
			IsVerified = true;
			SetRating(rating);
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

		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach (Dish d in Dishes)
				builder.Append(d.ToString() + "\n");
			if (builder.Length > 0)
				builder.Remove(builder.Length - 1, 1);
			return Owner + "|" + Name + "|" + Rating + "\n" + builder.ToString();
		}
	}
}
