using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DijkstraImplementation : MonoBehaviour
{
    public List<DijkstraInfo> open = new List<DijkstraInfo>();
    public List<DijkstraInfo> closed = new List<DijkstraInfo>();

    public enum DJStates { waitingForStart, waitingForEnd, processing, waitingForClear}
    public DJStates state = DJStates.waitingForStart;

    /// <summary>
    /// Dijkstra method to find the shortest path between two tiles
    /// </summary>
    /// <param name="start">Tile to start from</param>
    /// <param name="destination">Tile to find the shortest path to</param>
    /// <returns>List of tiles that is the shortest path between these two tiles</returns>
    public List<DijkstraInfo> Dijkstra(DijkstraInfo start, DijkstraInfo destination)
    {
        List<DijkstraInfo> temp = new List<DijkstraInfo>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            DijkstraInfo tile = GetLowestCost();
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
            DijkstraInfo tempTile = destination;
            while(tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = (DijkstraInfo)tempTile.root;
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
    public List<DijkstraInfo> DijkstraToBuilding(DijkstraInfo start, List<Buildings> destination, int teamId)
    {
        List<DijkstraInfo> temp = new List<DijkstraInfo>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);
        DijkstraInfo destinationTile = new DijkstraInfo();

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            DijkstraInfo tile = GetLowestCost();
            for(int i = 0; i < GlobalAttributes.Global.Buildings.Count; ++i)
            {
                if(GlobalAttributes.Global.Buildings[i].hexTransform.Position == tile.GetComponent<HexTile>().hexTransform.Position)
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
            DijkstraInfo tempTile = destinationTile;
            // THIS NEEDS TO LOOK FOR A TILE ON THE building's exclusion zone
            //tempTile.hexTransform.position = destination.hexTransform.position;
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = (DijkstraInfo)tempTile.root;
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
    public List<DijkstraInfo> Dijkstra<T>(DijkstraInfo start)
    {
        List<DijkstraInfo> temp = new List<DijkstraInfo>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);
        DijkstraInfo destination = new DijkstraInfo();

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            DijkstraInfo tile = GetLowestCost();
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
            DijkstraInfo tempTile = destination;
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = (DijkstraInfo)tempTile.root;
            }

            temp.Reverse();
        }
        //state = DJStates.waitingForClear;

        return temp;
    }


    DijkstraInfo GetLowestCost()
    {
        if (open.Count > 0)
        {
            DijkstraInfo tempTile = open[0];
            float lowestCost = tempTile.costSoFar;
            if (open.Count > 1)
            {
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].costSoFar < lowestCost)
                    {
                        lowestCost = open[i].costSoFar;
                        tempTile = open[i];
                    }
                }
            }
            return tempTile;
        }

        return null;
    }

    void AddConnectionsToOpen(DijkstraInfo hex)
    {
        if(hex != null)
        {
            foreach(DijkstraInfo c in hex.Connections)
            {
                //If the tested tile has connections it is not impassable
                if (c.Connections.Count > 0)
                {
                    if (!open.Contains(c) && !closed.Contains(c))
                    {
                        open.Add(c);
                        c.costSoFar = hex.costSoFar + (c.cost / 2f) + (hex.cost / 2f);
                        c.root = hex;
                    }
                    else if (c.costSoFar > hex.costSoFar + (c.cost / 2f) + (hex.cost / 2f))
                    {
                        c.costSoFar = hex.costSoFar + (c.cost / 2f) + (hex.cost / 2f);
                        c.root = hex;
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
    
}
