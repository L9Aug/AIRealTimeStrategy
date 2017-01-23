// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region Global Enums

/// <summary>
/// The available products in the game.
/// </summary>
public enum Products { Wheat, Fruit, Fish, Wood, Water, Iron, Gold, Stone, Cotton, Cattle, Flour, Planks, Leather, Meat, Clothing, Wine, Bread, Swords, Armour, Bows, Paper, Culture, Food }    

/// <summary>
/// The available units in the game.
/// </summary>
public enum Units { Soldier, Archer, Catapult, Builder, Courier, Merchant, Citizen }

/// <summary>
/// The available buildings in the game.
/// </summary>
public enum Buildings { TownHall, Farm, Fishery, Mine, Orchard, Pasture, Plantation, Quarry, Sawmill, WaterWell, Butcher, Carpenter, House, Mill, Tailor, Tanner, Winery, Armoursmith, Bakery, Fletcher, Typography, Villa, Warehouse, Weaponsmith, Barracks, Library, Market, Workshop }

/// <summary>
/// The available terrain types in the game.
/// </summary>
public enum TerrainTypes { Plains, Grassland, Coast, Hills, Forest, Mountains, Desert, River, Lake, Sea }

#endregion

public class GlobalAttributes : MonoBehaviour
{
    /// <summary>
    /// Static reference to the Global Attributes object.
    /// </summary>
    public static GlobalAttributes Global;

    /// <summary>
    /// List of all buildings prefabs.
    /// In order according to the Buildings enum.
    /// </summary>
    [Tooltip("List of all building prefabs, in order according to the buildings enum.\nBuilding prefabs must be named acroding to the enum (Pascal case and no spaces)")]
    public List<BaseBuilding> Buildings = new List<BaseBuilding>();

    /// <summary>
    /// List of meshes for buildings whilst they are under construction
    /// </summary>
    [Tooltip("List of meshes for buildings whilst they are under construction")]
    public List<GameObject> ConstructionBuildings = new List<GameObject>();

    private void Start()
    {
        Global = this;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(GlobalAttributes))]
public class GlobalAttributesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GlobalAttributes myTarget = (GlobalAttributes)target;

        if(GUILayout.Button("Sort Buildings List"))
        {
            SortBuildings(myTarget);
        }
    }

    void SortBuildings(GlobalAttributes GA)
    {
        List<BaseBuilding> tempList = new List<BaseBuilding>();
        for(int i = 0; i < GA.Buildings.Count; ++i)
        {
            tempList.Add(GA.Buildings.Find(x => x.name == ((Buildings)i).ToString()));
        }
        GA.Buildings.Clear();
        GA.Buildings.AddRange(tempList);
    }

}
#endif