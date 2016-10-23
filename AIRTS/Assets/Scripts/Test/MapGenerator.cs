using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    public int NumTiles = 10000;
    public GameObject TerrainTile;
    GameObject[,] Map;

	// Use this for initialization
	void Start ()
    {
        int LateralTiles = (int)Mathf.Sqrt(NumTiles);
        Map = new GameObject[LateralTiles, LateralTiles];
        Debug.Log(LateralTiles);
	    for(int i = 0; i < LateralTiles; ++i)
        {
            for(int j = 0; j < LateralTiles; ++j)
            {
                GameObject nTile = (GameObject)Instantiate(TerrainTile, new Vector3((i - (LateralTiles / 2f)) * 2, 0, (j - (LateralTiles / 2f)) * 2), Quaternion.identity, transform);
                Map[i, j] = nTile;
                if(i < LateralTiles / 2 && j < LateralTiles / 2)
                {
                    nTile.GetComponent<Renderer>().material.color = Color.green;
                }
                else if(i > LateralTiles / 2 && j > LateralTiles / 2)
                {
                    nTile.GetComponent<Renderer>().material.color = Color.cyan;
                }
                else if (i < LateralTiles / 2 && j > LateralTiles / 2)
                {
                    nTile.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if(i > LateralTiles / 2 && j < LateralTiles / 2)
                {
                    nTile.GetComponent<Renderer>().material.color = Color.magenta;
                }
                else
                {
                    nTile.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

}
