using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductionBase : BuildingBase {

    #region Public Variables

    /// <summary>
    /// The time it takes to generate the product.
    /// </summary>
    [Tooltip("The time it takes to generate the product.")]
    public float ProductionTime;

    /// <summary>
    /// The product/s that this building creates.
    /// </summary>
    [Tooltip("The product/s that this building creates.")]
    public List<Products> Output = new List<Products>();

    /// <summary>
    /// Storage for products this building creates.
    /// </summary>
    [Tooltip("Storage for products this building creates.")]
    public List<Products> OutputStorage = new List<Products>();

    #endregion

}
