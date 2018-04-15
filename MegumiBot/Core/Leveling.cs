using System;
using Discord.WebSocket;
using MegumiBot.Core.Accounts;

namespace MegumiBot.Core
{
	internal static class Leveling
	{
		internal static async void MessageReceived(SocketGuildUser user, SocketTextChannel channel)
		{
			if (Math.Abs(UserAccounts.GetAccount(user).LastCountedMessageTime.Minute - DateTime.Now.Minute) < 1) return;

			var userAccount = UserAccounts.GetAccount(user);
			userAccount.LastCountedMessageTime = DateTime.Now;
			var oldLevel = userAccount.LevelNumber;
			userAccount.Xp += (uint)Global.Random.Next(15, 26);
			UserAccounts.SaveAccounts();

			if (oldLevel != userAccount.LevelNumber)
			{
				await channel.SendMessageAsync($"{Global.GetNickname(user)} is now level {userAccount.LevelNumber}!");
			}
		}
	}
}
