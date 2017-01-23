// Script by: Tristan Bampton UP690813

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    [Tooltip("The number of tiles you want horizontally.")]
    public int x;

    [Tooltip("The number of tiles you want vertically.")]
    public int y;

    [Tooltip("The tile prefab that you want to use for the map.")]
    public HexTile TerrainTile;
    public static HexTile[,] Map;

    [Tooltip("The framerate that you want the generation to run at.\nAt 0 it will generate it in one frame.")]
    public float TargetFrameRate;

    float FrameStartTime = 0;

    TeamManager teamManager;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GenerateMap());
        teamManager = FindObjectOfType<TeamManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        FrameStartTime = Time.realtimeSinceStartup;
	}

    IEnumerator GenerateMap()
    {
        Map = new HexTile[x, y];

        float TileHieght = Mathf.Sqrt(3);

        float TargetFrameTime = 1f / TargetFrameRate;

        float GenStartTime = Time.realtimeSinceStartup;
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < y; ++j)
            {
                //print(i);
                Vector3 Location = new Vector3(i * 1.5f, 0, j * TileHieght * -1);
                if ((i & 1) == 1)
                {
                    Location += new Vector3(0, 0, -(TileHieght / 2f));
                }

                Map[i, j] = (HexTile)(Instantiate(TerrainTile, Location, Quaternion.identity, transform));
                Map[i, j].ConfigureTile(i, j);

                if ((Time.realtimeSinceStartup - FrameStartTime) >= TargetFrameTime)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        float genTime = Time.realtimeSinceStartup - GenStartTime;

        print("Map Generated, " + (x * y) + " tiles in " + genTime + " seconds.");

        float connectionsStartTime = Time.realtimeSinceStartup;

        int numCons = 0;

        for (int i = 0; i < x; ++i)
        {
            for(int j = 0; j < y; ++j)
            {
                numCons += Map[i, j].GetConnections();

                if ((Time.realtimeSinceStartup - FrameStartTime) >= TargetFrameTime)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        print("Made " + numCons + " connections in " + (Time.realtimeSinceStartup - connectionsStartTime) + " seconds. Total Generation Time " + (Time.realtimeSinceStartup - GenStartTime) + " seconds.");

        GetComponent<TerrainGenerator>().StartGen();

        teamManager.PlaceStartingBuildings();
    }

}