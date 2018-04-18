using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MegumiBot.Core.GuildAccounts;
using Newtonsoft.Json;
using Image = YubooruCollectionManager.Files.Image;

namespace MegumiBot.Modules
{
	public class Nsfw : ModuleBase<SocketCommandContext>
	{
		private static void Search(string[] tags, List<Image> images, List<Image> matchingImages)
		{
			foreach (var image in images)
			{
				var allTags = new List<string>();
				if (image.Artists != null) allTags.AddRange(image.Artists);
				if (image.Characters != null) allTags.AddRange(image.Characters);
				if (image.Copyrights != null) allTags.AddRange(image.Copyrights);
				if (image.Tags != null) allTags.AddRange(image.Tags);

				var matches = 0u;
				foreach (var tag in allTags)
				{
					foreach (var t in tags)
					{
						if (tag.Equals(t, StringComparison.InvariantCultureIgnoreCase))
							matches++;
					}
				}

				if (matches == tags.Length) matchingImages.Add(image);
			}
		}


		/// <summary>
		/// Get a lewd image on Yumi08's personal hentai collection ;>
		/// </summary>
		/// <returns></returns>
		[Command("yubooru")]
		public async Task Yubooru()
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

			var image = images[Global.Random.Next(images.Count)];
			var imageFile = directory.GetFiles().FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name) == image.Id);

			await Context.Channel.SendFileAsync(imageFile?.FullName);
		}

		[Command("yubooru")]
		public async Task Yubooru(params string[] tags)
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

			Search(tags, images, matchingImages);

			var image = matchingImages[Global.Random.Next(matchingImages.Count)];
			var imageFile = directory.GetFiles().FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name) == image.Id);

			await Context.Channel.SendFileAsync(imageFile?.FullName);
		}

		[Command("yuboorustat")]
		[Alias("yuboorustats")]
		public async Task YubooruStats()
		{
			if (!Guilds.GetGuild(Context.Guild).GetChannel(Context.Channel).IsNsfw)
			{
				await Context.Channel.SendMessageAsync("Th-That's for NSFW channels! You lewdie!!!");
				return;
			}

			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);

			var embed = new EmbedBuilder
			{
				Title = "Yubooru's stats",
				Color = Color.DarkMagenta
			};
			embed.AddInlineField("Images", images.Count);
			embed.AddInlineField("Tags", GetTotalTagCount());
			embed.AddInlineField("Artists", GetTotalArtistCount());
			embed.AddInlineField("Copyrights", GetTotalCopyrightCount());
			embed.AddInlineField("Characters", GetTotalCharacterCount());
			embed.AddInlineField("Contributors", 1); // Filler field because 5 fields looks really ugly on desktop

			await Context.Channel.SendMessageAsync("", embed: embed);
		}
		[Command("yuboorustat")]
		[Alias("yuboorustats")]
		public async Task YubooruStats(string stat)
		{
			if (!Guilds.GetGuild(Context.Guild).GetChannel(Context.Channel).IsNsfw)
			{
				await Context.Channel.SendMessageAsync("Th-That's for NSFW channels! You lewdie!!!");
				return;
			}

			// ToLower() is bad practice, but switches only use ordinal comparison
			switch (stat.ToLower())
			{
				case "total":
				{
					var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
					var json = File.ReadAllText(imageInfoPath);
					var images = JsonConvert.DeserializeObject<List<Image>>(json);
					await Context.Channel.SendMessageAsync($"Yubooru currently contains {images.Count} images!");
				}
					break;

				case "tags":
					await Context.Channel.SendMessageAsync($"Yubooru currently contains {GetTotalTagCount()} different tags!");
					break;

				case "artists":
					await Context.Channel.SendMessageAsync($"Yubooru currently contains {GetTotalArtistCount()} different artists!");
					break;

				case "copyrights":
					await Context.Channel.SendMessageAsync($"Yubooru currently contains {GetTotalCopyrightCount()} different copyrights!");
					break;

				case "characters":
				{
					

					await Context.Channel.SendMessageAsync($"Yubooru currently contains {GetTotalCharacterCount()} different characters!");
				}
					break;

				default:
					await Context.Channel.SendMessageAsync("Unknown statistic!");
					break;
			}
		}

		private static int GetTotalTagCount()
		{
			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);
			var tags = new List<string>();

			foreach (var image in images)
			{
				foreach (var tag in image.Tags)
				{
					var tagString = tag.ToLower();
					if (!tags.Contains(tagString))
						tags.Add(tagString);
				}
			}

			return tags.Count;
		}

		private static int GetTotalArtistCount()
		{
			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);
			var artists = new List<string>();

			foreach (var image in images)
			{
				if (image.Artists == null) continue;

				foreach (var artist in image.Artists)
				{
					var artistString = artist.ToLower();
					if (!artists.Contains(artistString))
						artists.Add(artistString);
				}
			}

			return artists.Count;
		}

		private static int GetTotalCopyrightCount()
		{
			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);
			var copyrights = new List<string>();

			foreach (var image in images)
			{
				if (image.Copyrights == null) continue;

				foreach (var copyright in image.Copyrights)
				{
					var copyrightString = copyright.ToLower();
					if (!copyrights.Contains(copyrightString))
						copyrights.Add(copyrightString);
				}
			}

			return copyrights.Count;
		}

		private static int GetTotalCharacterCount()
		{
			var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
			var json = File.ReadAllText(imageInfoPath);
			var images = JsonConvert.DeserializeObject<List<Image>>(json);
			var characters = new List<string>();

			foreach (var image in images)
			{
				if (image.Characters == null) continue;

				foreach (var character in image.Characters)
				{
					var characterString = character.ToLower();
					if (!characters.Contains(characterString))
						characters.Add(characterString);
				}
			}

			return characters.Count;
		}

		[Command("yuboorustat")]
		[Alias("yuboorustats")]
		public async Task YubooruStats(string stat, params string[] tags)
		{
			if (!Guilds.GetGuild(Context.Guild).GetChannel(Context.Channel).IsNsfw)
			{
				await Context.Channel.SendMessageAsync("Th-That's for NSFW channels! You lewdie!!!");
				return;
			}

			// ToLower() is bad practice, but switches only use ordinal comparison
			switch (stat.ToLower())
			{
				case "total":
					var imageInfoPath = Config.Bot.YubooruLocation + "\\ImageInfo.json";
					var json = File.ReadAllText(imageInfoPath);
					var images = JsonConvert.DeserializeObject<List<Image>>(json);

					var matchingImages = new List<Image>();

					Search(tags, images, matchingImages);

					var tagsString = string.Join(" + ", tags);

					await Context.Channel.SendMessageAsync($"Yubooru currently contains {matchingImages.Count} images of tags \"{tagsString}\"!");
					break;

				default:
					await Context.Channel.SendMessageAsync("Unknown statistic!");
					break;
			}
		}
	}
}
