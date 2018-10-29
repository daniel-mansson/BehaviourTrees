using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class Succeeder : INode
	{
		INode node;

		public Succeeder(INode node)
		{
			this.node = node;
		}

		public async Task<bool> Execute(IContext context)
		{
			await node.Execute(context);

			return true;
		}
	}
}
