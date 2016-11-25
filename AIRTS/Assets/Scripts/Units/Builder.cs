using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Builder : BaseUnit
{
    public BuildingBase assignedBuilding;
    public BuildingBase homeBuilding;

    State moveToBuildingSite = new State();
    State build = new State();
    State returnHome = new State();

    Transition arriveAtSite = new Transition();
    Transition buildingComplete = new Transition();
    Transition arrivedHome = new Transition();

    AndCondition positionCorrect = new AndCondition(); 
    AEqualsB columnCorrect = new AEqualsB();
    AEqualsB rowCorrect = new AEqualsB();
    
    AGreaterThanB buildTimeComplete = new AGreaterThanB();

	// Use this for initialization
	void Start ()
    {
        positionCorrect.A = columnCorrect;
        positionCorrect.B = rowCorrect;

        arriveAtSite.condition = positionCorrect;
        arriveAtSite.TargetState = build;
        arriveAtSite.Action = new List<Action> { NullAction };

        buildingComplete.Action = new List<Action> { NullAction };
        buildingComplete.condition = buildTimeComplete;
        buildingComplete.TargetState = returnHome;

        arrivedHome.condition = positionCorrect;
        arrivedHome.Action = new List<Action> { ReturnHome };
        arrivedHome.TargetState = null;

        moveToBuildingSite.action = new List<Action> { Move, DestinationCheck };
        moveToBuildingSite.entryAction = NullAction;
        moveToBuildingSite.exitAction = NullAction;
        moveToBuildingSite.transitions = new List<Transition> { arriveAtSite };

        build.action = new List<Action> { Build };
        build.entryAction = NullAction;
        build.exitAction = NullAction;
        build.transitions = new List<Transition> { buildingComplete };

        returnHome.action = new List<Action> { Move, DestinationCheck };
        returnHome.entryAction = FindHome;
        returnHome.exitAction = NullAction;
        returnHome.transitions = new List<Transition> { };

        unitStateMachine.states = new List<State> { moveToBuildingSite, build, returnHome };
        unitStateMachine.initialState = moveToBuildingSite;
	}

    void ReturnHome()
    {
        // Add this to the building and the global count
        // Remove from the map list
        Destroy(gameObject);
    }

    void FindHome()
    {
        path = ASImplementation.ASI.AStar(MapGenerator.Map[hexTransform.Q, hexTransform.R], MapGenerator.Map[homeBuilding.hexTransform.Q, homeBuilding.hexTransform.R]);
    }

    void DestinationCheck()
    {
        columnCorrect.A = hexTransform.Q;
        rowCorrect.A = hexTransform.R;

        if(unitStateMachine.currentState == moveToBuildingSite)
        {
            columnCorrect.B = assignedBuilding.hexTransform.Q;
            rowCorrect.B = assignedBuilding.hexTransform.R;
        }
        if(unitStateMachine.currentState == returnHome)
        {
            columnCorrect.B = homeBuilding.hexTransform.Q;
            rowCorrect.B = homeBuilding.hexTransform.R;
        }
    }

    void Build()
    {

    }
}
