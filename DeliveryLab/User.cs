using System.Security.Cryptography;
using System.Text;

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
			Group = group;
			Login = login;
			Password = EncryptString(password);
		}

		public override string ToString()
		{
			return Group + "|" + Login + "|" + Password;
		}

		public static string EncryptString(string password)
		{
			byte[] hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(password));
			var builder = new StringBuilder();
			foreach (var b in hash)
				builder.Append(b.ToString("x2"));
			return builder.ToString();
		}
	}
}
