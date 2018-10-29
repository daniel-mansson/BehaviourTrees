using BehaviourTrees.BehaviourTree.Abstract;
using BehaviourTrees.BehaviourTree.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourTrees.BehaviourTree
{
	public class Tree
	{
		class ConcreteNode
		{
			public AbstractNode abstractNode;
			public INode behaviourNode;
		}

		Dictionary<string, INode> labelLookup = new Dictionary<string, INode>();
		Dictionary<string, Type> builtInLookup = new Dictionary<string, Type>();
		List<KeyValuePair<AbstractNode, Node.Decorator.Call>> deferredCallNodes = new List<KeyValuePair<AbstractNode, Node.Decorator.Call>>();

		public Tree()
		{
			BuildBuiltInNodeLookup();
		}

		void BuildBuiltInNodeLookup()
		{
			builtInLookup = new Dictionary<string, Type>();

			var assembly = Assembly.GetAssembly(typeof(INode));
			var types = assembly.GetTypes().Where(t => typeof(INode).IsAssignableFrom(t) && !t.IsAbstract);

			foreach (var type in types)
			{
				var name = type.Namespace.Contains("Leaf") ? type.Name : type.Name.ToUpper();

				builtInLookup[name] = type;
			}
		}

		public INode GetNodeByLabel(string label)
		{
			return labelLookup[label];
		}

		public void Build(AbstractNode root)
		{
			var topLevel = new List<INode>();

			foreach (var node in root.Children)
			{
				topLevel.Add(BuildRecursive(node));
			}

			foreach (var pair in deferredCallNodes)
			{
				var id = pair.Key.Raw[0].Value;
				var label = pair.Key.Raw[1].Value;

				if (labelLookup.TryGetValue(label, out var node))
				{
					pair.Value.SetNodeDeferred(node);
				}
				else
				{
					throw new GenerationException($"Node '{id}' could not find any node with label '{label}'.", pair.Key.Raw[1].MatchIndex);
				}
			}
			deferredCallNodes.Clear();
		}

		INode BuildRecursive(AbstractNode node)
		{
			var childNodes = new List<INode>();

			foreach (var child in node.Children)
			{
				childNodes.Add(BuildRecursive(child));
			}

			return CreateBehaviourNode(node, childNodes);
		}

		private INode CreateBehaviourNode(AbstractNode abstractNode, List<INode> childNodes)
		{
			var id = abstractNode.Raw[0].Value;

			if(id == "CALL")
			{
				//Special case for call lookups
				var type = typeof(Node.Decorator.Call);

				if (childNodes.Count != 0)
					throw new GenerationException($"Node '{id}' cannot have any child nodes.", abstractNode.Raw[0].MatchIndex);

				var node = new Node.Decorator.Call();

				deferredCallNodes.Add(new KeyValuePair<AbstractNode, Node.Decorator.Call>(abstractNode, node));

				return node;
			}
			else if (builtInLookup.TryGetValue(id, out var type))
			{
				//Built-in type
				var constructor = type.GetConstructors()[0];
				var parameters = constructor.GetParameters();
				var paramObjs = new object[parameters.Length];

				if (parameters.Length > 0)
				{
					int paramsStart = 0;

					if (parameters[0].ParameterType == typeof(INode))
					{
						paramsStart++;
						if (childNodes.Count != 1)
							throw new GenerationException($"Node '{id}' needs exactly one child node.", abstractNode.Raw[0].MatchIndex);

						paramObjs[0] = childNodes[0];
					}
					else if (parameters[0].ParameterType == typeof(List<INode>))
					{
						paramsStart++;
						if (childNodes.Count < 1)
							throw new GenerationException($"Node '{id}' needs at least one child node.", abstractNode.Raw[0].MatchIndex);

						paramObjs[0] = childNodes;
					}

					for (int i = paramsStart; i < parameters.Length; i++)
					{
						var p = parameters[i];

						string nodeParamData = string.Empty;
						if (1 + i - paramsStart < abstractNode.Raw.Count)
						{
							var ptoken = abstractNode.Raw[1 + i - paramsStart];
							if (ptoken.Id == "VALUE")
								nodeParamData = ptoken.Value;
						}

						//Expecting  Func<T> only
						var isExpectedFunc = p.ParameterType.Name.Contains("Func") && p.ParameterType.BaseType == typeof(System.MulticastDelegate) && p.ParameterType.GenericTypeArguments.Length == 1;
						if (!isExpectedFunc)
						{
							throw new GenerationException($"Unexpected parameter type in constructor for node type '{type.Name}'. Only Func<T> is supported", abstractNode.Raw[0].MatchIndex);
						}
						else
						{
							if (nodeParamData == string.Empty)
							{
								paramObjs[i] = null;
							}
							else
							{
								var genericType = p.ParameterType.GenericTypeArguments[0];

								var func = CreateFuncParameter(genericType, nodeParamData);
								if (func == null)
								{
									throw new GenerationException($"Parameter type ({genericType.Name}) in constructor for node type '{type.Name}' not supported.", abstractNode.Raw[0].MatchIndex);
								}

								paramObjs[i] = func;
							}
						}
					}
				}

				var node = (INode)Activator.CreateInstance(type, paramObjs);

				var labelToken = abstractNode.Raw.FirstOrDefault(t => t.Id == "LABEL");
				if (labelToken != null)
				{
					var label = labelToken.Value.Replace(":", "");

					if (labelLookup.ContainsKey(label))
					{
						throw new GenerationException($"Label '{label}' is already assigned to another node.", labelToken.MatchIndex);
					}

					labelLookup.Add(label, node);
				}

				return node;
			}
			else
			{
				throw new GenerationException($"Could not find node type '{id}'", abstractNode.Raw[0].MatchIndex);
			}
		}

		private object CreateFuncParameter(Type funcReturnType, string nodeParamData)
		{
			if (funcReturnType == typeof(int))
			{
				return new Func<int>(() => { return int.Parse(nodeParamData); });
			}
			else if (funcReturnType == typeof(string))
			{
				return new Func<string>(() => { return nodeParamData; });
			}

			return null;
		}
	}
}
