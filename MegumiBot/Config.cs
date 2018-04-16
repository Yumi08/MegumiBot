using System.IO;
using Newtonsoft.Json;

namespace MegumiBot
{
	// Written by Petrspelos
	public class Config
	{
		private const string ConfigFolder = "Resources";
		private const string ConfigFile = "config.json";

		private static BotConfig _bot;

		public static MegumiBot.BotConfig Bot;

		static Config()
		{
			if (!Directory.Exists(ConfigFolder))
				Directory.CreateDirectory(ConfigFolder);

			if(!File.Exists(ConfigFolder + "/" + ConfigFile))
			{
				_bot = new BotConfig();
				string json = JsonConvert.SerializeObject(_bot, Formatting.Indented);
				File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
			}
			else
			{
				string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
				_bot = JsonConvert.DeserializeObject<BotConfig>(json);
			}

			Bot.Bot = _bot;
		}

		public struct BotConfig
		{
			public string Token;
			public string DefaultPrefix;
			public string CurrencySymbol;
			public uint AutosaveRate;
		}
	}

	public struct BotConfig
	{
		public Config.BotConfig Bot;

		public string Token => Bot.Token;
		public string DefaultPrefix => Bot.DefaultPrefix;
		public string CurrencySymbol => Bot.CurrencySymbol;
		public uint AutosaveRate => Bot.AutosaveRate * 60000;
	}
}