using System.Collections.Generic;
using System.Linq;
using Discord;

namespace MegumiBot.Core.GuildAccounts
{
	public class Guild
	{
		public ulong Id { get; set; }

		public string Prefix { get; set; } = null;

		public List<GuildChannel> Channels { get; set; }= new List<GuildChannel>();

		public GuildChannel GetChannel(IChannel channel)
		{
			if (Channels.Any(c => c.Id == channel.Id)) return Channels.FirstOrDefault(c => c.Id == channel.Id);

			var newChannel = new GuildChannel {Id = channel.Id};
			Channels.Add(newChannel);
			return newChannel;
		}
	}
}
