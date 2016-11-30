using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour
{
    #region Variables

    #region Public

    /// <summary>
    /// This Tiles HexTransform.
    /// </summary>
    public HexTransform hexTransform;

    /// <summary>
    /// The ID of the team that this building belongs to.
    /// </summary>
    [Tooltip("The ID of the team that this building belongs to.")]
    public float TeamID;

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

    #endregion

}
