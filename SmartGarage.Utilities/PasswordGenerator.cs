namespace SmartGarage.Utilities
{
	internal class PasswordGenerator
	{
		internal string Generate()
		{
			var randomString = GenerateRandomString();
			var password = ShuffleString(randomString);

			return password;
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

		private string ShuffleString(string stringToShuffle)
		{
			var stringToShuffleAsList = inputString.ToCharArray().ToList();
			var lenght = stringToShuffleAsList.Count;

			var resultAsList = new List<char>();

			for (int i = 0; i < lenght; i++)
			{
				var currentSymbol = stringToShuffleAsList[new Random().Next(0, stringToShuffleAsList.Count - 1)];

				resultAsList.Insert(0, currentSymbol);
				stringToShuffleAsList.Remove(currentSymbol);
			}

			return string.Join("", resultAsList);

		}
	}
}
