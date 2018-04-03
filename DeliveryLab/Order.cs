using System.Collections.ObjectModel;
using System.Linq;

namespace DeliveryLab
{
	public class Order
	{
		public ObservableCollection<OrderItem> Items;

		public Order()
		{
			Items = new ObservableCollection<OrderItem>();
		}

		public void Add(Dish item)
		{
			var orderItem = Items.Where(i => i.Item == item).FirstOrDefault();
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
			var orderItem = Items.Where(i => i.Item == item).FirstOrDefault();
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
		public Dish Item { get; set; }
		public int Count { get; set; }
		public double Sum { get; set; }
		public bool IsReady { get; set; }

		public OrderItem(Dish item)
		{
			Item = item;
			Count = 1;
			Sum = item.Price;
		}
	}
}
