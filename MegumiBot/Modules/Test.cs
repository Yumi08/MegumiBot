using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using WebClient = System.Net.WebClient;

namespace MegumiBot.Modules
{
	public class Test : ModuleBase<SocketCommandContext>
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
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task warn(SocketGuildUser guilduser, [Remainder] string reason)
        {
            string timestamp = System.Convert.ToString(DateTime.Now);
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



        [Command("someone")]
		public async Task Someone()
		{
			var users = Context.Guild.Users.Where(u => u.Status == UserStatus.Online).ToList();

			var userIndex = Global.Random.Next(users.Count() + 1);

			await Context.Channel.SendMessageAsync(users[userIndex].Mention);
		}
	}
}
