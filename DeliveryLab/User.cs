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
		public Type Group { get; private set; }
		public string Login { get; private set; }
		public string Password { get; private set; }

		public User(string login, string password, Type group)
		{
			if (group == Type.Administrator)
				group = Type.User;
			Group = group;
			Login = login;
			Password = password;
		}
		
		public User(string login, string password, string masterPassword)
		{
			Group = (masterPassword == "12345") ? Type.Administrator : Type.User;
			Login = login;
			Password = password;
		}
	}
}
