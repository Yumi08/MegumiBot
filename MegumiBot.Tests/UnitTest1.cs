using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MegumiBot.Tests
{
	public class UnitTest1
	{
		// For populating responses.json
		[Test]
		public void ResponseTest()
		{
			var responses = new Dictionary<string, List<string>>();
			var list = new List<string> {"Test1", "Test2"};

			responses.Add("TestKey1", list);

			File.WriteAllText("C:\\Users\\Yumi\\source\\Console\\Discord\\MegumiBot\\MegumiBot\\bin\\Debug\\Resources\\responses.json", JsonConvert.SerializeObject(responses));
		}
	}
}
