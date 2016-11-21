using UnityEngine;
using System.Collections;

public class MilitaryUnit : BaseUnit
{
    public float attackSpeed;
    public float attackDuration;
    public float damage;
    public int attackRange;

    public GameObject target;

    // States
    State idle = new State();
    State attack = new State();

    #region Transitions
    // From Attack
    Transition targetKilled = new Transition(); // To idle
    Transition cancelAttack = new Transition(); // To idle
    Transition enemyLeftRange = new Transition(); // To move

    // From Idle
    Transition enemyInRange = new Transition(); // To Attack
    Transition havePath = new Transition(); // To Move

    // From Move
    Transition lostTarget = new Transition(); // To Idle
    Transition reachedDestination = new Transition(); // To idle
    Transition reachedEnemy = new Transition(); // To Attack
    #endregion

    #region Conditions
    NullGameobject targetNull = new NullGameobject(); // For lost target
    
    // For enemy out of range
    AndCondition targetOutOfRange = new AndCondition(); 
    NotCondition haveTarget = new NotCondition();
    NullGameobject targetNotNull = new NullGameobject();
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


}
