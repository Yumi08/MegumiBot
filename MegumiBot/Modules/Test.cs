using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegumiBot.Core.Accounts;

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

		[Command("respond", RunMode = RunMode.Async)]
		public async Task Respond()
		{
			var response = await Global.AwaitMessage(Context, 5000);

			if (response == null)
				await Context.Channel.SendMessageAsync("You said nothing!");
			else
				switch (response.ToLower())
				{
					case "y":
						await Context.Channel.SendMessageAsync("You said yes!");
						break;
					case "n":
						await Context.Channel.SendMessageAsync("You said no!");
						break;
				}
		}
	}
}
