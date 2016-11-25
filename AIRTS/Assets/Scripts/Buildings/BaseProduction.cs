using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseProduction : BaseBuilding
{

    #region Variables

    #region Public

    public float ProductionTime;

    #endregion

    #endregion

    #region Functions

    #region Public  

    public virtual void ProductionCycle() { }

    #endregion

    #endregion

    #region Decicion Trees

    protected DT.DecisionTree ProductionTree;

    #endregion
}