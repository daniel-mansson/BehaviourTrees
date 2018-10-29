using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class Inverter : INode
	{
		INode node;

		public Inverter(INode node)
		{
			this.node = node;
		}

		public async Task<bool> Execute(IContext context)
		{
			var result = await node.Execute(context);

			return !result;
		}
	}
}
