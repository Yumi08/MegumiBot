using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace MegumiBot.Core.Accounts
{
	// Written by Petrspelos
	public static class UserAccounts
	{
		private static List<UserAccount> _accounts;

		private static string _accountsFile = "Resources/accounts.json";

		static UserAccounts()
		{
			if(DataStorage<UserAccount>.SaveExists(_accountsFile))
			{
				_accounts = DataStorage<UserAccount>.LoadItems(_accountsFile).ToList();
			}
			else
			{
				_accounts = new List<UserAccount>();
				SaveAccounts();
			}
		}

		public static void SaveAccounts()
		{
			DataStorage<UserAccount>.SaveItems(_accounts, _accountsFile);
		}

		public static UserAccount GetAccount(SocketUser user)
		{
			return GetOrCreateAccount(user.Id);
		}

		private static UserAccount GetOrCreateAccount(ulong id)
		{
			var result = from a in _accounts
				where a.Id == id
				select a;

			var account = result.FirstOrDefault() ?? CreateUserAccount(id);
			return account;
		}

		private static UserAccount CreateUserAccount(ulong id)
		{
			var newAccount = new UserAccount()
			{
				Id = id,
				Points = 10,
				Xp = 0
			};

			_accounts.Add(newAccount);
			SaveAccounts();
			return newAccount;
		}
	}
}