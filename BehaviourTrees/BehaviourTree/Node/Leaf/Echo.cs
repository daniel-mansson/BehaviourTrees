using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Leaf
{
	public class Echo : INode
	{
		Func<string> text;

		public Echo(Func<string> text)
		{
			this.text = text;
		}

		public Task<bool> Execute(IContext context)
		{
			var t = text?.Invoke() ?? string.Empty;

			var replacedText = Regex.Replace(t, "({.*?})+", key => 
			{
				context.GlobalMemory.TryGet(key.Value, out object obj);
				return obj?.ToString() ?? $"({key}=null)";
			});

			Console.WriteLine(replacedText);

			return Task.FromResult(true);
		}
	}
}
