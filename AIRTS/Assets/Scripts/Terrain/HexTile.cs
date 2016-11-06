using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class HexTile : MonoBehaviour {

    /// <summary>
    /// The Column of that this tile is in.
    /// </summary>
    public int Q;
    /// <summary>
    /// the Row that this tile is in.
    /// </summary>
    public int R;
    public int x;
    public int y;
    public int z;

    public List<HexTile> Connections = new List<HexTile>();

    struct HextileTest
    {
        public int x;
        public int y;

        public HextileTest(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    /// <summary>
    /// Configure the tile parameters using columna nd row data.
    /// </summary>
    /// <param name="q">The Column of the tile.</param>
    /// <param name="r">The Row of the tile.</param>
	public void ConfigureTile(int q, int r)
    {
        Q = q;
        R = r;
        x = Q;
        z = R - ((Q - (Q & 1)) / 2);
        y = -x - z;
    }

    public int GetConnections()
    {
        Connections.Clear();
        HextileTest testCon = new HextileTest();

        testCon = CubetoOddQ(x + 1, z - 1);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        testCon = CubetoOddQ(x + 1, z);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        testCon = CubetoOddQ(x, z + 1);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        testCon = CubetoOddQ(x - 1, z + 1);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        testCon = CubetoOddQ(x - 1, z);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        testCon = CubetoOddQ(x, z - 1);
        if (validateOddQ(testCon)) Connections.Add(MapGenerator.Map[testCon.x, testCon.y]);

        return Connections.Count;
    }

    HextileTest CubetoOddQ(int x, int z)
    {
        return new HextileTest(x, z + (x - (x & 1)) / 2);
    }

    bool validateOddQ(HextileTest testCon)
    {
        if(testCon.x >= 0 && testCon.x < MapGenerator.Map.GetLength(0) && testCon.y >= 0 && testCon.y < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

    void ClearConnections()
    {
        Connections.Clear();
    }

    void UnclearConnections()
    {
        GetConnections();
    }

    public float CalcHexManhattanDist(HexTile TargetTile)
    {
        return ((Mathf.Abs(TargetTile.x - x) + Mathf.Abs(TargetTile.y - y) + Mathf.Abs(TargetTile.z - z)) / 2f);
    }
}
