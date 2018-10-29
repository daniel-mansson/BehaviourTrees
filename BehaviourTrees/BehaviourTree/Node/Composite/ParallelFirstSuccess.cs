using BehaviourTrees.BehaviourTree.Node.Decorator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Composite
{
	/// <summary>
	/// Danger! WIP! Cancellation not working
	/// </summary>
	public class ParallelFirstSuccess : INode
	{
		List<INode> nodes;

		public ParallelFirstSuccess(List<INode> nodes)
		{
			this.nodes = nodes;
		}

		public async Task<bool> Execute(IContext context)
		{
			var tasks = nodes.Select(node => new Scope(node).Execute(context));
			var tcs = new TaskCompletionSource<bool>();

			//Only wait for the first successful task
			var ignore = Task.WhenAll(tasks.Select(async task => 
			{
				var result = await task;
				if (result)
				{
					tcs.TrySetResult(result);
				}
			})).ContinueWith(task => 
			{
				tcs.TrySetResult(false);
				return Task.CompletedTask;
			});

			await tcs.Task;

			return tasks.Any(t => t.Result);
		}
	}
}
