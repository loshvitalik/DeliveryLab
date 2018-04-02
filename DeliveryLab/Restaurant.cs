using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
		public bool IsVerified { get; set; }
		public List<Order> Orders;
		public List<Dish> Dishes;

		public Restaurant(string name, User owner)
		{
			ID = Restaurants.Any() ? Restaurants.Last().ID + 1 : 0;
			Name = name;
			OwnerID = owner.ID;
			Rating = 0;
			Orders = new List<Order>();
			Dishes = new List<Dish>();
		}

		[JsonConstructor]
		private Restaurant(int id, int ownerID, string name, int rating, bool isVerified, List<Dish> dishes, List<Order> orders)
		{
			ID = id;
			OwnerID = ownerID;
			Name = name;
			Rating = rating;
			IsVerified = isVerified;
			Dishes = dishes;
			Orders = orders;
		}

		public void UpdateDishes(List<Dish> dishes)
		{
			Dishes = dishes;
			SaveSystem.SaveRestsToFile();
			if (AutoRefresh)
				UpdateMenu();
		}
	}
}