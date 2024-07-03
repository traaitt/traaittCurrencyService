using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace traaittCurrencyService
{
    partial class traaittCurrencyService
    {
        private static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;

        // Initialization
        public static async Task Main(string[] args)
        {
            await RunBotAsync();

            // Wait for keypress to exit
            Console.ReadKey();

            // Close the database connection
            CloseDatabase();
        }

        // Initiate bot
        public static async Task RunBotAsync()
        {
            // Set message cache size
            DiscordSocketConfig socketConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 100
            };

            // Load local files
            Log(0, "traaittCurrencyService", "Loading config");
            await LoadConfig();
            Log(0, "traaittCurrencyService", "Loading database");
            await LoadDatabase();

            // Populate API variables
            _client = new DiscordSocketClient(socketConfig);
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            // Add event handlers
            _client.Log += Log;
            _client.Ready += Ready;

            // Register commands and start bot
            Log(0, "traaittCurrencyService", "Starting discord client");
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, botToken);
            await _client.StartAsync();

            // Set tip bot address
            Log(0, "traaittCurrencyService", "Setting default address");
            await SetAddress();

            // Rest until a disconnect is detected
            await Task.Delay(-1);
        }

        private static bool Monitoring = false;
        private static Task Ready()
        {
            // Begin wallet monitoring once gateway reports as ready
            if (!Monitoring)
            {
                Log(0, "traaittCurrencyService", "Starting wallet monitor");
                BeginMonitoringAsync();
                Monitoring = true;
            }
            return Task.CompletedTask;
        }

        // Register commands within API
        private static async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += MessageReceivedAsync;
            _client.ReactionAdded += ReactionAddedAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // Log event handler
        private static Task Log(LogMessage arg)
        {
            // Ignore invalid messages
            if (arg.Message == null) return Task.CompletedTask;

            // Log message to console
            if (!arg.Message.Contains("UNKNOWN_DISPATCH"))
                Console.WriteLine($"{arg.Source}: {arg.Message}");

            // Restart if disconnected
            if (arg.Message.Contains("Disconnected"))
                Console.WriteLine("Restarting bot...");

            return Task.CompletedTask;
        }

        // Message received
        private static async Task MessageReceivedAsync(SocketMessage arg)
        {
            // Get message and create a context
            if (!(arg is SocketUserMessage message)) return;
            var context = new SocketCommandContext(_client, message);

            // Process commands
            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        // Reaction added
        private static async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> cacheableMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Get reaction data
            var message = await cacheableMessage.GetOrDownloadAsync();

            // Ignore own reactions
            if (reaction.UserId == _client.CurrentUser.Id)
                return;

            // Get emote name
            string emote = reaction.Emote.ToString().Contains(':')
                ? reaction.Emote.ToString().Split(':')[1]
                : reaction.Emote.ToString();

            // Check if reaction is a join reaction
            if (emote == "joinReact")
            {
                // Check if message is a tip message
                if (!message.Content.StartsWith("!tip")) return;

                // Example for checking tip amount and user balance
                decimal amount = Convert.ToDecimal(message.Content.Split(' ')[1]);
                if (amount < 0.1m) return; // Example amount check

                // Additional checks and tipping logic here...

                // Send success reaction
                await message.AddReactionAsync(new Emoji("✅"));
            }
        }

        private static async Task BeginMonitoringAsync()
        {
            // Simulated monitoring task
            for (int i = 573001; i <= 577001; i += 1000)
            {
                Console.WriteLine($"Scanning for deposits from height: {i}");
                await Task.Delay(500); // Simulate scanning work
            }
        }

        private static async Task LoadConfig()
        {
            // Simulate loading config
            await Task.Delay(500);
        }

        private static async Task LoadDatabase()
        {
            // Simulate loading database
            await Task.Delay(500);
        }

        private static void CloseDatabase()
        {
            // Simulate closing database
            Console.WriteLine("Closing database...");
        }

        private static async Task SetAddress()
        {
            // Simulate setting address
            await Task.Delay(500);
            Console.WriteLine("Address set.");
        }
    }
}
