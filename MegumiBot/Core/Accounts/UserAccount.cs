using System;

namespace MegumiBot.Core.Accounts
{
	public class UserAccount
	{
		public ulong Id { get; set; }

		public uint Currency { get; set; }
		
		public DateTime LastCountedMessageTime { get; set; } = new DateTime(2000, 1, 1);

		public uint Xp { get; set; }

		public uint LevelNumber => (uint)Math.Sqrt(Xp / 50f);

		public uint TotalMessages { get; set;}
	}
}