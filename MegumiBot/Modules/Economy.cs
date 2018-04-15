using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MegumiBot.Core.Accounts;
using MegumiBot.Core.Responses;

namespace MegumiBot.Modules
{
	public class Economy : ModuleBase<SocketCommandContext>
	{
		[Command("profile")]
		public async Task Profile()
		{
			var userAccount = UserAccounts.GetAccount(Context.User);

			var embed = new EmbedBuilder
			{
				Title = $"{Global.GetNickname(Context.User as IGuildUser)}'s Profile",
				Color = new Color(135, 229, 197)
			};

			embed.AddInlineField("Level", userAccount.LevelNumber);
			embed.AddInlineField("XP", userAccount.Xp);
			embed.AddInlineField("Money", $"{Config.bot.CurrencySymbol}{userAccount.Currency}");
			embed.AddInlineField("Messages", userAccount.TotalMessages);

			await Context.Channel.SendMessageAsync("", embed: embed);
		}

		[Command("bet")]
		public async Task Bet(uint amt)
		{
			if (amt == 0) return;

			var userAccount = UserAccounts.GetAccount(Context.User);

			if (userAccount.Currency < amt)
			{
				await Context.Channel.SendMessageAsync(
					$"Unfortunately, you're {Config.bot.CurrencySymbol}{amt - userAccount.Currency} short of that!");
				return;
			}

			userAccount.Currency -= amt;

			switch (Global.Random.Next(2))
			{
				case 0:
					await Context.Channel.SendMessageAsync(ResponseGetter.GetRandomResponse("BetLoss"));
					break;
				case 1:
					await Context.Channel.SendMessageAsync($"Congratulations! You won {Config.bot.CurrencySymbol}{amt * 2}!");
					userAccount.Currency += amt * 2;
					break;
			}
		}

		[Command("dump", RunMode = RunMode.Async)]
		public async Task Dump(uint amt)
		{
			var userAccount = UserAccounts.GetAccount(Context.User);

			if (userAccount.Currency < amt)
			{
				await Context.Channel.SendMessageAsync(
					$"Unfortunately, you're {Config.bot.CurrencySymbol}{amt - userAccount.Currency} short of that!");
				return;
			}

			await Context.Channel.SendMessageAsync($"Are you sure you want to dump {Config.bot.CurrencySymbol}{amt}? (Y/N)");

			var response = await Global.AwaitYesNoMessage(Context.User.Id, Context.Channel.Id, 5000);

			switch (response)
			{
				case true:
					await Context.Channel.SendMessageAsync($"Throwing away {Config.bot.CurrencySymbol}{amt}!");
					userAccount.Currency -= amt;
					break;
				case false:
					await Context.Channel.SendMessageAsync("Cancelled!");
					break;
			}
		}
	}
}
