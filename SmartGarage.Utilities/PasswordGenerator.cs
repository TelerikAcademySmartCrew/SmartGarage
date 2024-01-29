namespace SmartGarage.Utilities
{
	internal class PasswordGenerator
	{
		internal string Generate()
		{
			var passwordSymbols = GenerateRandomString();
			var actualPassword = ShuffleString(passwordSymbols);

			return actualPassword;
		}

		private string GenerateRandomString()
		{
			var uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
			var specialSymbols = "!@#$%&";
			var numbers = "0123456789";

			var result = string.Empty;

			result += GenerateRandomStringFromString(uppercaseLetters);
			result += GenerateRandomStringFromString(lowercaseLetters);
			result += GenerateRandomStringFromString(specialSymbols);
			result += GenerateRandomStringFromString(numbers);

			return result;
		}

		private string GenerateRandomStringFromString(string source)
		{
			var result = string.Empty;
			var lenght = new Random().Next(2, 4);

			for (int i = 0; i < lenght; i++)
			{
				result += source[new Random().Next(0, source.Length - 1)];
			}

			return result;
		}

		private string ShuffleString(string password)
		{
			var passwordList = password.ToCharArray().ToList();
			var lenght = passwordList.Count;

			var actualPasswordList = new List<char>();

			for (int i = 0; i < lenght; i++)
			{
				var currentSymbol = passwordList[new Random().Next(0, passwordList.Count - 1)];

				actualPasswordList.Insert(0, currentSymbol);
				passwordList.Remove(currentSymbol);
			}

			return string.Join("", actualPasswordList);

		}
	}
}
