using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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

		public static bool CheckYN(string input)
		{
			switch (input.ToLower())
			{
				case "y":
					return true;
				case "n":
					return false;
				default:
					return false;
			}
		}

		public static async Task<string> AwaitMessage(SocketCommandContext context, int delay)
		{
			string response = null;

			var canceler = new CancellationTokenSource();
			var waiter = Task.Delay(delay, canceler.Token);

			context.Client.MessageReceived += OnMessageReceived;

			try { await waiter; }
			// Calls the catch block whenever it's cancelled
			catch
			{
				context.Client.MessageReceived -= OnMessageReceived;
				return response;
			}

			context.Client.MessageReceived -= OnMessageReceived;
			return response;

			Task OnMessageReceived(SocketMessage message)
			{
				if (message.Author.Id != context.User.Id ||
				    message.Channel.Id != context.Channel.Id)
					return Task.CompletedTask;

				response = message.Content;
				canceler.Cancel();
				return Task.CompletedTask;
			}
		}
	}
}