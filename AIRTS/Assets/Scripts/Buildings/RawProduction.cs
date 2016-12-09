// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Decisions;
using DT;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    #region Functions

    #region Public

    public override void ProductionCycle()
    {
        base.ProductionCycle();
    }

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();
        SetupDecisionTree();
    }

    protected override void Update()
    {
        base.Update();
        //here for testing purposes.
    }

    #endregion

    #region Private

    /// <summary>
    /// returns true if output storage is full
    /// </summary>
    /// <returns></returns>
    object TestOutputStorage()
    {
        return (OutputStorage.Count >= 5) ? true : false;
    }

    void BeginProduction()
    {
        inProduction = true;
    }

    /// <summary>
    /// Advances the current production.
    /// </summary>
    void DoProduction()
    {
        ProductionTimer += cumulativeDeltaTime;
        cumulativeDeltaTime = 0;

        if(ProductionTimer >= ProductionTime)
        {
            OutputStorage.Add(new StorageItem(OutputProduct[ProductionMode]));
            ProductionTimer = 0;
            inProduction = false;
        }

    }

    void SetupDecisionTree()
    {
        // Create Conditions
        ObjectDecision InProductionCond = new ObjectDecision(InProduction);

        ObjectDecision IsOutputStorageFullCond = new ObjectDecision(TestOutputStorage);

        ObjectDecision IsThereStorageBuildingCond = new ObjectDecision(IsThereAStorageBuilding);

        ObjectDecision DoIHaveCourierCond = new ObjectDecision(IsThereAnAvailableCourier);

        // Create leaves
        Leaf WaitLeaf = new Leaf();

        Leaf BeginProductionLeaf = new Leaf(BeginProduction);

        Leaf SendCourierLeaf = new Leaf();

        Leaf DoProductionLeaf = new Leaf(DoProduction);

        // Create Verticies
        Vertex HaveCourier = new Vertex(DoIHaveCourierCond, SendCourierLeaf, WaitLeaf);

        Vertex IsThereStorage = new Vertex(IsThereStorageBuildingCond, HaveCourier, WaitLeaf);

        Vertex IsOutputStorageFull = new Vertex(IsOutputStorageFullCond, IsThereStorage, BeginProductionLeaf);

        Vertex InProductionVert = new Vertex(InProductionCond, DoProductionLeaf, IsOutputStorageFull);

        // Create Tree
        ProductionTree = new DecisionTree(InProductionVert);
    }

    #endregion

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(RawProduction))]
[CanEditMultipleObjects]
public class RawProductionEditor : BaseProductionEditor
{
    private RawProduction myRPTarget;
    private static bool showRequirements;

    private static bool showOutputStorage;
    private int previousOutputStorageCount;
    private bool previousShowOutputStorage;

    protected override void OnEnable()
    {
        base.OnEnable();
        myRPTarget = (RawProduction)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(UseCustomInpector)
        {
            TerrainInspector();

            StorageInspector("Output Storage: ", ref showOutputStorage, myRPTarget.OutputStorage, ref previousOutputStorageCount, ref previousShowOutputStorage);
        }
    }

    void TerrainInspector()
    {
        showRequirements = EditorGUILayout.Foldout(showRequirements, "Production Requirements:");

        if (showRequirements)
        {
            
            for (int i = 0; i < myRPTarget.TerrainRequirement.Count; ++i)
            {
                List<string> terrains = new List<string>();
                // for each terrain requirement add it to a string to keep them together.
                for(int j = 0; j < myRPTarget.TerrainRequirement[i].TerrainRequirment.Count; ++j)
                {
                    terrains.Add(myRPTarget.TerrainRequirement[i].TerrainRequirment[j].ToString());
                }

                // print the terrains and the output based on that terrain.
                EditorGUILayout.LabelField(terrains[0], "->\t" + myRPTarget.OutputProduct[i].ToString());
                if(terrains.Count > 1)
                {
                    for(int i2 = 1; i2 < terrains.Count; ++i2)
                    {
                        EditorGUILayout.LabelField(terrains[i2]);
                    }
                }

                // if this building has multiple modes then seperate them with a line.
                if (i != myRPTarget.TerrainRequirement.Count - 1)
                {
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                }
            }
        }
    }
}
#endif