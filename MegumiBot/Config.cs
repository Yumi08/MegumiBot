using System.IO;
using Newtonsoft.Json;

namespace MegumiBot
{
	// Written by Petrspelos
	class Config
	{
		private const string ConfigFolder = "Resources";
		private const string ConfigFile = "config.json";

		public static BotConfig bot;

		static Config()
		{
			if (!Directory.Exists(ConfigFolder))
				Directory.CreateDirectory(ConfigFolder);

			if(!File.Exists(ConfigFolder + "/" + ConfigFile))
			{
				bot = new BotConfig();
				string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
				File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
			}
			else
			{
				string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
				bot = JsonConvert.DeserializeObject<BotConfig>(json);
			}
		}
	}

	public struct BotConfig
	{
		public string Token;
		public string CmdPrefix;
	}
}