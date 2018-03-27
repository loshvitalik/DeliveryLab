using System.Linq;
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

		public override string ToString()
		{
			return ID + "|" + Group + "|" + Login + "|" + Password;
		}

		public static User FromString(string str)
		{
			string[] user = str.Split('|');
			Type group = user[1].Equals("Administrator") ? Type.Administrator :
				user[1].Equals("Restaurant") ? Type.Restaurant : Type.User;
			return new User(group, user[2], user[3]);
		}
	}
}
