using BehaviourTrees.BehaviourTree.Node.Decorator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Composite
{
	public class ParallelAll : INode
	{
		List<INode> nodes;

		public ParallelAll(List<INode> nodes)
		{
			this.nodes = nodes;
		}

		public async Task<bool> Execute(IContext context)
		{
			var tasks = nodes.Select(node => new Scope(node).Execute(context));

			await Task.WhenAll(tasks);

			return tasks.All(t => t.Result);
		}
	}
}
