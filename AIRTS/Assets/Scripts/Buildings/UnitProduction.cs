using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitProduction : BuildingBase {

    #region Public Variables

    /// <summary>
    /// The time it takes to generate the product.
    /// </summary>
    [Tooltip("The time it takes to generate the product.")]
    public float ProductionTime;

    /// <summary>
    /// The unit/s that this building creates.
    /// </summary>
    [Tooltip("The unit/s that this building creates.")]
    public List<UnitOutputClass> UnitOutput = new List<UnitOutputClass>();

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The products required for this building to function.")]
    public List<InputRequirements> inputRequirements = new List<InputRequirements>();

    /// <summary>
    /// Product storage whilst this build is producing a product.
    /// </summary>
    [Tooltip("Product storage whilst this build is producing a product.")]
    public List<Products> InputStorage = new List<Products>();

    #endregion

    #region Classes

    [System.Serializable]
    public class UnitOutputClass
    {
        public List<Units> Units = new List<Units>();
    }

    #endregion
}
