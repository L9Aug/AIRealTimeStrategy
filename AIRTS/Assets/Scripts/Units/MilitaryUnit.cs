using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;
using Condition;

public class MilitaryUnit : BaseUnit
{
    public float attackSpeed;
    public float attackDuration;
    public float damage;
    public int attackRange;

    public GameObject target;

    // States
    State idle;
    State attack;

    #region Transitions
    // From Attack
    Transition targetKilled; // To idle
    Transition cancelAttack; // To idle
    Transition enemyLeftRange;// To move

    // From Idle
    Transition enemyInRange;// To Attack
    Transition havePath; // To Move

    // From Move
    Transition lostTarget;// To Idle
    Transition reachedDestination; // To idle
    Transition reachedEnemy; // To Attack
    #endregion

    #region Conditions
    NullObject<GameObject> targetNull = new NullObject<GameObject>(); // For lost target
    
    // For enemy out of range
    AndCondition targetOutOfRange = new AndCondition(); 
    NotCondition haveTarget = new NotCondition();
    NullObject<GameObject> targetNotNull = new NullObject<GameObject>();
    AGreaterThanB outOfRange = new AGreaterThanB();

    // Cancel attack needs a condition, not sure what it is

    // For Enemy in range
    AndCondition canAttackEnemy = new AndCondition();
    ALessThanB inAttackRange = new ALessThanB();

    // For have path
    ListHasDataCond<HexTile> pathNotEmpty = new ListHasDataCond<HexTile>();

    // For reached destination
    NotCondition pathEmpty = new NotCondition();

    // For reached enemy
    AndCondition reachedTarget = new AndCondition();

    #endregion

    void Start()
    {

    }

    void SetupStateMachine()
    {
        // Define Conditions

        // Create Transitions

        // Create States

        // Assign target states to transitions

        // Setup state machine

        unitStateMachine.InitMachine();
    }

}
