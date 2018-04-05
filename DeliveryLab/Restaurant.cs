using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;
using System.Globalization;

namespace DeliveryLab
{
	public class Restaurant
	{
		private int rating;

		public int Rating
		{
			get => rating;
			set
			{
				if (value < 0) rating = 0;
				else if (value > 5) rating = 5;
				else rating = value;
			}
		}

		public int ID { get; }
		public string Name { get; }
		public int OwnerID { get; }
		public bool IsVerified { get; set; }

		public Restaurant(string name, User owner)
		{
			ID = Restaurants.Any() ? Restaurants.Last().ID + 1 : 0;
			Name = name;
			OwnerID = owner.ID;
			Rating = 0;
		}

		[JsonConstructor]
		private Restaurant(int id, int ownerID, string name, int rating, bool isVerified)
		{
			ID = id;
			OwnerID = ownerID;
			Name = name;
			Rating = rating;
			IsVerified = isVerified;
		}

		public void ReplaceDishes(List<string> dishes)
		{
			foreach (Dish d in Dishes.ToList())
				if (d.RestID == ID)
					Dishes.Remove(d);
			AddDishes(dishes);
		}

		public void AddDishes(List<string> dishes)
		{
			foreach (string d in dishes)
			{
				string[] dish = d.Split(':');
				try
				{
					Dishes.Add(new Dish(ID, Name, dish[0], double.Parse(dish[1], CultureInfo.InvariantCulture)));
				}
				catch (Exception)
				{
					new Alert("Неверный формат", "Блюда были введены в\nневерном формате.\nФормат: название:цена").Show();
				}
			}

			SaveSystem.SaveDishesToFile();
		}
	}
}