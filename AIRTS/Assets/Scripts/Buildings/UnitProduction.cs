using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The Products required for this building to function.")]
    public List<InputRequirements> ProductionRequirements = new List<InputRequirements>();

    /// <summary>
    /// Storage for products that are using in production
    /// </summary>
    [HideInInspector]
    public List<Products> ProductionStorage = new List<Products>();

    /// <summary>
    /// The unit/s that this building creates.
    /// </summary>
    [Tooltip("The unit/s that this building creates.")]
    public List<UnitOutputClass> UnitOutput = new List<UnitOutputClass>();

    #endregion

    #endregion

    #region Classes

    #region Public

    [System.Serializable]
    public class UnitOutputClass
    {
        public List<Units> Units = new List<Units>();
    }

    [System.Serializable]
    public class InputRequirements
    {
        public List<Products> RequiredProducts = new List<Products>();
    }

    #endregion

    #endregion
}
