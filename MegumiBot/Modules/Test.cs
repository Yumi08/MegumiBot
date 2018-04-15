using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegumiBot.Core.GuildAccounts;
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

		[Command("mentionguilduser")]
		public async Task Mention(IGuildUser targetGuildUser)
		{
			await Context.Channel.SendMessageAsync(targetGuildUser.Mention);
		}

		[Command("neko")]
		public async Task Neko()
		{
			if (!Guilds.GetGuild(Context.Guild).GetChannel(Context.Channel).IsNsfw)
			{
				await Context.Channel.SendMessageAsync("Th-That's for NSFW channels! You lewdie!!!");
				return;
			}

			string json;
			using (var client = new WebClient())
			{
				json = client.DownloadString("https://nekos.life/api/neko");
			}

			var searchResult = JsonConvert.DeserializeObject<dynamic>(json);
            var embed = new EmbedBuilder();
			var url = searchResult.neko.ToString();
            embed.WithImageUrl(url);
            embed.WithAuthor("Source : Nekos.life");
            embed.WithColor(255, 0, 255);
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)] // RequireBotPermission isn't completely necessary here.
        public async Task Warn(SocketGuildUser guilduser, [Remainder] string reason)
        {
            await Context.Message.DeleteAsync();
            string timestamp = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);
            var embed = new EmbedBuilder()
            {
				Color = new Color(255, 0, 2),
				Title = ":warning: Warning",
				Description = $"Reason: {reason}",
            };

            embed.WithAuthor(Context.Guild.Name);
            embed.WithFooter($"Timestamp : {timestamp} UTC-5");

            await guilduser.SendMessageAsync("", false, embed);

            var m = await Context.Channel.SendMessageAsync("Success");
            await Task.Delay(1000);
            await m.DeleteAsync();
        }

		//Inspired by a Command from an existing Bot
        [Command("dealwithit")]
        public async Task Glasses()
        {
            await this.Context.Message.DeleteAsync();
            var msg = await ReplyAsync("( ͡° ͜ʖ ͡°)>⌐■-■");
            await Task.Delay(1350);
            await msg.ModifyAsync(x => x.Content = "( ͡⌐■ ͜ʖ ͡-■) Deal with it.");
        }

        [Command("someone")]
		public async Task Someone()
		{
			var users = Context.Guild.Users.Where(u => u.Status == UserStatus.Online).ToList();

			var userIndex = Global.Random.Next(users.Count() + 1);

			await Context.Channel.SendMessageAsync(users[userIndex].Mention);
		}

		[Command("setnsfw")]
		public async Task SetNsfw(IChannel channel)
		{
			var guild = Guilds.GetGuild(Context.Guild);
			guild.GetChannel(channel).IsNsfw = true;
			Guilds.SaveGuilds();

			await Context.Channel.SendMessageAsync($"#{channel.Name} is now NSFW!");
		}

		[Command("unsetnsfw")]
		public async Task UnsetNsfw(IChannel channel)
		{
			var guild = Guilds.GetGuild(Context.Guild);
			guild.GetChannel(channel).IsNsfw = false;
			Guilds.SaveGuilds();

			await Context.Channel.SendMessageAsync($"#{channel.Name} is now no longer NSFW!");
		}
	}
}
