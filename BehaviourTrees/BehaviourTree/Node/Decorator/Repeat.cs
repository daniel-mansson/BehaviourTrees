using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class Repeat : INode
	{
		INode node;
		Func<int> count;

		public Repeat(INode node, Func<int> count = null)
		{
			this.node = node;
			this.count = count;
		}

		public async Task<bool> Execute(IContext context)
		{
			int c = count?.Invoke() ?? 0;

			for (int i = 0; c <= 0 || i < c; i++)
			{
				await node.Execute(context);
			}

			return true;
		}
	}
}
