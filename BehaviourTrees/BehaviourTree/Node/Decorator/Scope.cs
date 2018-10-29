using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class Scope : INode
	{
		INode node;

		public Scope(INode node)
		{
			this.node = node;
		}

		public async Task<bool> Execute(IContext context)
		{
			using (var scopeContext = context.NewScope())
			{
				var result = await node.Execute(scopeContext);
				return !result;
			}
		}
	}
}
