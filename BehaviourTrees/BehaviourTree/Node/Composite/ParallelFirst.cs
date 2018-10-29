using BehaviourTrees.BehaviourTree.Node.Decorator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Composite
{
	/// <summary>
	/// Danger! WIP! Cancellation not working
	/// </summary>
	public class ParallelFirst : INode
	{
		List<INode> nodes;

		public ParallelFirst(List<INode> nodes)
		{
			this.nodes = nodes;
		}

		public async Task<bool> Execute(IContext context)
		{
			var tasks = nodes.Select(node => new Scope(node).Execute(context));

			var first = await Task.WhenAny(tasks);
			return first.Result;
		}
	}
}
