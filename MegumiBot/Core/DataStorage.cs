using MegumiBot.Core.Accounts;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace MegumiBot.Core
{
	// Written by Petrspelos
	public static class DataStorage
	{
		public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
		{
			var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
		{
			if (!File.Exists(filePath)) return null;
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<UserAccount>>(json);
		}

		public static bool SaveExists(string filePath)
		{
			return File.Exists(filePath);
		}
	}
}