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

    public bool IsExlusionZone = false;

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

    public void ClearConnections()
    {
        Connections.Clear();
    }

    public void UnclearConnections()
    {
        GetConnections();
    }

    /// <summary>
    /// Calculates the Hex Manhatton distance from this tile to the specified tile.
    /// </summary>
    /// <param name="TargetTile">The tile you want to find the distance to.</param>
    /// <returns></returns>
    public float CalcHexManhattanDist(HexTile TargetTile)
    {
        return ((Mathf.Abs(TargetTile.x - x) + Mathf.Abs(TargetTile.y - y) + Mathf.Abs(TargetTile.z - z)) / 2f);
    }

    void OnMouseDown()
    {

    }

    /// <summary>
    /// Returns the specified area.
    /// </summary>
    /// <param name="Center">The center of the area.</param>
    /// <param name="Radius">The radius of the area.</param>
    /// <returns>A list of tiles that make up the area.</returns>
    public List<HexTile> GetHexArea(HexTile Center, int Radius)
    {
        List<HexTile> ReturnList = new List<HexTile>();
        List<HexTile> ClosedList = new List<HexTile>();

        GetHexArea(Center, Radius, ref ReturnList, ref ClosedList);

        return ReturnList;
    }

    /// <summary>
    /// Calculates the area with this tile at the center.
    /// </summary>
    /// <param name="Radius">The radius of the area.</param>
    /// <returns>A list of tiles that make up the area.</returns>
    public List<HexTile> GetHexArea(int Radius)
    {
        List<HexTile> ReturnList = new List<HexTile>();
        List<HexTile> ClosedList = new List<HexTile>();

        GetHexArea(this, Radius, ref ReturnList, ref ClosedList);

        return ReturnList;
    }

    /// <summary>
    /// Calculates the specified area.
    /// </summary>
    /// <param name="Center">The center of the area you want to search.</param>
    /// <param name="Radius">The radius of the area.</param>
    /// <param name="list">A reference to a list that will contain the area.</param>
    private void GetHexArea(HexTile Center, int Radius, ref List<HexTile> list, ref List<HexTile> ClosedList)
    {
        if(Connections.Count != 0 && !IsExlusionZone) list.Add(this);

        foreach(HexTile c in Connections)
        {
            float dist = c.CalcHexManhattanDist(Center);

            if (dist < Radius)
            {
                if (!ClosedList.Contains(c))
                {
                    ClosedList.Add(c);
                    c.GetHexArea(Center, Radius, ref list, ref ClosedList);
                }
            }
        }
    }

    /// <summary>
    /// Returns a list of tiles that make up the specified hex ring.
    /// </summary>
    /// <param name="Center">The center of the ring.</param>
    /// <param name="Radius">The radius of the ring.</param>
    /// <returns>A list containing the hex ring.</returns>
    public List<HexTile> GetHexRing(HexTile Center, int Radius)
    {
        List<HexTile> ReturnList = new List<HexTile>();
        List<HexTile> IgnoreList = new List<HexTile>();

        GetHexRing(Center, Radius, ref ReturnList, ref IgnoreList);

        return ReturnList;
    }

    /// <summary>
    /// Calculates a hex ring with this tile at the center.
    /// </summary>
    /// <param name="Radius">The radius of the ring.</param>
    /// <returns>A list containing the hex ring.</returns>
    public List<HexTile> GetHexRing(int Radius)
    {
        List<HexTile> ReturnList = new List<HexTile>();
        List<HexTile> IgnoreList = new List<HexTile>();

        GetHexRing(this, Radius, ref ReturnList, ref IgnoreList);

        return ReturnList;
    }

    /// <summary>
    /// Calcualtes a hex ring of the specified dimentions.
    /// </summary>
    /// <param name="Center">The center of the ring.</param>
    /// <param name="Radius">The radius of the ring.</param>
    /// <param name="list">A reference to a list that will contain the hex ring.</param>
    /// <param name="ignoreList">A reference to a list that will store the tiles leading up to the ring.(stops it from checking the same tile more than once)</param>
    private void GetHexRing(HexTile Center, int Radius, ref List<HexTile> list, ref List<HexTile> ignoreList)
    {
        foreach (HexTile c in Connections)
        {
            float dist = c.CalcHexManhattanDist(Center);

            if (dist + 1 == Radius) // if the connection is on the ring add it to the list and search it's connections.
            {
                if (!list.Contains(c))
                {
                    list.Add(c);
                    c.GetHexRing(Center, Radius, ref list, ref ignoreList);
                }
            }
            else if (dist <= Radius) // if the connection is in the area of the ring add it to the ignore list and search it's connections.
            {
                if (!ignoreList.Contains(c))
                {
                    ignoreList.Add(c);
                    c.GetHexRing(Center, Radius, ref list, ref ignoreList);
                }
            }
        }
    }

}
