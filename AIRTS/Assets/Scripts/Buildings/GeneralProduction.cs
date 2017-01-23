// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Decisions;
using DT;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneralProduction : BaseProduction
{

    #region Variables

    #region Public

    /// <summary>
    /// The products required for this building to function.
    /// </summary>
    [Tooltip("The products required for this building to function.")]
    public List<Products> ProductionRequirements = new List<Products>();

    /// <summary>
    /// Storage for production resources.
    /// </summary>
    [HideInInspector]
    public List<int> ProductionStorage = new List<int>();

    /// <summary>
    /// The product this building creates.
    /// </summary>
    [Tooltip("The product this building creates.")]
    public Products OutputProduct;

    /// <summary>
    /// Storage for completed products.
    /// </summary>
    [HideInInspector]
    public List<StorageItem> OutputStorage = new List<StorageItem>();

    #endregion

    #endregion

    #region Functions

    #region Public

    public override List<KalamataTicket> GetTicketForProducts(ref List<Products> products)
    {
        return GetTicketsForProducts(OutputStorage, ref products);
    }

    public override bool TestForProducts(params Products[] products)
    {
        return TestForProducts(OutputStorage, products);
    }

    #endregion

    #region Protected

    protected override void Start()
    {
        base.Start();

        for(int i = 0; i < ProductionRequirements.Count; ++i)
        {
            ProductionStorage.Add(1);
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
                OutputStorage.Add(new StorageItem(OutputProduct));
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
        for(int i = 0; i < ProductionStorage.Count; ++i)
        {
            --ProductionStorage[i];
        }
    }

    #endregion

    #region Private

    private object TestOutputStorage()
    {
        return (OutputStorage.Count >= 5) ? true : false;
    }

    private object FutureProductsTest()
    {
        bool isFull = true;

        foreach(int Ps in ProductionStorage)
        {
            if(Ps < 5)
            {
                isFull = false;
                break;
            }
        }

        return isFull;
    }

    private object ProductCheck()
    {
        bool HaveProducts = true;

        foreach(int Ps in ProductionStorage)
        {
            if(Ps == 0)
            {
                HaveProducts = false;
                break;
            }
        }

        return HaveProducts;
    }

    private object AreProductsOnMap()
    {
        return TeamManager.TM.Teams[TeamID].FindProducts(ProductionRequirements.ToArray());
    }

    private void SetupDecisionTree()
    {
        ObjectDecision isInProduction = new ObjectDecision(InProduction);

        ObjectDecision doIHaveProductsForFuture = new ObjectDecision(FutureProductsTest);

        ObjectDecision doIHaveProductsForProduction = new ObjectDecision(ProductCheck);

        ObjectDecision isStorageOutputFull = new ObjectDecision(TestOutputStorage);

        ObjectDecision canIGetProductsForProduction = new ObjectDecision(AreProductsOnMap);

        ObjectDecision isThereAnAvailableStorageFacility = new ObjectDecision(IsThereAStorageBuilding);

        ObjectDecision DoIHaveACourier = new ObjectDecision(IsThereAnAvailableCourier);

        Leaf WaitForNextCycle = new Leaf();

        Leaf beginProduction = new Leaf(BeginProduction);

        Leaf SendCourierWithProducts = new Leaf();

        Leaf SendCourierForProducts = new Leaf();

        Vertex HasCourierBeenSentForProducts = new Vertex(DoIHaveACourier, WaitForNextCycle, SendCourierForProducts);

        Vertex HasCourierBeenSentWithProducts = new Vertex(DoIHaveACourier, WaitForNextCycle, SendCourierWithProducts);

        Vertex IsThereStorageFacility = new Vertex(isThereAnAvailableStorageFacility, HasCourierBeenSentWithProducts, WaitForNextCycle);

        Vertex IsOutputStorageFull = new Vertex(isStorageOutputFull, IsThereStorageFacility, beginProduction);

        Vertex CanIGetProductsForProduction = new Vertex(canIGetProductsForProduction, HasCourierBeenSentForProducts, WaitForNextCycle);

        Vertex DoIHaveProductsForProduction = new Vertex(doIHaveProductsForProduction, IsOutputStorageFull, CanIGetProductsForProduction);

        Vertex DoIHaveProductsForFutureProductions = new Vertex(doIHaveProductsForFuture, WaitForNextCycle, CanIGetProductsForProduction);

        Vertex IsInProduction = new Vertex(isInProduction, DoIHaveProductsForFutureProductions, DoIHaveProductsForProduction);

        ProductionTree = new DecisionTree(IsInProduction);
    }

    #endregion

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(GeneralProduction))]
[CanEditMultipleObjects]
public class GeneralProductionEditor : BaseProductionEditor
{
    private GeneralProduction myGPTarget;

    private static bool showGPOutputStorage;
    private int previousOutputStorageCount;
    private bool previousShowGPOutputStorage;

    private List<ProductNumbers> ProductionReqCounts = new List<ProductNumbers>();
    private static bool showProductionReqs;
    private int previousPRcount;
    private bool previousShowPR;

    private List<ProductNumbers> InputStorageCounts = new List<ProductNumbers>();
    private static bool showInputStorage;
    private int previousInputStorageCount;
    private bool previousShowInputStorage;

    protected override void OnEnable()
    {
        base.OnEnable();
        myGPTarget = (GeneralProduction)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            ProductionReqInspector();

            StorageInspector("Input Storage: ", ref showInputStorage, myGPTarget.ProductionStorage, myGPTarget.ProductionRequirements);

            StorageInspector("Output Storage: ", ref showGPOutputStorage, myGPTarget.OutputStorage, ref previousOutputStorageCount, ref previousShowGPOutputStorage);
        }
    }

    void ProductionReqInspector()
    {
        showProductionReqs = EditorGUILayout.Foldout(showProductionReqs, "Production Requirements: ");

        if (showProductionReqs)
        {
            List<ProductNumbers> inputs = new List<ProductNumbers>();

            foreach(Products prod in myGPTarget.ProductionRequirements)
            {
                if(inputs.Find(x => x.type == prod) == null)
                {
                    inputs.Add(new ProductNumbers(1, prod));
                }
                else
                {
                    ++inputs.Find(x => x.type == prod).count;
                }
            }

            string output = myGPTarget.OutputProduct.ToString();

            EditorGUILayout.LabelField(inputs[0].type.ToString() + ((inputs[0].count > 1) ? (" x" + inputs[0].count.ToString()) : ""), "->\t" + output);

            if(inputs.Count > 1)
            {
                for(int i = 1; i < inputs.Count; ++i)
                {
                    EditorGUILayout.LabelField(inputs[i].type.ToString() + ((inputs[i].count > 1) ? (" x" + inputs[i].count.ToString()) : ""));
                }
            }

        }
    }
        
}
#endif