namespace SmartGarage.Utilities
{
	internal class PasswordGenerator
	{
		internal string Generate()
		{
			var randomString = GenerateRandomString();
			var actualPassword = ShuffleString(randomString);

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

		private string ShuffleString(string inputString)
		{
			var inputStringAsList = inputString.ToCharArray().ToList();
			var lenght = inputStringAsList.Count;

			var shuffledStringAsList = new List<char>();

			for (int i = 0; i < lenght; i++)
			{
				var currentSymbol = inputStringAsList[new Random().Next(0, inputStringAsList.Count - 1)];

				shuffledStringAsList.Insert(0, currentSymbol);
				inputStringAsList.Remove(currentSymbol);
			}

			return string.Join("", shuffledStringAsList);
		}
	}
}
