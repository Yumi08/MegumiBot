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
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
			await _handler.InitializeAsync(_client);
			// Don't await
	        ConsoleInput();
            await Task.Delay(-1);
        }

	    private async Task ConsoleInput()
	    {
		    var input = string.Empty;
			while (input?.Trim().ToLower() != "block")
			{
				input = Console.ReadLine();

				switch (input?.Trim().ToLower())
				{
					case "message":
						ConsoleSendMessage();
						break;
				}
			}
	    }

	    private async void ConsoleSendMessage()
	    {
			Console.WriteLine("Select the guild:");
		    var guild = GetSelected(_client.Guilds);
		    var textChannel = GetSelected(guild.TextChannels);
		    var msg = string.Empty;
		    while (msg.Trim() == string.Empty)
		    {
			    Console.WriteLine("Your message:");
			    msg = Console.ReadLine();
		    }

		    await textChannel.SendMessageAsync(msg);
	    }

	    private static SocketGuild GetSelected(IEnumerable<SocketGuild> guilds)
	    {
		    var guildsList = guilds.ToList();
			var maxIndex = guildsList.Count - 1;

		    for (var i = 0; i <= maxIndex; i++)
		    {
			    Console.WriteLine($"{i} - {guildsList[i].Name}");
		    }

		    var selectedIndex = -1;
		    while (selectedIndex < 0 || selectedIndex > maxIndex)
		    {
			    var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
			    if (!success)
			    {
					Console.WriteLine("Please provide a valid index.");
				    selectedIndex = -1;
			    }

				if (selectedIndex < 0 || selectedIndex > maxIndex)
					Console.WriteLine("The index didn't fit within the guild list, please try again.");
		    }

		    return guildsList[selectedIndex];
	    }

	    private static SocketTextChannel GetSelected(IEnumerable<SocketTextChannel> channels)
	    {
		    var socketTextChannels = channels.ToList();
		    var maxIndex = socketTextChannels.Count - 1;

		    for (var i = 0; i <= maxIndex; i++)
		    {
			    Console.WriteLine($"{i} - {socketTextChannels[i].Name}");
		    }

		    var selectedIndex = -1;
		    while (selectedIndex < 0 || selectedIndex > maxIndex)
		    {
			    var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
			    if (!success)
			    {
				    Console.WriteLine("Please provide a valid index.");
				    selectedIndex = -1;
			    }

			    if (selectedIndex < 0 || selectedIndex > maxIndex)
				    Console.WriteLine("The index didn't fit within the channel list, please try again.");
		    }

		    return socketTextChannels[selectedIndex];
	    }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}