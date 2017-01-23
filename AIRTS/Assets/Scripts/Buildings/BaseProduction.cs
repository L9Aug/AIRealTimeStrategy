// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseProduction : BaseBuilding
{

    #region Variables

    #region Public

    public float ProductionTime;

    public int CourierCount = 0;

    public List<GameEntity> OnMapCouriers = new List<GameEntity>();

    public float ProductionTimer = 0;

    public bool inProduction = false;

    #endregion

    #region Protected

    #endregion

    #endregion

    #region Functions

    #region Public  

    public virtual void TreeTick()
    {
        if(ProductionTree != null)
        {
            ProductionTree.RunTree();
        }
    }

    public override void BuildingUpdate()
    {
        base.BuildingUpdate();
    }

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void BeginOperational()
    {
        base.BeginOperational();
        StartCoroutine(DecisionTreeRunIntervals());
    }

    protected virtual IEnumerator DecisionTreeRunIntervals()
    {
        while(ProductionTree != null)
        {
            TreeTick();

            yield return null;
        }
    }

    protected object InProduction()
    {
        return inProduction;
    }

    /// <summary>
    /// Needs to be implemented still
    /// </summary>
    /// <returns></returns>
    protected StorageBuilding FindStorageBuilding()
    {
        return null;
    }

    protected object IsThereAStorageBuilding()
    {
        return (FindStorageBuilding() != null) ? true : false;
    }

    /// <summary>
    /// Requires implementation
    /// </summary>
    /// <returns></returns>
    protected object IsThereAnAvailableCourier()
    {
        return (CourierCount > 0) ? true : false;
    }

    protected virtual void BeginProduction()
    {
        inProduction = true;
        StartCoroutine(ProductionCycle());
    }

    protected virtual IEnumerator ProductionCycle()
    {
        yield return null;
    }

    #endregion

    #endregion

    #region Decicion Trees

    protected DT.DecisionTree ProductionTree;

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseProduction))]
[CanEditMultipleObjects]
public class BaseProductionEditor : BaseBuildingEditor
{

    private BaseProduction myBPTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        myBPTarget = (BaseProduction)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            EditorGUILayout.LabelField("Production Time:", myBPTarget.ProductionTimer.ToString("F2") + " / " + myBPTarget.ProductionTime.ToString());
        }
    }

}
#endif