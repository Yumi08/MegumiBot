using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MegumiBot.Core.Responses
{
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

		public static string GetResponse(string key, ushort index)
		{
			return _responses[key][index];
		}

		public static string GetRandomResponse(string key)
		{
			var count = _responses[key].Count;

			return _responses[key][Global.Random.Next(count)];
		}
	}
}
