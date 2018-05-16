using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using MegumiBot.Core;
using MegumiBot.Core.GuildAccounts;

namespace MegumiBot
{
	// Written by Petrspelos
	public class CommandHandler
	{
		DiscordSocketClient _client;
		CommandService _service;

		public async Task InitializeAsync(DiscordSocketClient client)
		{
			_client = client;
			_service = new CommandService();
			await _service.AddModulesAsync(Assembly.GetEntryAssembly());
			_client.MessageReceived += HandleCommandAsync;
		}

		private async Task HandleCommandAsync(SocketMessage s)
		{
			if (!(s is SocketUserMessage msg)) return;
			var context = new SocketCommandContext(_client, msg);
			if (context.User.IsBot) return;

			Leveling.MessageReceived(context.User as SocketGuildUser, context.Channel as SocketTextChannel);

			int argPos = 0;
			// If the guild prefix hasn't been set, then get the default one
			if(msg.HasStringPrefix(Guilds.GetGuild(context.Guild).Prefix ?? Config.Bot.DefaultPrefix, ref argPos) 
			   || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
			{
				var result = await _service.ExecuteAsync(context, argPos);
				if(!result.IsSuccess)
				{
					Console.WriteLine(result.ErrorReason);
				}
			}
		}
	}
}
