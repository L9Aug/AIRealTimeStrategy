// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public List<Products> ProductionStorage = new List<Products>();

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

            StorageInspector("Input Storage: ", ref showInputStorage, myGPTarget.ProductionStorage, ref previousInputStorageCount, ref previousShowInputStorage, ref InputStorageCounts);

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