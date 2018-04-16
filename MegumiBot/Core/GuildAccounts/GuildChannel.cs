namespace MegumiBot.Core.GuildAccounts
{
	public class GuildChannel
	{
		/// <summary>
		/// The channel's Discord-generated Id.
		/// </summary>
		public ulong Id { get; set; }

		/// <summary>
		/// If the channel is assigned to be nsfw by a channel manager through a command, not through discord.
		/// </summary>
		public bool IsNsfw { get; set; }
	}
}
