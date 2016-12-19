// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Decisions;
using SM;

namespace DT
{
    public interface INode { }

    public class DecisionTree
    {
        /// <summary>
        /// The root of the tree.
        /// </summary>
        Vertex rootNode;

        /// <summary>
        /// Constructor for the Decision tree.
        /// </summary>
        /// <param name="RootNode">The root of the tree.</param>
        public DecisionTree(Vertex RootNode)
        {
            rootNode = RootNode;
        }

        /// <summary>
        /// Traverses the tree and executes any actions that are reached.
        /// </summary>
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
        IDecision decision;
        List<INode> Nodes = new List<INode>();

        /// <summary>
        /// Constructor for the vertex class, must have at least two nodes.
        /// </summary>
        /// <param name="Decision">The decision that determines which branch to go down.</param>
        /// <param name="FirstNode">The first branch, this will be used for: True if boolean and 0 if numeric.</param>
        /// <param name="SecondNode">The second branch, this will be used for: False if boolean and 1 if numeric.</param>
        /// <param name="AdditionalNodes">Any additional branches are put in here and will continue the numeric sequence.</param>
        public Vertex(IDecision Decision, INode FirstNode, INode SecondNode, params INode[] AdditionalNodes)
        {
            decision = Decision;
            Nodes.Add(FirstNode);
            Nodes.Add(SecondNode);
            Nodes.AddRange(AdditionalNodes);
        }

        public List<Action> TraverseVertex()
        {
            // store which branch to go down.
            INode branch = GetBranch;
            if(branch is Vertex)
            {
                // if the branch is a vertex then traverse that vertex.
                return ((Vertex)branch).TraverseVertex();
            }
            else
            {
                // if the branch is a leaf then return the actions in that leaf.
                return ((Leaf)branch).Actions;
            }
        }
        
        public INode GetBranch
        {
            get
            {
                // run the decision and store it to prevent it running multiple times.
                object temp = decision.Test();

                if(temp is bool)
                {
                    // if the decision returned a boolean then return the true node (0) or the false node (1)
                    return ((bool)temp) ? Nodes[0] : Nodes[1];
                }

                if(temp is int)
                {
                    // if the decision returned an integer then return the branch with the same index as returned.
                    return Nodes[(int)temp];
                }

                if(temp is System.Enum)
                {
                    // if the decision returned an Enum then return the branch with the same index as the enum value.
                    return Nodes[(int)temp];
                }

                return null;
            }
        }
    }

    public class Leaf : INode
    {
        public List<Action> Actions = new List<Action>();

        /// <summary>
        /// Constructor for a leaf
        /// </summary>
        /// <param name="actions">A list of actions to be executed when the tree reaches this leaf.</param>
        public Leaf(params Action[] actions)
        {
            Actions.AddRange(actions);
        }
    }

}