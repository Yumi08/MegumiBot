using System.Threading.Tasks;
using Discord.Commands;

namespace MegumiBot.Modules
{
	public class Test : ModuleBase<SocketCommandContext>
	{
		[Command("echo")]
		public async Task Echo(string input)
		{
			await Context.Channel.SendMessageAsync(input);
		}
	}
}
