using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	public class OrderList
	{
		public ObservableCollection<OrderItem> Items;

		public OrderList()
		{
			Items = new ObservableCollection<OrderItem>();
		}

		public void Add(Dish item)
		{
			var orderItem = Items.FirstOrDefault(i => i.Item == item && i.UserID == CurrentUser.ID);
			if (orderItem != null)
			{
				orderItem.Count++;
				orderItem.Sum += orderItem.Item.Price;
			}
			else
				Items.Add(new OrderItem(item));
		}

		public void Remove(Dish item)
		{
			var orderItem = Items.FirstOrDefault(i =>
				i.Item == item && (i.UserID == CurrentUser.ID || CurrentUser.Group == Type.Administrator));
			if (orderItem != null)
				if (orderItem.Count == 1)
					Items.Remove(orderItem);
				else
				{
					orderItem.Count--;
					orderItem.Sum -= orderItem.Item.Price;
				}
		}
	}

	public class OrderItem
	{
		public int UserID { get; }
		public string UserName { get; }
		public Dish Item { get; }
		public int Count { get; set; }
		public double Sum { get; set; }
		public bool IsReady { get; set; }

		public OrderItem(Dish item)
		{
			UserID = CurrentUser.ID;
			UserName = CurrentUser.Login;
			Item = item;
			Count = 1;
			Sum = item.Price;
		}

		[JsonConstructor]
		private OrderItem(int userID, string userName, Dish item, int count, double sum)
		{
			UserID = userID;
			UserName = userName;
			Item = item;
			Count = count;
			Sum = sum;
		}
	}
}