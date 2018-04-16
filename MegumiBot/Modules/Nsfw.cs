using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using MegumiBot.Core.GuildAccounts;
using Newtonsoft.Json;
using YubooruCollectionManager.Files;

namespace MegumiBot.Modules
{
	public class Nsfw : ModuleBase<SocketCommandContext>
	{
		[Command("yubooru")]
		public async Task Yubooru(string tag)
		{
			if (!Guilds.GetGuild(Context.Guild).GetChannel(Context.Channel).IsNsfw)
			{
				await Context.Channel.SendMessageAsync("Th-That's for NSFW channels! You lewdie!!!");
				return;
			}

			var directory = new DirectoryInfo(Config.Bot.YubooruLocation);

			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);
			var matchingImages = images.Where(i => i.Tags.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase))).ToList();
			var image = matchingImages[Global.Random.Next(matchingImages.Count)];
			var imageFile = directory.GetFiles().FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name) == image.Id);

			await Context.Channel.SendFileAsync(imageFile?.FullName);
		}
	}
}
