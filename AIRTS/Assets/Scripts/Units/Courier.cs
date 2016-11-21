using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Courier : BaseUnit
{
    public BuildingBase homeBuilding;

    int inventorySpace = 10;
    List<Products> inventory = new List<Products>();
    List<Products> shoppingList = new List<Products>();

    #region States
    State pickUp = new State();
    State returnProduct = new State();
    State atHome = new State();
    #endregion

    #region Transitions
    Transition allProductsFound = new Transition();
    Transition findNextProduct = new Transition();
    Transition getHome = new Transition();
    #endregion

    #region Conditions
    ListHasDataCond<Products> shoppingListEmpty = new ListHasDataCond<Products>();
    AEqualsB inventoryFull = new AEqualsB();
    NotCondition shoppingNotEmpty = new NotCondition();
    NotCondition inventoryNotFull = new NotCondition();
    AndCondition canFindMore = new AndCondition();

    AndCondition positionCorrect = new AndCondition();
    AEqualsB columnCorrect = new AEqualsB();
    AEqualsB rowCorrect = new AEqualsB();
    #endregion

    void Start()
    {
        shoppingNotEmpty.condition = shoppingListEmpty;
        allProductsFound.condition = shoppingListEmpty;
        inventoryNotFull.condition = inventoryFull;
        canFindMore.A = shoppingNotEmpty;
        canFindMore.B = inventoryNotFull;

        positionCorrect.A = columnCorrect;
        positionCorrect.B = rowCorrect;

        allProductsFound.TargetState = returnProduct;
        allProductsFound.Action = new List<Action> { GetPathHome };

        findNextProduct.condition = canFindMore;
        findNextProduct.TargetState = pickUp;
        findNextProduct.Action = new List<Action> { NullAction };

        getHome.Action = new List<Action> { ReturnHome };
        getHome.condition = positionCorrect;
        getHome.TargetState = atHome;
        
        pickUp.transitions = new List<Transition> { allProductsFound, findNextProduct };
        pickUp.action = new List<Action> { Move };
        pickUp.entryAction = NullAction;
        pickUp.exitAction = NullAction;

        returnProduct.action = new List<Action> { Move };
        returnProduct.entryAction = NullAction;
        returnProduct.exitAction = NullAction;
        returnProduct.transitions = new List<Transition> { getHome };

        atHome.action = new List<Action> { NullAction };
        atHome.entryAction = NullAction;
        atHome.exitAction = NullAction;
        atHome.transitions = new List<Transition> { };

        unitStateMachine.states = new List<State> { pickUp, returnProduct, atHome };
        unitStateMachine.initialState = pickUp;
    }

    void GetPathHome()
    {
        path = ASImplementation.ASI.AStar(MapGenerator.Map[hexTransform.Q, hexTransform.R], MapGenerator.Map[homeBuilding.hexTransform.Q, homeBuilding.hexTransform.R]);
    }

    void ReturnHome()
    {
        // Add this unit to the building
        // Remove it from the map's list
        Destroy(gameObject);
    }

    /*void FindProduct()
    {
        List<StorageBuilding> allStorageBuildings = new List<StorageBuilding>();
        List<StorageBuilding> buildingsWithProduct = new List<StorageBuilding>();

        // Find all warehouses and town halls on your team
        for(int i = 0; i < GlobalAttributes.Global.Buildings.Count; ++i)
        {
            if((GlobalAttributes.Global.Buildings[i].BuildingType == Buildings.Warehouse || GlobalAttributes.Global.Buildings[i].BuildingType == Buildings.TownHall) && GlobalAttributes.Global.Buildings[i].TeamID == TeamID)
            {
                allStorageBuildings.Add((StorageBuilding)GlobalAttributes.Global.Buildings[i]);
            }
        }

        // Find all of those buildings that contain the products you need
        for(int i = 0; i < allStorageBuildings.Count; ++i)
        {
            for (int j = 0; j < shoppingList.Count; ++j)
            {
                if (allStorageBuildings[i].ItemsStored.Contains(shoppingList[j]))
                {
                    buildingsWithProduct.Add(allStorageBuildings[i]);
                }
            }
        }

        // Remove the duplicate buildings
        for(int i = 0; i < buildingsWithProduct.Count; ++i)
        {
            if(buildingsWithProduct[i] == buildingsWithProduct[i + 1])
            {
                buildingsWithProduct.RemoveAt(i + 1); 
                i--;
            }
        }

        // If there is a building with the product, find the closest building
        if (buildingsWithProduct.Count > 0)
        {
            path = DijkstraImplementation.DJI.DijkstraToBuilding(MapGenerator.Map[hexTransform.Q, hexTransform.R], new List<Buildings> { Buildings.TownHall, Buildings.Warehouse }, TeamID);
        }
     }*/
}
