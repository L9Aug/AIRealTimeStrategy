// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DT;
using Decisions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnitProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The Products required for this building to function.")]
    public List<InputRequirements> ProductionRequirements = new List<InputRequirements>();

    /// <summary>
    /// Storage for products that are using in production
    /// </summary>
    [HideInInspector]
    public List<List<int>> ProductionStorage = new List<List<int>>();

    /// <summary>
    /// The unit/s that this building creates.
    /// </summary>
    [Tooltip("The unit/s that this building creates.")]
    public List<UnitOutputClass> UnitOutput = new List<UnitOutputClass>();

    #endregion

    #endregion

    #region Classes

    #region Public

    [System.Serializable]
    public class UnitOutputClass
    {
        public List<Units> Units = new List<Units>();
    }

    [System.Serializable]
    public class InputRequirements
    {
        public List<Products> RequiredProducts = new List<Products>();
    }

    #endregion

    #endregion

    #region Functions

    #region Protected

    protected override void Start()
    {
        base.Start();

        for(int i = 0; i < ProductionRequirements.Count; ++i)
        {
            ProductionStorage.Add(new List<int>());
            for(int j = 0; j < ProductionRequirements[i].RequiredProducts.Count; ++j)
            {
                ProductionStorage[i].Add(1);
            }
        }

        SetupDecisionTree();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator ProductionCycle()
    {
        while (inProduction)
        {
            yield return null;
            ProductionTimer += Time.deltaTime;

            if (ProductionTimer >= ProductionTime)
            {
                for (int i = 0; i < UnitOutput[ProductionMode].Units.Count; ++i)
                {
                    print("New " + UnitOutput[ProductionMode].Units[i].ToString() + " for Team " + TeamID);
                }
                ProductionTimer = 0;
                inProduction = false;
            }
        }
    }

    protected override IEnumerator DecisionTreeRunIntervals()
    {
        while (ProductionTree != null)
        {
            TreeTick();
            yield return null;
        }
    }

    protected override void BeginProduction()
    {
        base.BeginProduction();
        for(int i = 0; i < ProductionRequirements[ProductionMode].RequiredProducts.Count; ++i)
        {
            --ProductionStorage[ProductionMode][i];
        }
    }

    #endregion

    #region Private

    private object ProductCheck()
    {
        bool haveProducts = true;

        for(int i = 0; i < ProductionRequirements[ProductionMode].RequiredProducts.Count; ++i)
        {
            if(ProductionStorage[ProductionMode][i] == 0)
            {
                haveProducts = false;
                break;
            }
        }

        return haveProducts;
    }

    private object FutureProductCheck()
    {
        bool haveProducts = true;

        for (int i = 0; i < ProductionRequirements[ProductionMode].RequiredProducts.Count; ++i)
        {
            if(ProductionStorage[ProductionMode][i] < 5)
            {
                haveProducts = false;
                break;
            }
        }

        return haveProducts;
    }

    private object AreProductsOnMap()
    {
        return TeamManager.TM.Teams[TeamID].FindProducts(ProductionRequirements[ProductionMode].RequiredProducts.ToArray());
    }

    private object HaveISentACourier()
    {
        return false;
    }

    private void SetupDecisionTree()
    {
        // create decisions
        ObjectDecision isInProduction = new ObjectDecision(InProduction);

        ObjectDecision doIHaveProductsForProduction = new ObjectDecision(ProductCheck);

        ObjectDecision doIHaveProductsForFuture = new ObjectDecision(FutureProductCheck);

        ObjectDecision canIGetProductsForProduction = new ObjectDecision(AreProductsOnMap);

        ObjectDecision haveISentACouierForProducts = new ObjectDecision(HaveISentACourier);

        // create leaves
        Leaf WaitForNextCycle = new Leaf();

        Leaf SendCourierForProducts = new Leaf();

        Leaf BeginProductionLeaf = new Leaf(BeginProduction);

        // create verticies
        Vertex HaveISentCourier = new Vertex(haveISentACouierForProducts, WaitForNextCycle, SendCourierForProducts);

        Vertex CanIGetProductsForProduction = new Vertex(canIGetProductsForProduction, HaveISentCourier, WaitForNextCycle);

        Vertex DoIHaveProductsForThisProduction = new Vertex(doIHaveProductsForProduction, BeginProductionLeaf, CanIGetProductsForProduction);

        Vertex DoIHaveProductsForFutureProductions = new Vertex(doIHaveProductsForFuture, WaitForNextCycle, CanIGetProductsForProduction);

        Vertex IsInProduction = new Vertex(isInProduction, DoIHaveProductsForFutureProductions, DoIHaveProductsForThisProduction);

        // create decition tree
        ProductionTree = new DecisionTree(IsInProduction);
    }

    #endregion

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(UnitProduction))]
[CanEditMultipleObjects]
public class UnitProductionEditor : BaseProductionEditor
{
    private UnitProduction myUPTarget;

    private static bool showUnitReqs;

    private static bool showInputStorage;
    private bool previousShowInputStorage;
    private int previousInputStorageCount;

    protected override void OnEnable()
    {
        base.OnEnable();
        myUPTarget = (UnitProduction)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            UnitInspector();

            StorageInspector("Input Storage: ", ref showInputStorage, myUPTarget.ProductionStorage, myUPTarget.ProductionRequirements);
        }
    }

    void UnitInspector()
    {
        showUnitReqs = EditorGUILayout.Foldout(showUnitReqs, "Unit Requirements:");

        if (showUnitReqs)
        {
            for (int i = 0; i < myUPTarget.ProductionRequirements.Count; ++i)
            {
                List<ProductNumbers> inputs = new List<ProductNumbers>();
                //int[] counts = new int[myUPTarget.ProductionRequirements[i].RequiredProducts.Count];

                // for each required product see if we have already added it to our inputs list,
                // if we have then increase the corrosponding count for that product, if not then add it to our inputs list.
                for (int j = 0; j < myUPTarget.ProductionRequirements[i].RequiredProducts.Count; ++j)
                {
                    if (inputs.Find(x => x.type == myUPTarget.ProductionRequirements[i].RequiredProducts[j]) == null)
                    {
                        inputs.Add(new ProductNumbers(1, myUPTarget.ProductionRequirements[i].RequiredProducts[j]));
                    }
                    else
                    {
                        ++inputs.Find(x => x.type == myUPTarget.ProductionRequirements[i].RequiredProducts[j]).count;
                    }
                }

                // if we have multiple output units then make it more readable that we do.
                string outputs = myUPTarget.UnitOutput[i].Units[0].ToString();
                if (myUPTarget.UnitOutput[i].Units.Count > 1)
                {
                    outputs += " x" + myUPTarget.UnitOutput[i].Units.Count;
                }

                // print the first line of requirements showing the first input and the output of all following inputs for this output.
                EditorGUILayout.LabelField(inputs[0].type + ((inputs[0].count > 1) ? " x" + inputs[0].count.ToString() : ""), "->\t" + outputs);

                // if there is more than one input print the rest of the inputs.
                if (inputs.Count > 1)
                {
                    for (int i2 = 1; i2 < inputs.Count; ++i2)
                    {
                        EditorGUILayout.LabelField(inputs[i2].type + ((inputs[i2].count > 1) ? " x" + inputs[i2].count.ToString() : ""));
                    }
                }

                // if this building has multiple modes then seperate them with a line.
                if(i != myUPTarget.ProductionRequirements.Count - 1)
                {
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                }

            }
        }

    }
}
#endif