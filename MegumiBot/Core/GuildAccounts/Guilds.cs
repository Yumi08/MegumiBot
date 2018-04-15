using System.Collections.Generic;
using System.Linq;
using Discord;

namespace MegumiBot.Core.GuildAccounts
{
	public static class Guilds
	{
		private static List<Guild> _guilds;

		private static string _guildsFile = "Resources/guilds.json";

		static Guilds()
		{
			if(DataStorage<Guild>.SaveExists(_guildsFile))
			{
				_guilds = DataStorage<Guild>.LoadItems(_guildsFile).ToList();
			}
			else
			{
				_guilds = new List<Guild>();
				SaveGuilds();
			}
		}

		public static void SaveGuilds()
		{
			DataStorage<Guild>.SaveItems(_guilds, _guildsFile);
		}
		
		public static Guild GetGuild(IGuild guild)
		{
			return GetOrCreateGuild(guild.Id);
		}

		private static Guild GetOrCreateGuild(ulong id)
		{
			var result = from a in _guilds
				where a.Id == id
				select a;

			var account = result.FirstOrDefault() ?? CreateGuild(id);
			return account;
		}

		private static Guild CreateGuild(ulong id)
		{
			var newAccount = new Guild()
			{
				Id = id,
			};

			_guilds.Add(newAccount);
			return newAccount;
		}
	}
}
