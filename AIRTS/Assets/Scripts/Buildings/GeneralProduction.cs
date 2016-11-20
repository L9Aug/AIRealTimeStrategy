using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The products required for this building to function.")]
    public List<Products> ProductionRequirements = new List<Products>();

    /// <summary>
    /// Storage for production resources.
    /// </summary>
    [HideInInspector]
    public List<Products> ProductionStorage = new List<Products>();

    /// <summary>
    /// The product this building creates.
    /// </summary>
    [Tooltip("The product this building creates.")]
    public Products OutputProduct;

    /// <summary>
    /// Storage for completed products.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> OutputStorage = new List<StorageItem>();

    #endregion

    #endregion



}
