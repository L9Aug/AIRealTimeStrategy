using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AITeamBase : MonoBehaviour {

    #region Public Variables

    /// <summary>
    /// The ID for this team.
    /// </summary>
    [Tooltip("The ID for this team")]
    public int TeamID = 0;

    /// <summary>
    /// The amount of gold this team has.
    /// </summary>
    [Tooltip("The amout of gold this team has.")]
    public int Gold = 0;

    /// <summary>
    /// The population available to this team. (Not population already in buildings)
    /// </summary>
    [Tooltip("The population available to this team.\n(Not population already in buildings.")]
    public PopulationClass Population = new PopulationClass();

    /// <summary>
    /// A list of the buildings that this team has.
    /// </summary>
    [Tooltip("A list of buildings that this team has.")]
    public List<BuildingBase> BuildingsList = new List<BuildingBase>();

    //Temp
    public int x;
    public int y;
    public Buildings TestSpawn;

    #endregion

    #region Classes
    [System.Serializable]
    public class PopulationClass
    {
        public List<BaseUnit> Citizens = new List<BaseUnit>();
        public List<BaseUnit> Merchants = new List<BaseUnit>();
        public List<BaseUnit> Military = new List<BaseUnit>();

        public int CitizenCount = 0;
        public int MerchantCount = 0;
        public MilitaryCountClass MilitaryCount = new MilitaryCountClass();

        [System.Serializable]
        public class MilitaryCountClass
        {
            int Soldiers = 0;
            int Archers = 0;
            int Catapults = 0;

            public int totalMilitaryUnits()
            {
                return Soldiers + Archers + Catapults;
            }
        }

    }

    #endregion

    #region Protected Functions

    /// <summary>
    /// Attempts to construct the chosen building.
    /// </summary>
    /// <param name="building">The building to build.</param>
    /// <param name="Location">The location to build it.</param>
    /// <returns>Returns true if the building can and is being built.</returns>
    public bool ConstructBuilding(Buildings building, Vector2 Location)
    {
        if (ValidateLocation(Location))
        {
            print("Valid Location");
            int BuildingSize = GlobalAttributes.Global.Buildings[(int)building].Size;
            if (ValidateArea(Location, BuildingSize))
            {
                print("Valid Area");
                int buildingTier = GlobalAttributes.Global.Buildings[(int)building].Tier;
                if (CheckResources(buildingTier))
                {
                    print("Valid Resources");
                    Vector3 BuildingPos = MapGenerator.Map[(int)Location.x, (int)Location.y].transform.position;
                    BuildingsList.Add((BuildingBase)Instantiate(GlobalAttributes.Global.Buildings[(int)building], BuildingPos, Quaternion.identity, transform));

                    //deduct resources.
                    DeductResources(buildingTier);

                    //set exlusion zone
                    SetExlusionZone(Location, BuildingSize);

                    //clear Connections.
                    ClearArea(Location, BuildingSize);

                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region Private Functions

    void SetExlusionZone(Vector2 loc, int buildingSize)
    {
        List<HexTile> BuildingRing = MapGenerator.Map[(int)loc.x, (int)loc.y].GetHexRing(buildingSize + 1);
        foreach(HexTile h in BuildingRing)
        {
            h.IsExlusionZone = true;
        }
    }

    void ClearArea(Vector2 loc, int buildingSize)
    {
        List<HexTile> BuildingArea = MapGenerator.Map[(int)loc.x, (int)loc.y].GetHexArea(buildingSize);
        foreach(HexTile h in BuildingArea)
        {
            h.ClearConnections();
        }
    }

    void DeductResources(int tier)
    {
        Gold -= tier;
        Population.CitizenCount -= tier;
        if(tier == 4)
        {
            Population.MerchantCount -= 1;
        }
    }

    /// <summary>
    /// Tests to see if there is enough Space for the building.
    /// </summary>
    /// <param name="loc">The location of the building.</param>
    /// <param name="buildingSize">The size of the building.</param>
    /// <returns>Returns true if there is enough space.</returns>
    private bool ValidateArea(Vector2 loc, int buildingSize)
    {
        int AreaCount = MapGenerator.Map[(int)loc.x, (int)loc.y].GetHexArea(buildingSize).Count;
        int RequiredArea = (int)((3 * Mathf.Pow(buildingSize - 1, 2)) + (3 * (buildingSize - 1)) + 1); //Hex Number equation 3n^2 + 3n + 1 from Wolfram Alpha : http://mathworld.wolfram.com/HexNumber.html
        if (AreaCount >= RequiredArea)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Test to see if the team has the required resources for the building tier.
    /// </summary>
    /// <param name="BuildingTier">The tier of the building.</param>
    /// <returns>Returns true if the team has the required resources.</returns>
    private bool CheckResources(int BuildingTier)
    {
        if(Gold >= BuildingTier && Population.CitizenCount >= BuildingTier)
        {
            if(BuildingTier == 4)
            {
                if(Population.MerchantCount >= 1)
                {

                    //dispatch merchant and pop to building site.
                    return true;
                }
            }

            // dispatch pop to building site.
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tests to see if the location exists.
    /// </summary>
    /// <param name="loc">The location to test.</param>
    /// <returns>Returns true if it exists.</returns>
    private bool ValidateLocation(Vector2 loc)
    {
        if(loc.x < MapGenerator.Map.GetLength(0) && loc.y < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

    #endregion

}


#if UNITY_EDITOR
[CustomEditor(typeof(AITeamBase))]
public class AITeamBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AITeamBase myTarget = (AITeamBase)target;

        if(GUILayout.Button("Create Building"))
        {
            Debug.Log(myTarget.ConstructBuilding(myTarget.TestSpawn, new Vector2(myTarget.x, myTarget.y)));
        }
    }

}
#endif