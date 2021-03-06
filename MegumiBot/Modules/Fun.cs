﻿using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MegumiBot.Core.GuildAccounts;
using Newtonsoft.Json;

namespace MegumiBot.Modules
{
	public class Fun : ModuleBase<SocketCommandContext>
	{
		[Command("neko")]
		public async Task Neko()
		{
			string json;
			using (var client = new WebClient())
			{
				json = client.DownloadString("https://nekos.life/api/neko");
			}

            string timestamp = System.DateTime.Now.ToString(CultureInfo.InvariantCulture);
			var searchResult = JsonConvert.DeserializeObject<dynamic>(json);
			var embed = new EmbedBuilder();
			var url = searchResult.neko.ToString();
			embed.WithImageUrl(url);
            embed.WithTitle($"Neko for {Global.GetNickname((IGuildUser)Context.User)}!");
			embed.WithAuthor("Source : Nekos.life");
            embed.WithFooter($"Timestamp : {timestamp} GMT-5 ");
			embed.WithColor(255, 0, 255);
			await Context.Channel.SendMessageAsync("", false, embed);
		}

		//Inspired by a Command from an existing Bot
		[Command("dealwithit")]
		public async Task Glasses()
		{
			await Context.Message.DeleteAsync();
			var msg = await ReplyAsync("( ͡° ͜ʖ ͡°)>⌐■-■");
			await Task.Delay(1350);
			await msg.ModifyAsync(x => x.Content = "( ͡⌐■ ͜ʖ ͡-■) Deal with it.");
		}

		[Command("someone")]
		public async Task Someone()
		{
			var users = Context.Guild.Users.Where(u => u.Status == UserStatus.Online).ToList();

			var userIndex = Global.Random.Next(users.Count);

			await Context.Channel.SendMessageAsync(users[userIndex].Mention);
		}

		[Command("decide")]
		public async Task Decide()
		{
			switch (Global.Random.Next(2))
			{
				case 0:
					await Context.Channel.SendMessageAsync(":thumbsup:");
					break;
				case 1:
					await Context.Channel.SendMessageAsync(":thumbsdown:");
					break;
			}
		}
	}
}
