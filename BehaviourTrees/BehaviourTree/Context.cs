using BehaviourTrees.BehaviourTree.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree
{
	public interface IContext : IDisposable
	{
		IMemory GlobalMemory { get; }
		IMemory LocalMemory { get; }
		CancellationToken CancellationToken { get; }

		IContext NewScope();
	}

	public class Context : IContext
	{
		Func<IMemory> memoryFactory;
		CancellationTokenSource cts;

		public IMemory GlobalMemory { get; private set; }
		public IMemory LocalMemory { get; private set; }
		public CancellationToken CancellationToken => cts.Token;

		public Context(Func<IMemory> memoryFactory)
		{
			this.memoryFactory = memoryFactory;

			GlobalMemory = memoryFactory();
			LocalMemory = memoryFactory();
			cts = new CancellationTokenSource();
		}

		public void Dispose()
		{
			cts.Cancel();
		}

		public IContext NewScope()
		{
			return new Context(memoryFactory)
			{
				GlobalMemory = this.GlobalMemory
			};
		}
	}
}
