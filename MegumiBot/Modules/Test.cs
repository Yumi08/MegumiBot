using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegumiBot.Core.Accounts;
using MegumiBot.Core.GuildAccounts;
using MegumiBot.Core.Responses;
using Newtonsoft.Json;
using WebClient = System.Net.WebClient;

namespace MegumiBot.Modules
{
	public class Test : ModuleBase<SocketCommandContext> //usefull commands should be transfered to the command class when done.
	{
        [Command("echo")]
		public async Task Echo(string input)
		{
			await Context.Channel.SendMessageAsync(input);
		}

		[Command("mention")]
		public async Task Mention()
		{
			await Context.Channel.SendMessageAsync(Context.User.Mention);
		}

		[Command("mention")]
		public async Task Mention(IGuildUser targetGuildUser)
		{
			await Context.Channel.SendMessageAsync(targetGuildUser.Mention);
		}

		// Debug command
		[Command("currencyset")]
		public async Task CurrencySet(uint value)
		{
			await Context.Channel.SendMessageAsync($"Setting your currency to {Config.bot.CurrencySymbol}{value}!");
			UserAccounts.GetAccount(Context.User).Currency = value;
		}

		// This command is VERY important in order to save accounts on exit
		[Command("exit")]
		// RequireOwner will obviously have to be changed later on to whoever's running the bot
		[RequireOwner]
		public async Task Exit()
		{
			await Context.Channel.SendMessageAsync("Closing down and saving!");
			await Global.SaveAll();

			Environment.Exit(0);
		}
	}
}
