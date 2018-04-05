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
		public User(Type group, string login, string password)
		{
			Id = Users.Any() ? Users.Last().Id + 1 : 0;
			Group = group;
			Login = login;
			Password = password;
		}

		[JsonConstructor]
		public User(int id, Type group, string login, string password)
		{
			Id = id;
			Group = group;
			Login = login;
			Password = password;
		}

		public int Id { get; }
		public Type Group { get; }
		public string Login { get; }
		public string Password { get; set; }
	}
}