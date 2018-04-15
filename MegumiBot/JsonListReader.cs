using System.Collections.Generic;
using System.IO;

namespace MegumiBot
{
	public class JsonListReader<T>
	{
		public JsonListReader(string filePath)
		{
			FilePath = filePath;
		}

		private List<T> _itemList = new List<T>();

		public string FilePath { get; }

		public string ReadAllText()
		{
			return GetOrCreateFile(FilePath);
		}

		public void Add(T item)
		{
			_itemList.Add(item);
		}

		public List<T> ItemList => _itemList;

		private string GetOrCreateFile(string filePath)
		{
			if (!File.Exists(FilePath))
			{
				File.WriteAllText(filePath, "{}");
				return "{}";
			}

			return File.ReadAllText(filePath);
		}
	}
}
