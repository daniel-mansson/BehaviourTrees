using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Abstract
{
	public class Parser
	{
		public AbstractNode Parse(List<Tokenizer.Token> tokens)
		{
			var clean = tokens.Where(t => t.Id != "COMMENT").ToList();
			var groups = SplitGroup(clean, t => t.Id == "NEWLINE");
			groups.RemoveAll(g => g.Count == 0 || g.All(t => t.Id == "TAB"));

			Console.WriteLine(groups.Count);

			AbstractNode root = new AbstractNode()
			{
				Children = new List<AbstractNode>(),
				Indent = -1,
				Raw = new List<Tokenizer.Token>(),
			};

			var index = Parse(root, groups, 0);
			if (index != groups.Count)
			{
				Console.WriteLine("why?");
			}

			return root;
		}

		public int Parse(AbstractNode parent, List<List<Tokenizer.Token>> groups, int index)
		{
			while (index < groups.Count)
			{
				var tokens = groups[index];
				int indent = CountFirst(tokens, t => t.Id == "TAB");

				if (indent == parent.Indent + 1)
				{
					var child = new AbstractNode()
					{
						Indent = indent,
						Raw = tokens.Where(t => t.Id != "TAB").ToList(),
						Children = new List<AbstractNode>()
					};

					index = Parse(child, groups, index + 1);

					parent.Children.Add(child);
				}
				else
				{
					return index;
				}
			}

			return index;

		}

		static List<List<T>> SplitGroup<T>(IList<T> list, Func<T, bool> pred)
		{
			var result = new List<List<T>>();
			var current = new List<T>();

			foreach (var item in list)
			{
				if (pred(item))
				{
					if (current.Count != 0)
					{
						result.Add(current);
						current = new List<T>();
					}
				}
				else
				{
					current.Add(item);
				}
			}

			if (current.Count != 0)
				result.Add(current);

			return result;
		}

		static int CountFirst<T>(IList<T> list, Func<T, bool> pred)
		{
			int count = 0;

			foreach (var item in list)
			{
				if (!pred(item))
					break;

				count++;
			}

			return count;
		}
	}
}
