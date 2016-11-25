using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RawProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The terrain/s required for this building to perform.
    /// </summary>
    [Tooltip("The terrain/s required for this building to perform.")]
    public List<TerrainMode> TerrainRequirement = new List<TerrainMode>();

    /// <summary>
    /// The products that this building can create.
    /// </summary>
    [Tooltip("the products that this building can create.")]
    public List<Products> OutputProduct = new List<Products>();

    /// <summary>
    /// Storage For completed products
    /// </summary>
    [HideInInspector]
    public List<StorageItem> OutputStorage = new List<StorageItem>();

    #endregion

    #endregion

    #region Classes

    #region Public

    [System.Serializable]
    public class TerrainMode
    {
        public List<TerrainTypes> TerrainRequirment = new List<TerrainTypes>();
    }

    #endregion

    #endregion

}
