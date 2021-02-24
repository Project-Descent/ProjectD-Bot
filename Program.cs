using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sheriff
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();


            string token = "NzM5OTIwNTcwMTA3NTU5OTk3.Xyheug.c0YcparuhayYXf_UVigLSp0nMMo";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;


            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
    public class MemberAssignmentService
    {
        private readonly ulong _roleId;
        public MemberAssignmentService(DiscordSocketClient client, ulong roleId)
        {
            // Hook the evnet
            client.UserJoined += AssignMemberAsync;

            // Note that we are using role identifier here instead
            // of name like your original solution; this is because
            // a role name check could easily be circumvented by a new role
            // with the exact name.
            _roleId = roleId;
        }

        private async Task AssignMemberAsync(SocketGuildUser guildUser)
        {
            var guild = guildUser.Guild;
            // Check if the desired role exist within this guild.
            // If not, we simply bail out of the handler.
            var role = guild.GetRole(_roleId);
            if (role == null) return;
            // Check if the bot user has sufficient permission
            // Finally, we call AddRoleAsync
            await guildUser.AddRoleAsync(role);
        }
    }
}
