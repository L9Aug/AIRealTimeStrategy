// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Market : BaseBuilding
{

    #region Functions

    #region Protected

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Market))]
[CanEditMultipleObjects]
public class MarketEditor : BaseBuildingEditor
{
    private Market myMTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        myMTarget = (Market)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {

        }
    }
}
#endif