using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageBuilding : BaseBuilding
{

    #region Public Variables

    /// <summary>
    /// The number of products this building can store.
    /// </summary>
    [Tooltip("The number of products this building can store.")]
    public int Capacity;

    /// <summary>
    /// The items stored in this buildings.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> ItemsStored = new List<StorageItem>();

    #endregion

}
