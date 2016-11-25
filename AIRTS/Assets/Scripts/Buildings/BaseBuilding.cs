using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBuilding : MonoBehaviour
{

    #region Variables

    #region Public 

    /// <summary>
    /// This Tiles HexTransform.
    /// </summary>
    public HexTransform hexTransform;

    /// <summary>
    /// The gameobject that has the 3D model under it.
    /// </summary>
    [Tooltip("The gameobject that has the 3D model under it.")]
    public GameObject OperationalModelData;

    /// <summary>
    /// The tiles that make up the exclusion zone for this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> exclusionZone = new List<HexTile>();

    /// <summary>
    /// The tiles that act as the entrance to this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> EntanceTiles = new List<HexTile>();

    /// <summary>
    /// The tiles that make up the area of this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> BuildingArea = new List<HexTile>();

    /// <summary>
    /// The ID of the team that this building belongs to.
    /// </summary>
    [Tooltip("The ID of the team that this building belongs to.")]
    public float TeamID;

    /// <summary>
    /// The type of building that this is.
    /// </summary>
    [Tooltip("The type of building this is.")]
    public Buildings BuildingType;

    /// <summary>
    /// The tier of this building.
    /// </summary>
    [Tooltip("The tier of this building.")]
    [Range(0, 4)]
    public int Tier;

    /// <summary>
    /// The size of this building.
    /// </summary>
    [Tooltip("The Size of this building.")]
    [Range(1, 5)]
    public int Size;

    /// <summary>
    /// The time it takes to build this building.
    /// </summary>
    [Tooltip("The time it takes to build this building.\n T0 : 0s\n T1 : 5s\n T2 : 10s\n T3 : 20s\n T4 : 40s")]
    [Range(0, 40)]
    public float ConstructionTime;

    /// <summary>
    /// The maximum health this building can have.
    /// </summary>
    [Tooltip("The maximum health this building can have.")]
    public float MaxHealth;

    /// <summary>
    /// The current health this building has.
    /// </summary>
    [Tooltip("The current health this building has.")]
    public float Health;

    /// <summary>
    /// The mode of production.
    /// ie. Will a barracks make Soldiers(0) or Arhers(1).
    /// 0 by default.
    /// </summary>
    [Tooltip("The mode of production.\nie. Will a barracks make Soldiers(0) or Arhers(1).\n0 by default.")]
    [Range(0, 1)]
    public int ProductionMode = 0;

    #endregion

    #region Private

    /// <summary>
    /// The time left for the construction of the building.
    /// </summary>
    private float ConstructionTimer = 0;

    /// <summary>
    /// The object that hold the 3D model data for construction. 
    /// </summary>
    private GameObject ConstructionObject;

    #endregion

    #endregion

    #region Classes

    #region Public  

    public class StorageItem
    {
        private Products Product;
        private bool Reserved = false;
        private float ReserveID = 0;

        /// <summary>
        /// Test to see if the product is what we are searching for
        /// </summary>
        /// <param name="Reserve">If the product is what we are searching for do we want to reserve it.</param>
        /// <param name="SearchingFor">The products we are searching for.</param>
        /// <returns>Returns the ReserveID unless this product is not what we are searching for or if it has already been reserved.</returns>
        public float TestProduct(bool Reserve, params Products[] SearchingFor)
        {
            if (!Reserved)
            {
                foreach (Products p in SearchingFor)
                {
                    if (Product == p)
                    {
                        if (Reserve)
                        {
                            Reserved = true;
                            ReserveID = GenerateID();
                        }
                        return ReserveID;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Generates a unique ID using the time from the start of the game.
        /// </summary>
        /// <returns></returns>
        private float GenerateID()
        {
            return Time.realtimeSinceStartup;
        }

        /// <summary>
        /// Un-Reserves the product if it has this ID.
        /// </summary>
        /// <param name="ID"></param>
        public void UnreserveProduct(float ID)
        {
            if(ReserveID == ID)
            {
                Reserved = false;
                ReserveID = 0;
            }
        }
    
    }

    #endregion

    #endregion

    #region Functions

    #region Public

    public void ConfigureBuilding(int Q, int R)
    {
        hexTransform = new HexTransform(Q, R);
    }

    public void ConfigureBuilding(float Q, float R)
    {
        ConfigureBuilding((int)Q, (int)R);
    }

    public void ConfigureBuilding(Vector2 Loc)
    {
        ConfigureBuilding((int)Loc.x, (int)Loc.y);
    }

    public virtual void BuildingUpdate() { }

    #endregion

    #region Private

    private void BeginConstruction()
    {
        // Create appropriate 3D model for being under construction.

        // Hide the operational model data.
        OperationalModelData.SetActive(false);

        ConstructionTimer = ConstructionTime;
    }

    private void ConstructionUpdate()
    {
        ConstructionTimer -= Time.deltaTime;
    }

    private void BeginOperational()
    {
        // Here change the update the model to the actual building model rather than the construction model.
        OperationalModelData.SetActive(true);

    }

    private void OperationalUpdate()
    {
        BuildingUpdate();
    }

    private bool HasConstructionFinished()
    {
        if(ConstructionTimer <= 0)
        {
            return true;
        }
        return false;
    }

    private void SetUpStateMachine()
    {
        // Configure Transistion requiremnts.
        Condition.BoolCondition ConstructionCondition = new Condition.BoolCondition();
        ConstructionCondition.Condition = HasConstructionFinished;

        // Create Transitions.
        SM.Transition ConstructionFinished = new SM.Transition("Construction Finished", ConstructionCondition, new List<SM.Action>());

        // Create States.
        SM.State UnderConstruction = new SM.State("Under Construction", 
            new List<SM.Transition>() { ConstructionFinished }, 
            new List<SM.Action>() { BeginConstruction },
            new List<SM.Action>() { ConstructionUpdate },
            null);

        SM.State Operational = new SM.State("Operational",
            null,
            new List<SM.Action>() { BeginOperational },
            new List<SM.Action>() { OperationalUpdate },
            null);

        // Add target state to transitions
        ConstructionFinished.SetTargetState(Operational);

        BuildingStateMachine = new SM.StateMachine(null, UnderConstruction, Operational);    

        // Initialise the machine.
        BuildingStateMachine.InitMachine();
    }

    #endregion

    #endregion

    #region StateMachines

    protected SM.StateMachine BuildingStateMachine;

    #endregion

    // Use this for initialization
    void Start ()
    {
        SetUpStateMachine();
    }
	
	// Update is called once per frame
	void Update ()
    {
        BuildingStateMachine.SMUpdate();
	}

}
