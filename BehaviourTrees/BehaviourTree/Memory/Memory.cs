using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Memory
{
	public interface IMemory
	{
		void Store<T>(string key, T value);
		bool TryGet<T>(string key, out T value);
	}

	public class DictionaryMemory : IMemory
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();

		public void Store<T>(string key, T value)
		{
			dict[key] = value;
		}

		public bool TryGet<T>(string key, out T value)
		{
			if (dict.TryGetValue(key, out var obj))
			{
				value = (T)obj;
				return true;
			}
			else
			{
				value = default(T);
				return false;
			}
		}
	}
}
