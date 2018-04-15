using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord;
using MegumiBot.Core.Accounts;
using MegumiBot.Core.GuildAccounts;

namespace MegumiBot
{
	internal static class Global
	{
		internal static DiscordSocketClient Client { get; set; }
		internal static ulong MessageIdToTrack { get; set; }
		public static Random Random = new Random();

		public static Task SaveAll()
		{
			UserAccounts.SaveAccounts();
			Guilds.SaveGuilds();

			return Task.CompletedTask;
		}

		public static string GetNickname(IGuildUser user)
		{
			return user.Nickname ?? user.Username;
		}
	}
}