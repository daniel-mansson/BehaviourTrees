using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Abstract
{
	public class AbstractNode
	{
		public List<Tokenizer.Token> Raw { get; set; }
		public string Type { get; set; }
		public List<AbstractNode> Children { get; set; }
		public int Indent { get; set; }
	}
}
