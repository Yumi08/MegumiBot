using System.Collections.Generic;
using System.Linq;
using Discord;

namespace MegumiBot.Core.GuildAccounts
{
	public class Guild
	{
		public ulong Id { get; set; }

		private List<GuildChannel> _channels = new List<GuildChannel>();

		public GuildChannel GetChannel(IChannel channel)
		{
			if (_channels.Any(c => c.Id == channel.Id)) return _channels.FirstOrDefault(c => c.Id == channel.Id);

			var newChannel = new GuildChannel {Id = channel.Id};
			_channels.Add(newChannel);
			return newChannel;
		}
	}
}
