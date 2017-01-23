// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseBuilding : GameEntity
{

    #region Variables

    #region Public 

    /// <summary>
    /// The tiles that make up the exclusion zone for this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> exclusionZone = new List<HexTile>();

    /// <summary>
    /// The tiles that act as the entrance to this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> EntanceTiles = new List<HexTile>();

    /// <summary>
    /// The tiles that make up the area of this building.
    /// </summary>
    [HideInInspector]
    public List<HexTile> BuildingArea = new List<HexTile>();

    /// <summary>
    /// The type of building that this is.
    /// </summary>
    [Tooltip("The type of building this is.")]
    public Buildings BuildingType;

    /// <summary>
    /// The tier of this building.
    /// </summary>
    [Tooltip("The tier of this building.")]
    [Range(0, 4)]
    public int Tier;

    /// <summary>
    /// The size of this building.
    /// </summary>
    [Tooltip("The Size of this building.")]
    [Range(1, 5)]
    public int Size;

    /// <summary>
    /// The time it takes to build this building.
    /// </summary>
    [Tooltip("The time it takes to build this building.\n T0 : 0s\n T1 : 5s\n T2 : 10s\n T3 : 20s\n T4 : 40s")]
    [Range(0, 40)]
    public float ConstructionTime;

    /// <summary>
    /// The mode of production.
    /// ie. Will a barracks make Soldiers(0) or Arhers(1).
    /// 0 by default.
    /// </summary>
    [Tooltip("The mode of production.\nie. Will a barracks make Soldiers(0) or Arhers(1).\n0 by default.")]
    [Range(0, 1)]
    public int ProductionMode = 0;
    
    /// <summary>
    /// The time left for the construction of the building.
    /// </summary>
    [HideInInspector]
    public float ConstructionTimer = 0;

    #endregion

    #region Protected

    /// <summary>
    /// The object that hold the 3D model data for construction. 
    /// </summary>
    protected GameObject ConstructionObject;

    /// <summary>
    /// The gameobject that has the 3D model under it.
    /// </summary>
    protected GameObject OperationalModelData;


    #endregion

    #endregion

    #region Classes

    #region Public    

    #endregion

    #endregion

    #region Functions

    #region Public

    public void ConfigureBuilding(int Q, int R, int teamID)
    {
        hexTransform = new HexTransform(Q, R);
        Health = MaxHealth;
        TeamID = teamID;
    }

    public void ConfigureBuilding(float Q, float R, int teamID)
    {
        ConfigureBuilding((int)Q, (int)R, teamID);
    }

    public void ConfigureBuilding(Vector2 Loc, int teamID)
    {
        ConfigureBuilding((int)Loc.x, (int)Loc.y, teamID);
    }

    public virtual void BuildingUpdate() { }

    public virtual List<KalamataTicket> GetTicketForProducts(ref List<Products> products)
    {
        return new List<KalamataTicket>();
    }

    public virtual bool TestForProducts(params Products[] products)
    {
        return false;
    }

    #endregion

    #region Protected

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        OperationalModelData = transform.FindChild("ModelData").gameObject;
        SetUpStateMachine();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Start();
        BuildingStateMachine.SMUpdate();
    }

    protected virtual void ConstructionFinished()
    {
        //Destroy the Construction model
        Destroy(ConstructionObject);

        // Here change the update the model to the actual building model rather than the construction model.
        OperationalModelData.SetActive(true);
    }

    protected virtual void BeginOperational()
    {
        
    }   

    protected List<KalamataTicket> GetTicketsForProducts(List<StorageItem> storage, ref List<Products> products)
    {
        List<KalamataTicket> tickets = new List<KalamataTicket>();

        // only loop the usually larger list once.
        foreach (StorageItem item in storage)
        {
            // if the item isn't reserved then continue tests on it.
            if (!item.Reserved)
            {
                // this usuially being the smaller list gets looped more often.
                foreach (Products product in products)
                {
                    // if the product is the one we want, create a ticket for it and remove it from the critea.
                    if (product == item.Product)
                    {
                        KalamataTicket nTicket = new KalamataTicket(product, this, item.ReserveProduct());
                        tickets.Add(nTicket);
                        products.Remove(product);
                        break;
                    }
                }
            }
            if (products.Count == 0) break;
        }

        return tickets;
    }

    protected bool TestForProducts(List<StorageItem> storage, params Products[] products)
    {
        foreach (StorageItem item in storage)
        {
            if (!item.Reserved)
            {
                foreach (Products product in products)
                {
                    if (item.Product == product)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    #endregion

    #region Private

    private void BeginConstruction()
    {
        // Create appropriate 3D model for being under construction.
        ConstructionObject = (GameObject)Instantiate(Resources.Load("ConstructionBuildings/ConstructionBuilding0" + Size));
        ConstructionObject.transform.SetParent(transform, false);

        // Hide the operational model data.
        OperationalModelData.SetActive(false);

        ConstructionTimer = ConstructionTime;
    }

    private void ConstructionUpdate()
    {
        ConstructionTimer -= Time.deltaTime;
    }

    private bool HasConstructionFinished()
    {
        if(ConstructionTimer <= 0)
        {
            return true;
        }
        return false;
    }

    private void SetUpStateMachine()
    {
        // Configure Transistion requiremnts.
        Condition.BoolCondition ConstructionCondition = new Condition.BoolCondition();
        ConstructionCondition.Condition = HasConstructionFinished;

        // Create Transitions.
        SM.Transition ConstructionFinishedTrans = new SM.Transition("Construction Finished", ConstructionCondition, ConstructionFinished);

        // Create States.
        SM.State UnderConstruction = new SM.State("Under Construction", 
            new List<SM.Transition>() { ConstructionFinishedTrans }, 
            new List<SM.Action>() { BeginConstruction },
            new List<SM.Action>() { ConstructionUpdate },
            new List<SM.Action>() {  });

        SM.State Operational = new SM.State("Operational",
            null,
            new List<SM.Action>() { BeginOperational },
            new List<SM.Action>() { BuildingUpdate },
            null);

        // Add target state to transitions
        ConstructionFinishedTrans.SetTargetState(Operational);

        BuildingStateMachine = new SM.StateMachine(null, UnderConstruction, Operational);    

        // Initialise the machine.
        BuildingStateMachine.InitMachine();
    }

    #endregion

    #endregion

    #region StateMachines

    protected SM.StateMachine BuildingStateMachine;

    #endregion

}



#if UNITY_EDITOR
[CustomEditor(typeof(BaseBuilding))]
[CanEditMultipleObjects]
public class BaseBuildingEditor : GameEntityEditor
{
    protected List<ProductNumbers> productCounts = new List<ProductNumbers>();

    private BaseBuilding myBBTarget;

    protected override void OnEnable()
    {
        base.OnEnable();
        myBBTarget = (BaseBuilding)target;
    }

    protected class ProductNumbers
    {
        public int count;
        public Products type;

        public ProductNumbers(int Count, Products Type)
        {
            count = Count;
            type = Type;
        }

        public void AddtoCount(int amount)
        {
            count += amount;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (UseCustomInpector)
        {
            EditorGUILayout.LabelField("Building Type:", myBBTarget.BuildingType.ToString());

            EditorGUILayout.LabelField("Building Tier:", myBBTarget.Tier.ToString());

            EditorGUILayout.LabelField("Building Size:", myBBTarget.Size.ToString());

            EditorGUILayout.LabelField("Construction Time:", myBBTarget.ConstructionTimer.ToString("F2") + " / " + myBBTarget.ConstructionTime.ToString());
            
            EditorGUILayout.LabelField("Production Mode:", myBBTarget.ProductionMode.ToString());
        }
    }

    protected List<ProductNumbers> CalcProductCounts(List<StorageItem> list)
    {
        List<ProductNumbers> ReturnList = new List<ProductNumbers>();

        // for each storage item check if it is already in our return list
        // if it is then increase it corrosponding count, if it is not then add it to the list.
        foreach (StorageItem SI in list)
        {
            if (DoesProdCountsContain(SI.Product, ReturnList))
            {
                int index = ReturnList.FindIndex(x => x.type == SI.Product);
                ReturnList[index].AddtoCount(1);
            }
            else
            {
                ReturnList.Add(new ProductNumbers(1, SI.Product));
            }
        }
        return ReturnList;
    }

    protected List<ProductNumbers> CalcProductCounts(List<Products> list)
    {
        List<ProductNumbers> ReturnList = new List<ProductNumbers>();

        // for each product check if it is already in our return list
        // if it is then increase it corrosponding count, if it is not then add it to the list.
        foreach (Products product in list)
        {
            if (DoesProdCountsContain(product, ReturnList))
            {
                int index = ReturnList.FindIndex(x => x.type == product);
                ReturnList[index].AddtoCount(1);
            }
            else
            {
                ReturnList.Add(new ProductNumbers(1, product));
            }
        }
        return ReturnList;
    }

    protected bool DoesProdCountsContain(Products product, List<ProductNumbers> testList)
    {
        // test to see if the roduct is in the list passed in.
        foreach (ProductNumbers PN in testList)
        {
            if (PN.type == product)
            {
                return true;
            }
        }
        return false;
    }

    protected void StorageInspector(string name, ref bool ShowStorage, List<List<int>> ItemList, List<UnitProduction.InputRequirements> ExpectedList)
    {
        ShowStorage = EditorGUILayout.Foldout(ShowStorage, name);

        if (ShowStorage)
        {
            for(int i = 0; i < ExpectedList.Count; ++i)
            {
                for(int j = 0; j < ExpectedList[i].RequiredProducts.Count; ++j)
                {
                    EditorGUILayout.LabelField(ItemList[i][j].ToString() + 'x', ExpectedList[i].RequiredProducts[j].ToString());
                }
            }
        }
    }

    protected void StorageInspector(string name, ref bool ShowStorage, List<Products> ItemList, ref int previousCount, ref bool previousShowStorage, ref List<ProductNumbers> list)
    {
        ShowStorage = EditorGUILayout.Foldout(ShowStorage, name + ItemList.Count.ToString());

        if (ItemList.Count > 0 && ShowStorage)
        {
            // if either the number of objects has changed or if we have begun to show it then
            // update the list of products.
            if (previousCount != ItemList.Count || previousShowStorage != ShowStorage)
            {
                list.Clear();
                list = CalcProductCounts(ItemList);
            }

            // for each product print it and the amount that there are for it.
            foreach (ProductNumbers PN in list)
            {
                EditorGUILayout.LabelField(PN.count.ToString() + "x", PN.type.ToString());
            }

        }

        previousCount = ItemList.Count;
        previousShowStorage = ShowStorage;
    }

    protected void StorageInspector(string name, ref bool ShowStorage, List<int> ItemList, List<Products> ExpectedList)
    {
        ShowStorage = EditorGUILayout.Foldout(ShowStorage, name);

        if(ItemList.Count > 0 && ShowStorage)
        {
            for(int  i =0; i < ExpectedList.Count; ++i)
            {
                EditorGUILayout.LabelField(ItemList[i].ToString() + 'x', ExpectedList[i].ToString());
            }
        }
    }

    protected void StorageInspector(string name, ref bool ShowStorage, List<StorageItem> ItemList, ref int previousCount, ref bool previousShowStorage)
    {
        ShowStorage = EditorGUILayout.Foldout(ShowStorage, name + ItemList.Count.ToString());

        if (ItemList.Count > 0 && ShowStorage)
        {
            // if either the number of objects has changed or if we have begun to show it then
            // update the list of products.
            if (previousCount != ItemList.Count || previousShowStorage != ShowStorage)
            {
                productCounts.Clear();
                productCounts = CalcProductCounts(ItemList);
            }

            // for each product print it and the amount that there are for it.
            foreach (ProductNumbers PN in productCounts)
            {
                EditorGUILayout.LabelField(PN.count.ToString() + "x", PN.type.ToString());
            }

        }

        previousCount = ItemList.Count;
        previousShowStorage = ShowStorage;
    }

}
#endif