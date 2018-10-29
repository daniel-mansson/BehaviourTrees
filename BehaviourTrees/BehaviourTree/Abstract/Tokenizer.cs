using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Abstract
{
	public class Tokenizer
	{
		public class Token
		{
			public string Id;
			public string Value;
			public int MatchIndex;
			public int MatchLength;
		}

		Regex regex;

		public Tokenizer()
		{
			var combinedRegex = string.Join("|", new string[]
			{
				"(?<VALUE>\".*?\")",
				"(?<COMMENT>#.*)",
				"(?<TAB>\t)",
				"(?<NEWLINE>\n)",
				"(?<LABEL>:[a-zA-Z][a-zA-Z0-9]*)",
				"(?<ID>[a-zA-Z][a-zA-Z0-9]*)",
				"(?<EQUALS>=)",
				"(?<VALUE>[1-9][0-9]*.[0-9]+)",
				"(?<VALUE>[1-9][0-9]*)",
				"(?<VALUE>[a-zA-Z][a-zA-Z0-9]*)",
			});

			regex = new Regex(combinedRegex);
		}

		public List<Token> Tokenize(string input)
		{
			input = input.Replace("\r", "");

			var matches = regex.Matches(input);
			var result = new List<Token>();

			foreach (Match match in matches)
			{
				foreach (Group group in match.Groups)
				{
					if (group.Success)
					{
						string name = (string)group.GetType().GetProperty("Name").GetValue(group);
						if(name != "0")
						{
							result.Add(new Token()
							{
								Id = name,
								Value = match.Value,
								MatchIndex = match.Index,
								MatchLength = match.Length
							});
						}
					}
				}
			}

			return result;
		}
	}
}
