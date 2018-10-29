using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Composite
{
	public class Sequence : INode
	{
		List<INode> nodes;

		public Sequence(List<INode> nodes)
		{
			this.nodes = nodes;
		}

		public async Task<bool> Execute(IContext context)
		{
			foreach (var node in nodes)
			{
				var result = await node.Execute(context);
				if (!result)
				{
					return false;
				}
			}

			return true;
		}
	}
}
