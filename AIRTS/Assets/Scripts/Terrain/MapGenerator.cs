﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public int x;
    public int y;
    public HexTile TerrainTile;
    public static HexTile[,] Map;

    public float TargetFrameRate;

    float FrameStartTime = 0;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GenerateMap());
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
    }

}