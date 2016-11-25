using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HexTransform
{
    /// <summary>
    /// The combined row and column position.
    /// </summary>
    public Vector2 RowColumn;
    /// <summary>
    /// The hex grid x y z position.
    /// </summary>
    public Vector3 Position;

    public HexTransform(Vector3 position)
    {
        RowColumn = new Vector2(position.x, position.z + (position.x - ((int)position.x & 1)) / 2);
        Position = position;
    }

    public HexTransform(Vector2 rowColumn)
    {
        float z = rowColumn.x - ((rowColumn.y - ((int)rowColumn.y & 1)) / 2);

        RowColumn = rowColumn;
        Position = new Vector3(rowColumn.x, -rowColumn.x - z, z);
    }
    
    public HexTransform(int X, int Y, int Z)
    {
        RowColumn = new Vector2(X, Z + (X - (X & 1)) / 2);
        Position = new Vector3(X, Y, Z);
    }

    public HexTransform(int Q, int R)
    {
        int z = R - ((Q - (Q & 1)) / 2);

        RowColumn = new Vector2(Q, R);
        Position = new Vector3(Q, -Q - z, z);
    }

    /// <summary>
    /// Calculates the Hex Manhatton distance from this tile to the specified tile.
    /// </summary>
    /// <param name="TargetTile">The tile you want to find the distance to.</param>
    /// <returns></returns>
    public int CalcHexManhattanDist(HexTransform TargetTile)
    {
        return (int)((Mathf.Abs(TargetTile.Position.x - Position.x) + Mathf.Abs(TargetTile.Position.y - Position.y) + Mathf.Abs(TargetTile.Position.z - Position.z)) / 2f);
    }

    public HexTransform CubetoOddQ(float X, float Y, float Z)
    {
        return new HexTransform(new Vector3(X, Y, Z));
    }

    public bool validateOddQ()
    {
        if (RowColumn.x >= 0 && RowColumn.x < MapGenerator.Map.GetLength(0) && RowColumn.y >= 0 && RowColumn.y < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

    public bool validateOddQ(HexTransform testCon)
    {
        if (testCon.RowColumn.x >= 0 && testCon.RowColumn.x < MapGenerator.Map.GetLength(0) && testCon.RowColumn.y >= 0 && testCon.RowColumn.y < MapGenerator.Map.GetLength(1))
        {
            return true;
        }
        return false;
    }

}

public class HexTile : MonoBehaviour
{
    #region Variables

    #region Public

    public HexTransform hexTransform;
    public TerrainTypes TerrainType;
    public bool IsExlusionZone = false;
    public List<HexTile> Connections = new List<HexTile>();
	public AStarInfo ASI;
    public DijkstraInfo DI;
    public float traverseSpeed;

    #endregion

    #endregion

    #region Classes

    #region Public

    public class AStarInfo
    {
        public HexTile root;
        public float ETC;
        public float costSoFar;
        public float heuristic;
    }

    public class DijkstraInfo
    {
        public HexTile root;
        public float costSoFar;
    }

    #endregion

    #endregion

    #region Functions

    #region Public

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

        testCon = testCon.CubetoOddQ(hexTransform.Position.x + 1, hexTransform.Position.y, hexTransform.Position.z - 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

        testCon = testCon.CubetoOddQ(hexTransform.Position.x + 1, hexTransform.Position.y, hexTransform.Position.z);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

        testCon = testCon.CubetoOddQ(hexTransform.Position.x, hexTransform.Position.y, hexTransform.Position.z + 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

        testCon = testCon.CubetoOddQ(hexTransform.Position.x - 1, hexTransform.Position.y, hexTransform.Position.z + 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

        testCon = testCon.CubetoOddQ(hexTransform.Position.x - 1, hexTransform.Position.y, hexTransform.Position.z);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

        testCon = testCon.CubetoOddQ(hexTransform.Position.x, hexTransform.Position.y, hexTransform.Position.z - 1);
        if (testCon.validateOddQ()) Connections.Add(MapGenerator.Map[(int)testCon.RowColumn.x, (int)testCon.RowColumn.y]);

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
    /// Set the colour of this tile.
    /// </summary>
    /// <param name="colour">The Colour you want to set it to.</param>
    public void SetColour(Color colour)
    {
        GetComponent<Renderer>().material.color = colour;
    }


    public void SetTexture(TerrainTypes terrain)
    {
        GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("Textures/" + terrain.ToString());
    }


    #endregion

    #region Private

    /// <summary>
    /// Calculates the specified area.
    /// </summary>
    /// <param name="Center">The center of the area you want to search.</param>
    /// <param name="Radius">The radius of the area.</param>
    /// <param name="list">A reference to a list that will contain the area.</param>
    private void GetHexArea(HexTile Center, int Radius, ref List<HexTile> list, ref List<HexTile> ClosedList)
    {
        if (Connections.Count != 0 && !IsExlusionZone) list.Add(this);

        foreach (HexTile c in Connections)
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

    #endregion

    #endregion
}
