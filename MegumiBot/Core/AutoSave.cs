using System.Threading.Tasks;
using System.Timers;

namespace MegumiBot.Core
{
	/// <summary>
	/// Automatically saves bot guilds, user-accounts, etc.
	/// </summary>
	internal static class AutoSave
	{
		private static Timer _loopingTimer;

		internal static Task StartTimer()
		{
			_loopingTimer = new Timer
			{
				Interval = Config.Bot.AutosaveRate,
				AutoReset = true,
				Enabled = true
			};
			_loopingTimer.Elapsed += OnTimerTicked;

			return Task.CompletedTask;
		}

		private static void OnTimerTicked(object sender, ElapsedEventArgs e)
		{
			Global.SaveAll();
		}
	}
}
