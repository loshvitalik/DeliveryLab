using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	public class Restaurant
	{ 
		public Restaurant(string name, User owner)
		{
			Id = Restaurants.Any() ? Restaurants.Last().Id + 1 : 0;
			Name = name;
			OwnerId = owner.Id;
			Rating = 0;
		}

		[JsonConstructor]
		private Restaurant(int id, int ownerId, string name, int rating, bool isVerified)
		{
			Id = id;
			OwnerId = ownerId;
			Name = name;
			Rating = rating;
			IsVerified = isVerified;
		}

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

		public int Id { get; }
		public string Name { get; }
		public int OwnerId { get; }
		public bool IsVerified { get; set; }

		public void ReplaceDishes(List<string> dishes)
		{
			foreach (var d in Dishes.ToList())
				if (d.RestId == Id)
					Dishes.Remove(d);
			AddDishes(dishes);
		}

		public void AddDishes(List<string> dishes)
		{
			foreach (var d in dishes)
			{
				var dish = d.Split(':');
				try
				{
					Dishes.Add(new Dish(Id, Name, dish[0], double.Parse(dish[1], CultureInfo.InvariantCulture)));
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