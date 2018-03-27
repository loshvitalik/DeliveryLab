using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DeliveryLab.MainWindow;

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

		public int ID { get; private set; }
		public string Name { get; private set; }
		public int OwnerID { get; private set; }
		public bool IsVerified { get; private set; }
		public List<Dish> Dishes;

		public Restaurant(string name, User owner)
		{
			ID = Restaurants.Any() ? Restaurants.Last().ID + 1 : 0;
			Name = name;
			OwnerID = owner.ID;
			Rating = 0;
			Dishes = new List<Dish>();
		}

		private Restaurant(int id, int ownerID, string name, int rating, bool isVerified, List<Dish> dishes)
		{
			ID = id;
			OwnerID = ownerID;
			Name = name;
			Rating = rating;
			IsVerified = isVerified;
			Dishes = dishes;
		}

		public void Verify(int rating)
		{
			IsVerified = true;
			Rating = rating;
		}

		public void UpdateDishes(List<Dish> dishes)
		{
			Dishes = dishes;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach (Dish d in Dishes)
				builder.Append(d.ToString() + "|");
			if (builder.Length > 0)
				builder.Remove(builder.Length - 1, 1);
			return ID + "|" + OwnerID + "|" + Name + "|" + Rating + "|" + IsVerified + "|" + builder.ToString();
		}

		public static Restaurant FromString(string str)
		{
			string[] rest = str.Split('|');
			bool isVerified = rest[4].Equals("True");
			List<Dish> dishes = new List<Dish>();
			for (int i = 5; i < rest.Length - 1; i += 2)
				dishes.Add(new Dish(rest[i], double.Parse(rest[i + 1])));
			return new Restaurant(int.Parse(rest[0]), int.Parse(rest[1]), rest[2], int.Parse(rest[3]), isVerified, dishes);
		}
	}
}