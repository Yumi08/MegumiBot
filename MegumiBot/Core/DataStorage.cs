using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace MegumiBot.Core
{
	// Written by Petrspelos
	public static class DataStorage<T>
	{
		/// <summary>
		/// Save an IEnumerable into a json file.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="filePath"></param>
		public static void SaveItems(IEnumerable<T> items, string filePath)
		{
			var json = JsonConvert.SerializeObject(items, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		/// <summary>
		/// Load an IEnumerable from a json file path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IEnumerable<T> LoadItems(string filePath)
		{
			if (!File.Exists(filePath)) return null;
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<T>>(json);
		}

		/// <summary>
		/// Check if a file exists for a certain path
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool SaveExists(string filePath)
		{
			return File.Exists(filePath);
		}
	}
}