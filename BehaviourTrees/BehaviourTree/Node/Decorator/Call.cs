using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class Call : INode
	{
		INode node;

		public void SetNodeDeferred(INode node)
		{
			this.node = node;
		}

		public Task<bool> Execute(IContext context)
		{
			return node.Execute(context);
		}
	}
}
