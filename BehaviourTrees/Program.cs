using BehaviourTrees.BehaviourTree;
using BehaviourTrees.BehaviourTree.Abstract;
using BehaviourTrees.BehaviourTree.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees
{
	class Program
	{
		static string input = @"
REPEAT :Start
	SEQUENCE
		Echo ""staring here""
		Wait 1000
		Echo ""then here""
		Wait 500
		CALL Hello
		CALL Hello

SEQUENCE :Hello
	Echo ""Hello there""
	Wait 1000

";

		static void Main(string[] args)
		{
			var tokenizer = new Tokenizer();
			var tokens = tokenizer.Tokenize(input);

			Console.WriteLine("== TOKENS ==");
			foreach (var token in tokens)
			{
				if (token.Id == "NEWLINE")
					Console.WriteLine($"NL");
				else if (token.Id == "TAB")
					Console.Write($".");
				else
					Console.Write($"{token.Id}({token.Value}) ");
			}

			var parser = new Parser();
			var root = parser.Parse(tokens);

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("== AST ==");
			PrintRawAst(root);

			var tree = new BehaviourTree.Tree();
			tree.Build(root);

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("== TREE ==");

			var node = tree.GetNodeByLabel("Start");
			node.Execute(new Context(() => new DictionaryMemory())).Wait();
		}

		static void PrintRawAst(AbstractNode node)
		{
			foreach (var child in node.Children)
			{
				for (int i = 0; i < child.Indent; i++)
				{
					Console.Write(".");
				}

				Console.WriteLine(string.Join(" ", child.Raw.Select(t => $"{t.Id}({t.Value})")));
				PrintRawAst(child);
			}
		}
	}
}
