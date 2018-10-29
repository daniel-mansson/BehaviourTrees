using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree
{
	public class GenerationException : Exception
	{
		public int MatchIndex { get; set; }

		public GenerationException(string message, int matchIndex) :
			base(message)
		{
			MatchIndex = matchIndex;
		}
	}
}
