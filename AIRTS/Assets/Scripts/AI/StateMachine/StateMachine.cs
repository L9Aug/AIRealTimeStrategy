using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Action();

public class StateMachine {
    public List<State> states;
    public State initialState;
    public State currentState;
    Transition triggeredTransition;
    State targetState;

    void Awake()
    {
        currentState = initialState;
    }

    void Start()
    {
        currentState = initialState;
    }

    public List<Action> Update()
    {
        triggeredTransition = null;

        foreach(Transition transition in currentState.transitions)
        {
            if (transition.isTriggered)
            {
                triggeredTransition = transition;
                break;
            }
        }

        if(triggeredTransition != null)
        {
            targetState = triggeredTransition.TargetState;

            List<Action> actions = new List<Action> { currentState.exitAction, triggeredTransition.Action[0], targetState.entryAction };
            currentState = targetState;
            return actions;
        }
        else
        {
            return currentState.action;
        }
    }
}

public class State
{
    public Action entryAction;
    public List<Action> action;
    public Action exitAction;
    public List<Transition> transitions;

}

public class Transition
{
    State targetState;
    List<Action> actions;
    public ICondition condition;

    public bool isTriggered
    {
        get
        {
            return condition.Test();
        }
    }

    public List<Action> Action
    {
        get
        {
            return actions;
        }
        set
        {
            actions = value;
        }
    }

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
}
