using BehaviourTrees.BehaviourTree.Node.Decorator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Composite
{
	public class ParallelAny : INode
	{
		List<INode> nodes;

		public ParallelAny(List<INode> nodes)
		{
			this.nodes = nodes;
		}

		public async Task<bool> Execute(IContext context)
		{
			var tasks = nodes.Select(node => new Scope(node).Execute(context));

			await Task.WhenAll(tasks);

			return tasks.Any(t => t.Result);
		}
	}
}
