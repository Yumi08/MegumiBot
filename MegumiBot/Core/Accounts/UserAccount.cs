using System;

namespace MegumiBot.Core.Accounts
{
	// Written by Petrspelos
	public class UserAccount
	{
		public ulong Id { get; set; }

		public uint Points { get; set; }

		public uint Xp { get; set; }

		public uint LevelNumber => (uint)Math.Sqrt(Xp / 50f);

		public bool IsMuted { get; set; }

		public uint NumberOfWarnings { get; set; }
	}
}