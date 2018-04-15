using System;
using Discord.WebSocket;
using MegumiBot.Core.Accounts;

namespace MegumiBot.Core
{
	internal static class Leveling
	{
		internal static async void MessageReceived(SocketGuildUser user, SocketTextChannel channel)
		{
			var userAccount = UserAccounts.GetAccount(user);

			userAccount.TotalMessages++;

			if (Math.Abs(userAccount.LastCountedMessageTime.Minute - DateTime.Now.Minute) < 1) return;

			userAccount.LastCountedMessageTime = DateTime.Now;
			var oldLevel = userAccount.LevelNumber;
			userAccount.Xp += (uint)Global.Random.Next(15, 26);

			if (oldLevel != userAccount.LevelNumber)
			{
				await channel.SendMessageAsync($"{Global.GetNickname(user)} is now level {userAccount.LevelNumber}!");
			}
		}
	}
}
