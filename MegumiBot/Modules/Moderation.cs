using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MegumiBot.Modules
{
	public class Moderation : ModuleBase<SocketCommandContext>
	{
		[Command("warn")]
		[Priority(1)]
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
	}
}
