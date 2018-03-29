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
		public int ID { get; private set; }
		public Type Group { get; private set; }
		public string Login { get; private set; }
		public string Password { get; private set; }

		public User(Type group, string login, string password)
		{
			ID = Users.Any() ? Users.Last().ID + 1 : 0;
			Group = group;
			Login = login;
			Password = password;
		}

		[JsonConstructor]
		private User(int id, Type group, string login, string password)
		{
			ID = id;
			Group = group;
			Login = login;
			Password = password;
		}
	}
}
