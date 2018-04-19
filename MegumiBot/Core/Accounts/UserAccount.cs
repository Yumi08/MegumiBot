using System;
using YubooruCollectionManager.Files;

namespace MegumiBot.Core.Accounts
{
	public class UserAccount
	{
		/// <summary>
		/// The user's Discord-generated Id.
		/// </summary>
		public ulong Id { get; set; }

		/// <summary>
		/// The amount of fake currency a user has.
		/// </summary>
		public uint Currency { get; set; }
		
		/// <summary>
		/// The last time a user sent a message that was counted toward Xp.
		/// </summary>
		public DateTime LastCountedMessageTime { get; set; } = new DateTime(2000, 1, 1);

		/// <summary>
		/// The amount of experience a user has gained through sending messages.
		/// </summary>
		public uint Xp { get; set; }

		/// <summary>
		/// This user's Xp based level.
		/// </summary>
		public uint LevelNumber => (uint)Math.Sqrt(Xp / 50f);

		/// <summary>
		/// The total messages this user has sent anywhere.
		/// </summary>
		public uint TotalMessages { get; set;}

		/// <summary>
		/// Info about the last Image the user called from Yubooru
		/// </summary>
		public ImageInfo LastYubooruImageInfo { get; set; }
	}
}