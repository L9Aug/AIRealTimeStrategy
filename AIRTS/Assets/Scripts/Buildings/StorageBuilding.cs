// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StorageBuilding : BaseBuilding
{

    #region Public Variables

    /// <summary>
    /// The number of products this building can store.
    /// </summary>
    [Tooltip("The number of products this building can store.")]
    public int Capacity;

    /// <summary>
    /// The items stored in this buildings.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> ItemsStored = new List<StorageItem>();

    #endregion

    #region Functions

    #region Public

    public List<Products> AddProduct(params Products[] products)
    {
        List<Products> returnList = new List<Products>();
        foreach (Products product in products)
        {
            if (ItemsStored.Count < Capacity)
            {
                ItemsStored.Add(new StorageItem(product));
            }
            else
            {
                returnList.Add(product);
            }
        }
        return returnList;
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

    #endregion

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(StorageBuilding))]
[CanEditMultipleObjects]
public class StorageBuildingEditor : BaseBuildingEditor
{
    private StorageBuilding mySBTarget;
    private static bool showSBStorage;
    private int previousCount;
    private bool previousShowSBStorage;

    

    protected override void OnEnable()
    {
        base.OnEnable();
        mySBTarget = (StorageBuilding)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            EditorGUILayout.LabelField("Capacity:", mySBTarget.Capacity.ToString());

            StorageInspector("Stored Items: ", ref showSBStorage, mySBTarget.ItemsStored, ref previousCount, ref previousShowSBStorage);         
        }
    }

}
#endif