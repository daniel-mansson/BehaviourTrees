using System;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node.Leaf
{
	public class Wait : INode
	{
		Func<int> milliseconds;

		public Wait(Func<int> milliseconds)
		{
			this.milliseconds = milliseconds;
		}

		public async Task<bool> Execute(IContext context)
		{
			var ms = milliseconds?.Invoke() ?? 0;

			await Task.Delay(ms);

			return true;
		}
	}
}
