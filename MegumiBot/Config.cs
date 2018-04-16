using System.IO;
using Newtonsoft.Json;

namespace MegumiBot
{
	// Written by Petrspelos
	public class Config
	{
		private const string ConfigFolder = "Resources";
		private const string ConfigFile = "config.json";

		public static BotConfig Bot;

		static Config()
		{
			if (!Directory.Exists(ConfigFolder))
				Directory.CreateDirectory(ConfigFolder);

			if(!File.Exists(ConfigFolder + "/" + ConfigFile))
			{
				Bot = new BotConfig();
				string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
				File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
			}
			else
			{
				string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
				Bot = JsonConvert.DeserializeObject<BotConfig>(json);
			}
		}

		public struct BotConfig
		{
			[JsonProperty] private string token;
			[JsonIgnore] public string Token => token;

			[JsonProperty] private string defaultPrefix;
			[JsonIgnore] public string DefaultPrefix => defaultPrefix;

			[JsonProperty] private string currencySymbol;
			[JsonIgnore] public string CurrencySymbol => currencySymbol;

			[JsonProperty] private uint autosaveRate;
			[JsonIgnore] public uint AutosaveRate => autosaveRate * 60000;

			[JsonProperty] private string yubooruLocation;
			[JsonIgnore] public string YubooruLocation => yubooruLocation;
		}
	}
}