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
		/// <summary>
		/// Get a lewd image on Yumi08's personal hentai collection ;>
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
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

			List<Image> matchingImages = new List<Image>();

			#region Match Detection

			matchingImages.AddRange(images.Where(i =>
			{
				if (i.Tags == null) return false;

				return i.Tags
					.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
			}));

			matchingImages.AddRange(images.Where(i =>
			{
				if (i.Characters == null) return false;

				return i.Characters
					.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
			}));

			matchingImages.AddRange(images.Where(i =>
			{
				if (i.Copyrights == null) return false;

				return i.Copyrights
					.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
			}));

			matchingImages.AddRange(images.Where(i =>
			{
				if (i.Artists == null) return false;

				return i.Artists
					.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase));
			}));

			#endregion

			var image = matchingImages[Global.Random.Next(matchingImages.Count)];
			var imageFile = directory.GetFiles().FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name) == image.Id);

			await Context.Channel.SendFileAsync(imageFile?.FullName);
		}
	}
}
