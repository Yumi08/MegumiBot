using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MegumiBot.Core.Responses
{
	/// <summary>
	/// Static class for getting bot responses/alerts from the json file.
	/// </summary>
	public static class ResponseGetter
	{
		private static Dictionary<string, List<string>> _responses;

		private static string _responsesFile = "Resources/responses.json";

		static ResponseGetter()
		{
			_responses = DataStorage<Dictionary<string, List<string>>>.SaveExists(_responsesFile) ?
				JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(_responsesFile)) :
				new Dictionary<string, List<string>>();
		}

		/// <summary>
		/// Get a specific response by its key and its index.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string GetResponse(string key, ushort index)
		{
			return _responses[key][index];
		}

		/// <summary>
		/// Get a random response from the list attached its key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetRandomResponse(string key)
		{
			var count = _responses[key].Count;

			return _responses[key][Global.Random.Next(count)];
		}
	}
}
