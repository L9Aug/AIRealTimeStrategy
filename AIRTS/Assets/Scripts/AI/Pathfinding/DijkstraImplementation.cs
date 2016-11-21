using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DijkstraImplementation : MonoBehaviour
{
    public static DijkstraImplementation DJI;

    public List<HexTile> open = new List<HexTile>();
    public List<HexTile> closed = new List<HexTile>();

    public enum DJStates { waitingForStart, waitingForEnd, processing, waitingForClear}
    public DJStates state = DJStates.waitingForStart;

    /// <summary>
    /// Dijkstra method to find the shortest path between two tiles
    /// </summary>
    /// <param name="start">Tile to start from</param>
    /// <param name="destination">Tile to find the shortest path to</param>
    /// <returns>List of tiles that is the shortest path between these two tiles</returns>
    public List<HexTile> Dijkstra(HexTile start, HexTile destination)
    {
        List<HexTile> temp = new List<HexTile>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            HexTile tile = GetLowestCost();
            if(tile != null && tile != destination)
            {
                AddConnectionsToOpen(tile);
            } else if(tile == destination)
            {
                destinationFound = true;
            }
            else
            {
                noValidPath = true;
            }
        }

        if (destinationFound)
        {
            HexTile tempTile = destination;
            while(tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = tempTile.DI.root;
            }

            temp.Reverse();
        }
        //state = DJStates.waitingForClear;
        
        return temp;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="destination"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public List<HexTile> DijkstraToBuilding(HexTile start, List<Buildings> destination, int teamId)
    {
        List<HexTile> temp = new List<HexTile>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);
        HexTile destinationTile = new HexTile();

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            HexTile tile = GetLowestCost();
            for(int i = 0; i < GlobalAttributes.Global.Buildings.Count; ++i)
            {
                if(GlobalAttributes.Global.Buildings[i].hexTransform.position == tile.hexTransform.position)
                {
                    for(int j = 0; j < destination.Count; ++j)
                    {
                        if(GlobalAttributes.Global.Buildings[i].BuildingType == destination[j])
                        {
                            destinationFound = true;
                            destinationTile = tile;
                        }
                        if (tile != null && GlobalAttributes.Global.Buildings[i].BuildingType != destination[j])
                        {
                            AddConnectionsToOpen(tile);
                        }
                        else
                        {
                            noValidPath = true;
                        }
                    }
                }
            }
            
        }

        if (destinationFound)
        {
            HexTile tempTile = destinationTile;
            // THIS NEEDS TO LOOK FOR A TILE ON THE building's exclusion zone
            //tempTile.hexTransform.position = destination.hexTransform.position;
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = tempTile.DI.root;
            }

            temp.Reverse();
        }
        //state = DJStates.waitingForClear;

        return temp;
    }

    /// <summary>
    /// Dijkstra method to find a tile that has a script attached of type T
    /// </summary>
    /// <typeparam name="T">Type to find on destination tile</typeparam>
    /// <param name="start">Tile to start from</param>
    /// <returns>List of tiles between the start and found tile</returns>
    public List<HexTile> Dijkstra<T>(HexTile start)
    {
        List<HexTile> temp = new List<HexTile>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);
        HexTile destination = new HexTile();

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            HexTile tile = GetLowestCost();
            if (tile != null && tile != destination)
            {
                AddConnectionsToOpen(tile);
            }
            else if (tile.GetComponent<T>() != null)
            {
                destination = tile;
                destinationFound = true;
            }
            else
            {
                noValidPath = true;
            }
        }

        if (destinationFound)
        {
            HexTile tempTile = destination;
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = tempTile.DI.root;
            }

            temp.Reverse();
        }
        //state = DJStates.waitingForClear;

        return temp;
    }


    HexTile GetLowestCost()
    {
        if (open.Count > 0)
        {
            HexTile tempTile = open[0];
            float lowestCost = tempTile.DI.costSoFar;
            if (open.Count > 1)
            {
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].DI.costSoFar < lowestCost)
                    {
                        lowestCost = open[i].DI.costSoFar;
                        tempTile = open[i];
                    }
                }
            }
            return tempTile;
        }

        return null;
    }

    void AddConnectionsToOpen(HexTile hex)
    {
        if(hex != null)
        {
            foreach(HexTile c in hex.Connections)
            {
                //If the tested tile has connections it is not impassable
                if (c.Connections.Count > 0)
                {
                    if (!open.Contains(c) && !closed.Contains(c))
                    {
                        open.Add(c);
                        c.DI.costSoFar = hex.DI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f);
                        c.DI.root = hex;
                    }
                    else if (c.DI.costSoFar > hex.DI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f))
                    {
                        c.DI.costSoFar = hex.DI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f);
                        c.DI.root = hex;
                        if (closed.Contains(c))
                        {
                            closed.Remove(c);
                            open.Add(c);
                        }
                    }
                }
            }
            open.Remove(hex);
            closed.Add(hex);
        }
    }

    /*public void TileClicked(HexTile tile)
    {
        switch (state)
        {
            case DJStates.waitingForStart:
                start = tile;
                state = DJStates.waitingForEnd;
                break;
            case DJStates.waitingForEnd:
                destination = tile;
                state = DJStates.processing;
                Dijkstra();
                break;
            case DJStates.processing:
                break;
            case DJStates.waitingForClear:
                foreach(HexTile t in open)
                {
                    t.SetColour(Color.white);
                }
                open.Clear();
                foreach (HexTile t in closed)
                {
                    t.SetColour(Color.white);
                }
                closed.Clear();
                state = DJStates.waitingForStart;
                break;
            default:
                break;
        }
    }*/

	// Use this for initialization
	void Start () {
        DJI = this;

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnDestroy()
    {
        DJI = null;
    }
}
