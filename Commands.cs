using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace TutorialBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("say")]
        [Summary("Echo text.")]
        public async Task SayAsync([Remainder] string messageString = null)
        { 
            if (String.IsNullOrEmpty(messageString)) return;
            await ReplyAsync(messageString);
        }

        [Command("report")]
        [Summary("report games from DM")]
        public async Task SendPMAsync([Remainder] string messageString = "Who did you play with?")
        {
            if (String.IsNullOrEmpty(messageString)) return;
            SocketUser user = Context.Message.Author;
            Discord.UserExtensions.SendMessageAsync(user, messageString).Wait();
        }

        [Command("report")]
        public async Task Report()
        {
           
            void SendDirect(SocketUser user)
            {
                string message = "This is just a placeholder test";
                Discord.UserExtensions.SendMessageAsync(user, message).Wait();
            }
        }

        [Command("Garbage")]
        private async Task Max()
        {
            string maxx = "C:\\maxx.jpg";
            await Context.Channel.SendFileAsync(maxx, "Fucking Garbage!");
        }

        [Command("Schedule")]
        private async Task Schedule()
        {
            DateTime dt1 = DateTime.Now;
            string weekschedule = "C:\\schedule.png";
            await Context.Channel.SendFileAsync(weekschedule, "Updated: " + dt1);
        }

        [Command("flip")]
        private async Task flip()
        {
            Random rdn = new Random();
            int coin = rdn.Next(0, 2);

            if (coin == 1)
            {
                await ReplyAsync("The answer is **heads**");

            }
            else if (coin == 0)
            {
                await ReplyAsync("The answer is **tails**");
            }


        }

        [Command("makemeasandwich")]
        async Task makemeasandwich()
        {
            string food = "C:\\sandwich.gif";
            await Context.Channel.SendFileAsync(food, "Nom Nom");
        }

        [Command("catchapacket")]
        public async Task cap()
        {
            await ReplyAsync("Yeah... not in overload im afriad.");
        }

        [Command("wirelesswins")]
        public async Task ww()
        {
            await ReplyAsync("Use a fucking wire, asshole.");
        }

        [Command("lucky")]
        public async Task role()
        {
            Random rdn = new Random();
            int chance = rdn.Next(0, 23);

            if (chance == 3)
            {
                var user = Context.User;
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "lucky");
                await (user as IGuildUser).AddRoleAsync(role);
                await ReplyAsync("95.24% chance of losing. Consider a lottery ticket ;)");
            }
            else if (chance != 3)
                await ReplyAsync("Sorry better luck next time.");
        }
    }
}

