using Discord.WebSocket;
using System;
using Discord;

namespace MegumiBot
{
	internal static class Global
	{
		internal static DiscordSocketClient Client { get; set; }
		internal static ulong MessageIdToTrack { get; set; }
		public static Random Random = new Random();

		public static string GetNickname(IGuildUser user)
		{
			return user.Nickname ?? user.Username;
		}
	}
}