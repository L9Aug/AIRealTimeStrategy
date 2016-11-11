using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The base class for buildings.
/// </summary>
public class BuildingBase : MonoBehaviour {

    #region Public Variables

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

    #endregion

    #region Public Classes

    [System.Serializable]
    public class InputRequirements
    {
        public List<Products> RequiredProducts = new List<Products>();
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
