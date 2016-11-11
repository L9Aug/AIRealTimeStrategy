using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageBuilding : BuildingBase {

    #region Public Variables

    /// <summary>
    /// The number of products this building can store.
    /// </summary>
    [Tooltip("The number of products this building can store.")]
    public int Capacity;

    /// <summary>
    /// The items stored in this buildings.
    /// </summary>
    [Tooltip("The items stored in this buildings.")]
    public List<Products> ItemsStored = new List<Products>();

    #endregion

}
