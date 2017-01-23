using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{

    public class State
    {
        public Condition.ICondition Condition;
        public List<State> Precondition;
        public List<SM.Action> Actions;
        public float cost;
    }

    public class GOAP : MonoBehaviour
    {
        public AStarInfo<State> Goal;
        public Queue<AStarInfo<State>> ActionPlan;
        public List<AStarInfo<State>> PossibleStates;

        AStarInfo<State> lastGoal;
        ASImplementation<State> Astar;

        public List<SM.Action> UpdateGOAP()
        {
            if (ActionPlan.Peek().current.Condition.Test())
            {
                ActionPlan.Dequeue();
            }
            
            if(Goal != lastGoal)
            {
                RecalculateActionPlan();
            }

            lastGoal = Goal;

            return ActionPlan.Peek().current.Actions;
        }

        void RecalculateActionPlan()
        {
            ActionPlan.Clear();
            List<AStarInfo<State>> states = Astar.AStar((x, y) => { return Goal.cost * 0.75f; });
            for(int i = 0; i < states.Count; ++i)
            {
                ActionPlan.Enqueue(states[i]);
            }
        }
    }
}