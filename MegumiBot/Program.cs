using Discord.WebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using MegumiBot.Core;

namespace MegumiBot
{
	// Written by Petrspelos
	class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;
	    private bool _clientIsReady;
	    private SocketTextChannel _currentMessageChannel;

        static void Main()
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (string.IsNullOrEmpty(Config.Bot.Token)) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
	        _client.Ready += AutoSave.StartTimer;
	        _client.Ready += () =>
	        {
		        _clientIsReady = true;
		        return Task.CompletedTask;
	        };
	        _client.Disconnected += OnDisconnected;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
			await _handler.InitializeAsync(_client);
			// Don't await
	        ConsoleInput();
            await Task.Delay(-1);
        }

	    private async Task Log(LogMessage msg)
	    {
		    Console.WriteLine(msg.Message);
	    }

	    private async Task OnDisconnected(Exception e)
	    {
		    await Task.Delay(7500);

		    if (_client.ConnectionState != ConnectionState.Disconnected) return;

		    Console.WriteLine("OnDisconnected(): Attempting to reconnect...");

		    await _client.LogoutAsync();
		    await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
	    }

		#region Console Commands

	    private async Task ConsoleInput()
	    {
		    var input = string.Empty;
			while (input?.Trim().ToLower() != "block")
			{
				input = Console.ReadLine();

				switch (input?.Trim().ToLower())
				{
					case "m":
						if (!_clientIsReady) break;
						ConsoleSendMessage();
						break;

					case "setchannel":
						Console.WriteLine("\nSelect the guild:");
						var guild = GetSelected(_client.Guilds);
						_currentMessageChannel = GetSelected(guild.TextChannels);
						break;
				}
			}
	    }

	    private async void ConsoleSendMessage()
	    {
		    if (_currentMessageChannel != null)
		    {
			    GetMessageAndSend(_currentMessageChannel);
			    return;
		    }

			Console.WriteLine("\nSelect the guild:");
		    var guild = GetSelected(_client.Guilds);
		    var textChannel = GetSelected(guild.TextChannels);
		    var msg = string.Empty;
		    while (msg?.Trim() == string.Empty)
			{
				Console.WriteLine("Your message:");
				msg = Console.ReadLine();
				Console.WriteLine();
			}

		    _currentMessageChannel = textChannel;
			await textChannel.SendMessageAsync(msg);
	    }

		private static async void GetMessageAndSend(ITextChannel channel)
		{
			Console.WriteLine("Your message:");
			var msg = Console.ReadLine();
			Console.WriteLine();

			await channel.SendMessageAsync(msg);
		}

		private static SocketGuild GetSelected(IEnumerable<SocketGuild> guilds)
	    {
		    var guildsList = guilds.ToList();
			var maxIndex = guildsList.Count - 1;

		    Console.WriteLine();
		    for (var i = 0; i <= maxIndex; i++)
		    {
			    Console.WriteLine($"{i} - {guildsList[i].Name}");
		    }
		    Console.WriteLine();

		    var selectedIndex = -1;
		    while (selectedIndex < 0 || selectedIndex > maxIndex)
		    {
			    var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
			    if (!success)
			    {
					Console.WriteLine("\nPlease provide a valid index.");
				    selectedIndex = -1;
			    }

				if (selectedIndex < 0 || selectedIndex > maxIndex)
					Console.WriteLine("\nThe index didn't fit within the guild list, please try again.");
		    }

		    return guildsList[selectedIndex];
	    }

	    private static SocketTextChannel GetSelected(IEnumerable<SocketTextChannel> channels)
	    {
		    var socketTextChannels = channels.ToList();
		    var maxIndex = socketTextChannels.Count - 1;

		    Console.WriteLine();
		    for (var i = 0; i <= maxIndex; i++)
		    {
			    Console.WriteLine($"{i} - {socketTextChannels[i].Name}");
		    }
		    Console.WriteLine();

		    var selectedIndex = -1;
		    while (selectedIndex < 0 || selectedIndex > maxIndex)
		    {
			    var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
			    if (!success)
			    {
				    Console.WriteLine("\nPlease provide a valid index.");
				    selectedIndex = -1;
			    }

			    if (selectedIndex < 0 || selectedIndex > maxIndex)
				    Console.WriteLine("\nThe index didn't fit within the channel list, please try again.");
		    }

		    return socketTextChannels[selectedIndex];
	    }

		#endregion

    }
}