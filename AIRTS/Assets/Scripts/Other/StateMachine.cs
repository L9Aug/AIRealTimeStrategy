using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SM
{

    public delegate void Action();

    public class StateMachine
    {

        public List<State> States = new List<State>();
        public State InitialState;

        public State CurrentState;

        Transition triggeredTransition;

        public void InitMachine()
        {
            CurrentState = InitialState;
            for (int i = 0; i < CurrentState.EntryActions.Count; ++i)
            {
                CurrentState.EntryActions[i]();
            }
            Debug.Log(CurrentState.StateName);
        }

        public void SMUpdate()
        {
            triggeredTransition = null;
            List<Action> ReturnList = new List<Action>();

            // Go through each possible transition until one if found to be triggered.
            foreach (Transition transition in CurrentState.Transitions)
            {
                if (transition.IsTriggered)
                {
                    triggeredTransition = transition;
                    break;
                }
            }

            // If a transition has been triggered queue up the necessary actions.
            if (triggeredTransition != null)
            {
                State targetState = triggeredTransition.TargetState;
                ReturnList.AddRange(CurrentState.ExitActions);
                ReturnList.AddRange(triggeredTransition.Actions);
                ReturnList.AddRange(targetState.EntryActions);

                CurrentState = targetState;

                Debug.Log(triggeredTransition.TransistionName);
                Debug.Log(CurrentState.StateName);
            }
            else // If no transition has happened continue with this states actions.
            {
                ReturnList.AddRange(CurrentState.Actions);
            }
            
            foreach(Action a in ReturnList)
            {
                a();
            }
        }
    }

    public interface ICondition
    {
        bool Test();
    }

    public class State
    {
        public string StateName;
        public List<Transition> Transitions = new List<Transition>();
        public List<Action> EntryActions = new List<Action>();
        public List<Action> Actions = new List<Action>();
        public List<Action> ExitActions = new List<Action>();
    }

    public class Transition
    {
        public string TransistionName;
        public List<Action> Actions = new List<Action>();
        State targetState;
        public ICondition condition;

        public State TargetState
        {
            get
            {
                return targetState;
            }
            set
            {
                targetState = value;
            }
        }
        public bool IsTriggered
        {
            get
            {
                return condition.Test();
            }
        }
    }

    /// <summary>
    /// Test Value between Min and Max values
    /// </summary>
    public class FloatCondition : ICondition
    {
        public float MinValue;
        public float MaxValue;

        public delegate float FloatParam();
        public FloatParam TestValue;

        bool ICondition.Test()
        {
            return (MinValue <= TestValue()) && (TestValue() <= MaxValue);
        }
    }

    /// <summary>
    /// A less than or equal to B
    /// </summary>
    public class LessThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() <= B();
        }
    }

    /// <summary>
    /// A greater than or equal to B
    /// </summary>
    public class GreaterThanFloatCondition : ICondition
    {
        public delegate float FloatParam();
        public FloatParam A;
        public FloatParam B;

        bool ICondition.Test()
        {
            return A() >= B();
        }
    }

    public class BoolCondition : ICondition
    {
        public delegate bool BoolParam();
        public BoolParam Condition;

        bool ICondition.Test()
        {
            return Condition();
        }
    }

    public class AndCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() && ConditionB.Test();
        }
    }

    public class OrCondition : ICondition
    {
        public ICondition ConditionA;
        public ICondition ConditionB;

        bool ICondition.Test()
        {
            return ConditionA.Test() || ConditionB.Test();
        }
    }

    public class NotCondition : ICondition
    {
        public ICondition Condition;

        bool ICondition.Test()
        {
            return !Condition.Test();
        }
    }

    /// <summary>
    /// True if null
    /// </summary>
    public class NullCondition : ICondition
    {
        public delegate Object ObjectParam();
        public ObjectParam Condition;

        bool ICondition.Test()
        {
            return Condition() == null;
        }
    }
}