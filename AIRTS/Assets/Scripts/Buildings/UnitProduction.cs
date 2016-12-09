// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public List<Products> ProductionStorage = new List<Products>();

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
    }

    protected override void Update()
    {
        base.Update();
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

            StorageInspector("Input Storage: ", ref showInputStorage, myUPTarget.ProductionStorage, ref previousInputStorageCount, ref previousShowInputStorage, ref productCounts);
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