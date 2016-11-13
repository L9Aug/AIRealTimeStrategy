using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Production : ProductionBase {

    #region Public Variables

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The products required for this building to function.")]
    public List<Products> InputRequirements = new List<Products>();

    /// <summary>
    /// Product storage whilst this build is producing a product.
    /// </summary>
    [Tooltip("Product storage whilst this build is producing a product.")]
    public List<Products> InputStorage = new List<Products>();

    #endregion

}
