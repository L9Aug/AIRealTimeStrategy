// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Condition;
using Decisions;
using SM;

namespace DT
{
    public interface INode { }

    /// <summary>
    /// Currently only a binary tree.
    /// </summary>
    public class DecisionTree
    {
        Vertex rootNode;

        public DecisionTree(Vertex RootNode)
        {
            rootNode = RootNode;
        }

        public void RunTree()
        {
            List<Action> actions = rootNode.TraverseVertex();
            foreach(Action action in actions)
            {
                action();
            }
        }

    }

    public class Vertex : INode
    {
        ADecision decision;
        List<INode> Nodes = new List<INode>();

        public Vertex(ADecision Decision, params INode[] nodes)
        {
            decision = Decision;
            Nodes.AddRange(nodes);
        }

        public List<Action> TraverseVertex()
        {
            INode branch = GetBranch;
            if(branch is Vertex)
            {
                return ((Vertex)branch).TraverseVertex();
            }
            else
            {
                return ((Leaf)branch).Actions;
            }
        }
        
        public INode GetBranch
        {
            get
            {
                object temp = decision.Test();

                if(temp is bool)
                {
                    return ((bool)temp) ? Nodes[0] : Nodes[1];
                }

                if(temp is int)
                {
                    return Nodes[(int)temp];
                }

                if(temp is System.Enum)
                {
                    return Nodes[(int)temp];
                }

                return null;
            }
        }
    }

    public class Leaf : INode
    {
        public List<Action> Actions = new List<Action>();

        public Leaf(params Action[] actions)
        {
            Actions.AddRange(actions);
        }
    }

}