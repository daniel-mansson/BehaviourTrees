using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Decorator
{
	public class RepeatUntilFail : INode
	{
		INode node;
		Func<int> count;

		public RepeatUntilFail(INode node, Func<int> count = null)
		{
			this.node = node;
			this.count = count;
		}

		public async Task<bool> Execute(IContext context)
		{
			int c = count?.Invoke() ?? 0;

			for (int i = 0; c <= 0 || i < c; i++)
			{
				var result = await node.Execute(context);
				if (!result)
				{
					return true;
				}
			}

			return true;
		}
	}
}
