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

		/// <summary>
		/// Save all the current bot variables such as user accounts and guilds.
		/// </summary>
		/// <returns></returns>
		public static Task SaveAll()
		{
			UserAccounts.SaveAccounts();
			Guilds.SaveGuilds();

			return Task.CompletedTask;
		}

		/// <summary>
		/// Get a user's nickname, else return their username.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static string GetNickname(IGuildUser user)
		{
			return user.Nickname ?? user.Username;
		}

		/// <summary>
		/// Wait for a user to send a message for a certain amount of time
		/// </summary>
		/// <param name="userId">The target user's ID</param>
		/// <param name="channelId">The target channel's ID</param>
		/// <param name="delayInMs">The delay until it stops waiting for a message</param>
		/// <returns>The message that the user sent</returns>
		public static async Task<SocketMessage> AwaitMessage(ulong userId, ulong channelId, int delayInMs)
		{
			SocketMessage response = null;
			var canceler = new CancellationTokenSource();
			var waiter = Task.Delay(delayInMs, canceler.Token);

			Client.MessageReceived += OnMessageReceived;
			try { await waiter; }
			catch (TaskCanceledException) {}
			Client.MessageReceived -= OnMessageReceived;
			return response;

			Task OnMessageReceived(SocketMessage message)
			{
				if (message.Author.Id != userId || message.Channel.Id != channelId)
					return Task.CompletedTask;

				response = message;
				canceler.Cancel();
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Wait for the user to say Y or N for a certain amount of time
		/// </summary>
		/// <param name="userId">The target user's ID</param>
		/// <param name="channelId">The target channel's ID</param>
		/// <param name="delayInMs">The delay until it stops waiting for a message</param>
		/// <returns>Whether the user said Y or N</returns>
		public static async Task<bool> AwaitYesNoMessage(ulong userId, ulong channelId, int delayInMs)
		{
			var response = await AwaitMessage(userId, channelId, delayInMs);

			if (response == null) return false;

			switch (response.Content.ToLower())
			{
				case "y":
					return true;

				case "n":
					return false;

				default:
					return false;
			}
		}
	}
}