using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RawProduction : ProductionBase {

    #region Public Variables

    /// <summary>
    /// The terrain/s required for this building to perform.
    /// </summary>
    [Tooltip("The terrain/s required for this building to perform.")]
    public List<TerrainMode> TerrainRequirement = new List<TerrainMode>();

    #endregion

    #region Classes

    [System.Serializable]
    public class TerrainMode
    {
        public List<TerrainTypes> TerrainRequirment = new List<TerrainTypes>();
    }

    #endregion

}
