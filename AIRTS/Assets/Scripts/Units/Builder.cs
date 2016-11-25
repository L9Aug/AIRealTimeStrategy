using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Condition;
using SM;

public class Builder : BaseUnit
{
    public BaseBuilding assignedBuilding;
    public BaseBuilding homeBuilding;

    State moveToBuildingSite;
    State returnHome;

    AndCondition positionCorrect = new AndCondition();
    AEqualsB columnCorrect = new AEqualsB();
    AEqualsB rowCorrect = new AEqualsB();

    AGreaterThanB buildTimeComplete = new AGreaterThanB();

    // Use this for initialization
    void Start ()
    {
        setupStateMachine();
	}

    void ReturnHome()
    {
        // Add this to the building and the global count
        // Remove from the map list
        Destroy(gameObject);
    }

    void FindHome()
    {
        path = ASImplementation.ASI.AStar(MapGenerator.Map[(int)hexTransform.RowColumn.x, (int)hexTransform.RowColumn.y], MapGenerator.Map[(int)homeBuilding.hexTransform.RowColumn.x, (int)homeBuilding.hexTransform.RowColumn.y]);
    }

    void DestinationCheck()
    {
        columnCorrect.A = hexTransform.RowColumn.x;
        rowCorrect.A = hexTransform.RowColumn.y;

        if(unitStateMachine.CurrentState == moveToBuildingSite)
        {
            columnCorrect.B = assignedBuilding.hexTransform.RowColumn.y;
            rowCorrect.B = assignedBuilding.hexTransform.RowColumn.y;
        }
        if(unitStateMachine.CurrentState == returnHome)
        {
            columnCorrect.B = homeBuilding.hexTransform.RowColumn.y;
            rowCorrect.B = homeBuilding.hexTransform.RowColumn.y;
        }
    }

    void Build()
    {

    }

    private void setupStateMachine()
    {
        positionCorrect.A = columnCorrect;
        positionCorrect.B = rowCorrect;

        Transition arriveAtSite = new Transition("Arrived At Site", positionCorrect, new List<Action>());
        Transition buildingComplete = new Transition("Building Complete", buildTimeComplete, new List<Action>());

        moveToBuildingSite = new State("Moving To building Site",
            new List<Transition>() { arriveAtSite },
            null,
            new List<Action>() { Move, DestinationCheck },
            null);

        State build = new State("Constructing",
            new List<Transition>() { arriveAtSite },
            null,
            new List<Action>() { Build },
            null);

        returnHome = new State("Retuning Home",
            new List<Transition>(),
            null,
            null,
            null);

        arriveAtSite.SetTargetState(build);
        buildingComplete.SetTargetState(returnHome);

        unitStateMachine = new StateMachine(null, moveToBuildingSite, build, returnHome);

        unitStateMachine.InitMachine();
    }
}
