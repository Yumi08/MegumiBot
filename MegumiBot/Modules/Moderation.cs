using System;
using System.Globalization;
using System.Linq;
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
		public async Task Warn(SocketGuildUser user, [Remainder] string reason)
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

			await user.SendMessageAsync("", false, embed);

			var m = await Context.Channel.SendMessageAsync("Success");
			await Task.Delay(1000);
			await m.DeleteAsync();
		}

		[Command("nick")]
		[Priority(1)]
		[RequireUserPermission(GuildPermission.ManageNicknames)]
		[RequireBotPermission(GuildPermission.ManageNicknames)]
		public async Task Nick(SocketGuildUser user, [Remainder] string newNickname)
		{
			await user.ModifyAsync(u => u.Nickname = newNickname);

			await Context.Channel.SendMessageAsync(
				$"{Global.GetNickname(user)}'s nickname has been changed to \"{newNickname}\"!");
		}

		[Command("kick")]
		[Priority(1)]
		[RequireUserPermission(GuildPermission.Administrator)]
		[RequireBotPermission(GuildPermission.KickMembers)]
		public async Task Kick(SocketGuildUser user)
		{
			await user.SendMessageAsync($"You've been kicked from {Context.Guild.Name}!");
			await user.KickAsync();

			await Context.Channel.SendMessageAsync($"{Global.GetNickname(user)} has been kicked!");
		}

		[Command("kick")]
		[Priority(1)]
		[RequireUserPermission(GuildPermission.Administrator)]
		[RequireBotPermission(GuildPermission.KickMembers)]
		public async Task Kick(SocketGuildUser user, [Remainder] string reason)
		{
			await user.SendMessageAsync($"You've been kicked from {Context.Guild.Name} for {reason}!");
			await user.KickAsync(reason);

			await Context.Channel.SendMessageAsync($"{Global.GetNickname(user)} has been kicked for {reason}!");
		}

		[Command("serverinfo")]
		public async Task ServerInfo()
		{
			var embed = new EmbedBuilder
			{
				Title = $"{Context.Guild.Name}'s Info",
				Color = Color.DarkBlue
			};

			embed.AddInlineField("ID", Context.Guild.Id);
			embed.AddInlineField("Users", Context.Guild.Users.Count);
			embed.AddInlineField("Online Users", Context.Guild.Users.Count(u => u.Status == UserStatus.Online));
			embed.AddInlineField("Bot Users", Context.Guild.Users.Count(u => u.IsBot));
			embed.AddInlineField("Text Channels", Context.Guild.TextChannels.Count);
			embed.AddInlineField("Voice Channels", Context.Guild.VoiceChannels.Count);

			await Context.Channel.SendMessageAsync("", embed: embed);
		}
	}
}
