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

    #endregion

    #region Protected

    protected bool inProduction = false;

    protected float ProductionTimer = 0;

    protected float cumulativeDeltaTime = 0;

    #endregion

    #endregion

    #region Functions

    #region Public  

    public virtual void ProductionCycle()
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
        cumulativeDeltaTime += Time.deltaTime;
    }

    protected override void ConstructionFinished()
    {
        base.ConstructionFinished();
        StartCoroutine(DesicionTreeRunIntervals());
    }

    protected virtual IEnumerator DesicionTreeRunIntervals()
    {
        float WaitInterval = 0;

        while(ProductionTree != null)
        {
            ProductionCycle();

            WaitInterval = (inProduction) ? 0 : 1;

            yield return new WaitForSeconds(WaitInterval);
        }
    }

    protected object InProduction()
    {
        return inProduction;
    }

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
        return false;
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
            EditorGUILayout.LabelField("Production Time:", myBPTarget.ProductionTime.ToString());
        }
    }

}
#endif