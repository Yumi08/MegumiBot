using Discord.WebSocket;
using System;
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
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}