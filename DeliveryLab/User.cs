using System.Linq;
using Newtonsoft.Json;
using static DeliveryLab.MainWindow;

namespace DeliveryLab
{
	public enum Type
	{
		Administrator,
		User,
		Restaurant
	}

	public class User
	{
		public int ID { get; }
		public Type Group { get; }
		public string Login { get; }
		public string Password { get; set; }

		public User(Type group, string login, string password)
		{
			ID = Users.Any() ? Users.Last().ID + 1 : 0;
			Group = group;
			Login = login;
			Password = password;
		}

		[JsonConstructor]
		public User(int id, Type group, string login, string password)
		{
			ID = id;
			Group = group;
			Login = login;
			Password = password;
		}
	}
}
