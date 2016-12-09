// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TeamManager : MonoBehaviour
{
    #region Enums

    #region Public

    /// <summary>
    /// Only Base will work.
    /// </summary>
    public enum TeamTypes { Base, Tristan, Scott, Oliver }

    #endregion

    #endregion

    #region Variables

    #region Public

    public List<BaseAITeam> Teams = new List<BaseAITeam>(1);

    #endregion

    #endregion

    #region Classes

    #region Public  

    #endregion

    #endregion

    #region Functions

    #region Public

    public void PlaceStartingBuildings()
    {
        foreach(BaseAITeam team in Teams)
        {
            if(team.StartingLocation == Vector2.zero)
            {
                team.StartingLocation = GetRandomMapLocation();
            }

            bool isbuildingBuilt = false;
            while(isbuildingBuilt == false)
            {
                isbuildingBuilt = team.ConstructBuilding(Buildings.TownHall, team.StartingLocation);
                if(isbuildingBuilt == false)
                {
                    team.StartingLocation = GetRandomMapLocation();
                }
            }
        }
    }

    public List<BaseAITeam> GetTeams()
    {
        return Teams;
    }

    public BaseAITeam GetTeam(int teamID)
    {
        return Teams[teamID];
    }

    /// <summary>
    /// Not finished
    /// </summary>
    /// <param name="newTeam"></param>
    /// <param name="StartingPos"></param>
    /// <param name="TeamID"></param>
    public void AddTeam(TeamTypes newTeam, Vector2 StartingPos, int TeamID)
    {
        BaseAITeam nTeam = Instantiate(Resources.Load<BaseAITeam>("TeamPrefabs/BaseTeam"));
        nTeam.StartingLocation = StartingPos;
        nTeam.TeamID = TeamID;
        nTeam.transform.SetParent(transform);
        Teams.Add(nTeam);
    }

    public void ClearTeams()
    {
        foreach(BaseAITeam team in Teams)
        {
            DestroyImmediate(team.gameObject);
        }
        Teams.Clear();
    }

    public void RemoveTeam(BaseAITeam team)
    {
        DestroyImmediate(team.gameObject);
        TestTeams();
    }

    public void RemoveTeam(int teamID)
    {
        DestroyImmediate(Teams.Find(x => x.TeamID == teamID).gameObject);
        TestTeams();
    }

    public void TestTeams()
    {
        for(int i = 0; i < Teams.Count; ++i)
        {
            if(Teams[i] == null)
            {
                Teams.RemoveAt(i);
                --i;
            }
        }
    }

    public bool IsTeamIDInUse(int testID)
    {
        foreach(BaseAITeam team in Teams)
        {
            if(testID == team.TeamID)
            {
                return true;
            }
        }
        return false;
    }

    public int GetNewTeamID()
    {
        int testID = 0;

        List<int> CurrentIDs = new List<int>();
        foreach(BaseAITeam t in Teams)
        {
            CurrentIDs.Add(t.TeamID);
        }

        bool isIDUnique = false;
        while(isIDUnique == false)
        {
            if (CurrentIDs.Contains(testID))
            {
                ++testID;
            }
            else
            {
                isIDUnique = true;
            }
        }

        return testID;

    }

    #endregion

    #region Private

    private Vector2 GetRandomMapLocation()
    {
        return new Vector2(Random.Range(0, MapGenerator.Map.GetLength(0)), Random.Range(0, MapGenerator.Map.GetLength(1)));
    }

    #endregion

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(TeamManager))]
public class TeamManagerEditor : Editor
{
    private bool IsAddingTeam = false;
    private TeamManager myTeamManager;
    private bool isDisplayingTeams = true;

    private int posWidth = 103;
    private int rowLabelWidth = 27;
    private int rowValueWidth = 30;
    private int columnLabelWidth = 49;
    private int columnValueWidth = 30;

    private TeamManager.TeamTypes selectedType = TeamManager.TeamTypes.Base;
    private int newTeamID;
    private Vector2 newTeamStartPosition = Vector2.zero;

    void OnEnable()
    {
        myTeamManager = (TeamManager)target;
        newTeamID = myTeamManager.GetNewTeamID();
    }

    public override void OnInspectorGUI()
    {
        myTeamManager.TestTeams();
        
        EditorGUILayout.Space();

        NewTeamOptions();
        
        EditorGUILayout.Space();

        DisplayTeams();

    }

