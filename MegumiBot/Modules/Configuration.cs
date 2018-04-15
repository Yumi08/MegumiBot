using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MegumiBot.Core.GuildAccounts;

namespace MegumiBot.Modules
{
	public class Configuration : ModuleBase<SocketCommandContext>
	{
		[Command("setnsfw")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task SetNsfw(IChannel channel)
		{
			var guild = Guilds.GetGuild(Context.Guild);
			guild.GetChannel(channel).IsNsfw = true;

			await Context.Channel.SendMessageAsync($"#{channel.Name} is now NSFW!");
		}

		[Command("unsetnsfw")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task UnsetNsfw(IChannel channel)
		{
			var guild = Guilds.GetGuild(Context.Guild);
			guild.GetChannel(channel).IsNsfw = false;

			await Context.Channel.SendMessageAsync($"#{channel.Name} is now no longer NSFW!");
		}

		/// <summary>
		/// Set's the guild's custom prefix
		/// </summary>
		/// <returns></returns>
		[Command("setprefix")]
		[RequireOwner]
		public async Task SetPrefix(string prefix)
		{
			if (prefix.Length > 4)
			{
				await Context.Channel.SendMessageAsync("Prefixes have a maximum of 4 characters in length!");
				return;
			}

			Guilds.GetGuild(Context.Guild).Prefix = prefix;

			await Context.Channel.SendMessageAsync($"My command prefix has been set to \"{prefix}\"!");
		}

		[Command("resetprefix")]
		[RequireOwner]
		public async Task ResetPrefix()
		{
			Guilds.GetGuild(Context.Guild).Prefix = null;

			await Context.Channel.SendMessageAsync("My command prefix has been reset!");
		}
	}
}
