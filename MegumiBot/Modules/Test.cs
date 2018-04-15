using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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

		[Command("mention")]
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
            embed.WithImageUrl(searchResult.neko.Tostring());
            embed.WithAuthor("Source : Neko.life");
            embed.WithColor(255, 0, 255);
            await Context.Channel.SendMessageAsync("", false, embed);
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