    void DisplayTeams()
    {
        isDisplayingTeams = EditorGUILayout.Foldout(isDisplayingTeams, "Team Information:");
        if (isDisplayingTeams)
        {
            List<BaseAITeam> teams = myTeamManager.GetTeams();
            int screenWidth = Screen.width;
            EditorGUILayout.LabelField("Size:", teams.Count.ToString());
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            for(int i = 0; i < teams.Count; ++i)
            {
                teams[i].name = EditorGUILayout.TextField("Team Name: ", teams[i].name);
                EditorGUILayout.LabelField("Team ID: " + teams[i].TeamID.ToString());

                EditorGUILayout.BeginHorizontal();
                int Row = (int)teams[i].StartingLocation.x;
                int Column = (int)teams[i].StartingLocation.y;

                EditorGUILayout.LabelField("Starting Location:", GUILayout.Width(posWidth), GUILayout.MinWidth(103));
                EditorGUILayout.LabelField("Row", GUILayout.Width(rowLabelWidth), GUILayout.MinWidth(27));
                Row = EditorGUILayout.IntField(Row, GUILayout.Width(rowValueWidth), GUILayout.MinWidth(20));
                EditorGUILayout.LabelField("Column", GUILayout.Width(columnLabelWidth), GUILayout.MinWidth(49));
                Column = EditorGUILayout.IntField(Column, GUILayout.Width(columnValueWidth), GUILayout.MinWidth(20));
                teams[i].StartingLocation = new Vector2(Row, Column);
                EditorGUILayout.EndHorizontal();

                if(GUILayout.Button("Remove Team"))
                {
                    myTeamManager.RemoveTeam(teams[i].TeamID);
                    newTeamID = myTeamManager.GetNewTeamID();
                    --i;
                }

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }

        }
    }

    void NewTeamOptions()
    {
        IsAddingTeam = EditorGUILayout.Foldout(IsAddingTeam, "Team Options:");

        if (IsAddingTeam)
        {
            bool isTeamTypeCorrect = true;
            bool isTeamIDCorrect = true;

            // Let the user choose the team that they want to add
            selectedType = (TeamManager.TeamTypes)EditorGUILayout.EnumPopup("Team Type: ", selectedType);

            // Warn the user if the team isn't implemented yet.
            if(selectedType != TeamManager.TeamTypes.Base)
            {
                EditorGUILayout.HelpBox("Team type not currently supported", MessageType.Warning);
                isTeamTypeCorrect = false;
            }
            
            newTeamID = EditorGUILayout.IntField("Team ID:", newTeamID);

            if (myTeamManager.IsTeamIDInUse(newTeamID))
            {
                EditorGUILayout.HelpBox("Team ID currently in use, please use another.", MessageType.Error);
                if(GUILayout.Button("Get New ID"))
                {
                    newTeamID = myTeamManager.GetNewTeamID();
                }
                isTeamIDCorrect = false;
            }

            EditorGUILayout.BeginHorizontal();
            int Row = (int)newTeamStartPosition.x;
            int Column = (int)newTeamStartPosition.y;

            EditorGUILayout.LabelField(new GUIContent("Starting Location:", "If this location is either:\n- at (0,0)\n- not on the map\n- cannot fit the building in the area\nthen it will spawn at a random location"), GUILayout.Width(posWidth), GUILayout.MinWidth(103));
            EditorGUILayout.LabelField("Row", GUILayout.Width(rowLabelWidth), GUILayout.MinWidth(27));
            Row = EditorGUILayout.IntField(Row, GUILayout.Width(rowValueWidth), GUILayout.MinWidth(20));
            EditorGUILayout.LabelField("Column", GUILayout.Width(columnLabelWidth), GUILayout.MinWidth(49));
            Column = EditorGUILayout.IntField(Column, GUILayout.Width(columnValueWidth), GUILayout.MinWidth(20));
            newTeamStartPosition = new Vector2(Row, Column);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button(new GUIContent("Add Team", "Adds a new team\nProvided there are no problems with the new team.")) && isTeamIDCorrect && isTeamTypeCorrect)
            {
                // Add the chosen team to the list of teams if it is implemented.
                myTeamManager.AddTeam(selectedType, newTeamStartPosition, newTeamID);
                newTeamID = myTeamManager.GetNewTeamID();
            }

            if (myTeamManager.GetTeams().Count > 0)
            {
                if (GUILayout.Button(new GUIContent("Clear Teams", "Clears all the teams in the game.")))
                {
                    myTeamManager.ClearTeams();
                    newTeamID = myTeamManager.GetNewTeamID();
                }
            }
        }
    }

}
#endif