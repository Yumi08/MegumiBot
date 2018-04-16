using System.Collections.Generic;
using System.Linq;
using Discord;

namespace MegumiBot.Core.GuildAccounts
{
	public class Guild
	{
		/// <summary>
		/// The guild's Discord-generated Id.
		/// </summary>
		public ulong Id { get; set; }

		/// <summary>
		/// The guild's custom-set command prefix.
		/// </summary>
		public string Prefix { get; set; } = null;

		/// <summary>
		/// A list of the guild's channels.
		/// </summary>
		public List<GuildChannel> Channels { get; set; }= new List<GuildChannel>();

		/// <summary>
		/// Get a bot channel from a Discord channel.
		/// </summary>
		/// <param name="channel"></param>
		/// <returns></returns>
		public GuildChannel GetChannel(IChannel channel)
		{
			if (Channels.Any(c => c.Id == channel.Id)) return Channels.FirstOrDefault(c => c.Id == channel.Id);

			var newChannel = new GuildChannel {Id = channel.Id};
			Channels.Add(newChannel);
			return newChannel;
		}
	}
}
