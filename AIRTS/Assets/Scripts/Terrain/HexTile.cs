using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HexTransform
{
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

    public HexTransform(int X, int Y, int Z)
    {
        x = X;
        y = Y;
        z = Z;

        Q = x;
        R = z + (x - (x & 1)) / 2;
    }

    public HexTransform(int q, int r)
    {
        Q = q;
        R = r;

        x = Q;
        z = R - ((Q - (Q & 1)) / 2);
        y = -x - z;
    }

    /// <summary>
    /// Calculates the Hex Manhatton distance from this tile to the specified tile.
    /// </summary>
    /// <param name="TargetTile">The tile you want to find the distance to.</param>
    /// <returns></returns>
    public int CalcHexManhattanDist(HexTransform TargetTile)
    {
        return (int)((Mathf.Abs(TargetTile.x - x) + Mathf.Abs(TargetTile.y - y) + Mathf.Abs(TargetTile.z - z)) / 2f);
    }

    public HexTransform CubetoOddQ(int X, int Y, int Z)
    {
        return new HexTransform(X, Y, Z);
    }

    public bool validateOddQ()
    {
        if (Q >= 0 && Q < MapGenerator.Map.GetLength(0) && R >= 0 && R < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

    public bool validateOddQ(HexTransform testCon)
    {
        if (testCon.Q >= 0 && testCon.Q < MapGenerator.Map.GetLength(0) && testCon.R >= 0 && testCon.R < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

}

public class HexTile : MonoBehaviour
{
    public HexTransform hexTransform;

    public bool IsExlusionZone = false;

    public List<HexTile> Connections = new List<HexTile>();

    /// <summary>
    /// Configure the tile parameters using columna nd row data.
    /// </summary>
    /// <param name="q">The Column of the tile.</param>
    /// <param name="r">The Row of the tile.</param>
	public void ConfigureTile(int q, int r)
    {
        hexTransform = new HexTransform(q, r);
    }

    public int GetConnections()
    {
        Connections.Clear();
        HexTransform testCon = new HexTransform(0, 0);

        testCon = testCon.CubetoOddQ(hexTransform.x + 1, hexTransform.y, hexTransform.z - 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        testCon = testCon.CubetoOddQ(hexTransform.x + 1, hexTransform.y, hexTransform.z);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        testCon = testCon.CubetoOddQ(hexTransform.x, hexTransform.y, hexTransform.z + 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        testCon = testCon.CubetoOddQ(hexTransform.x - 1, hexTransform.y, hexTransform.z + 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        testCon = testCon.CubetoOddQ(hexTransform.x - 1, hexTransform.y, hexTransform.z);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        testCon = testCon.CubetoOddQ(hexTransform.x, hexTransform.y, hexTransform.z - 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[testCon.Q, testCon.R]);

        return Connections.Count;
    }    

    public void ClearConnections()
    {
        Connections.Clear();
    }

    public void UnclearConnections()
    {
        GetConnections();
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
        return GetHexArea(this, Radius);
    }

    /// <summary>
    /// Calculates the specified area.
    /// </summary>
    /// <param name="Center">The center of the area you want to search.</param>
    /// <param name="Radius">The radius of the area.</param>
    /// <param name="list">A reference to a list that will contain the area.</param>
    private void GetHexArea(HexTile Center, int Radius, ref List<HexTile> list, ref List<HexTile> ClosedList)
    {
        if (Connections.Count != 0 && !IsExlusionZone) list.Add(this);

        foreach(HexTile c in Connections)
        {
            float dist = c.hexTransform.CalcHexManhattanDist(Center.hexTransform);

            if (dist < Radius)
            {
                if (!ClosedList.Contains(c) && !list.Contains(c))
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
        return GetHexRing(this, Radius);
    }

    /// <summary>
    /// Calcualtes a hex ring of the specified dimentions from a tile.
    /// </summary>
    /// <param name="Center">The center of the ring.</param>
    /// <param name="Radius">The radius of the ring.</param>
    /// <param name="list">A reference to a list that will contain the hex ring.</param>
    /// <param name="ignoreList">A reference to a list that will store the tiles leading up to the ring.(stops it from checking the same tile more than once)</param>
    private void GetHexRing(HexTile Center, int Radius, ref List<HexTile> list, ref List<HexTile> ignoreList)
    {
        foreach (HexTile c in Connections)
        {
            float dist = c.hexTransform.CalcHexManhattanDist(Center.hexTransform);

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

    /// <summary>
    /// Returns a list of tiles that make up the specified hex ring.
    /// </summary>
    /// <param name="Center">The center of the ring.</param>
    /// <param name="Radius">The radius of the ring.</param>
    /// <returns>A list containing the hex ring.</returns>
    public List<HexTile> GetHexRing(HexTransform Center, int Radius)
    {
        List<HexTile> ReturnList = new List<HexTile>();
        List<HexTile> IgnoreList = new List<HexTile>();

        GetHexRing(Center, Radius, ref ReturnList, ref IgnoreList);

        return ReturnList;
    }

    /// <summary>
    /// Calcualtes a hex ring of the specified dimentions from a HexTransform.
    /// </summary>
    /// <param name="Center">The center of the ring.</param>
    /// <param name="Radius">The radius of the ring.</param>
    /// <param name="list">A reference to a list that will contain the hex ring.</param>
    /// <param name="ignoreList">A reference to a list that will store the tiles leading up to the ring.(stops it from checking the same tile more than once)</param>
    private void GetHexRing(HexTransform Center, int Radius, ref List<HexTile> list, ref List<HexTile> ignoreList)
    {
        foreach (HexTile c in Connections)
        {
            float dist = c.hexTransform.CalcHexManhattanDist(Center);

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

    /// <summary>
    /// Set the colour of this tile.
    /// </summary>
    /// <param name="colour">The Colour you want to set it to.</param>
    public void SetColour(Color colour)
    {
        GetComponent<Renderer>().material.color = colour;
    }

}
