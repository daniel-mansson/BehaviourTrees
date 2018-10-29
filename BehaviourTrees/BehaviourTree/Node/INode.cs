using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree.Node
{
	public interface INode
	{
		Task<bool> Execute(IContext context);
	}
}
