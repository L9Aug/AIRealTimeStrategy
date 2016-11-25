using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ASImplementation : MonoBehaviour
{
    public static ASImplementation ASI;

    public HexTile start;
    public HexTile destination;
    public List<HexTile> open = new List<HexTile>();
    public List<HexTile> closed = new List<HexTile>();

    public enum ASStates { waitingForStart, waitingForEnd, processing, waitingForClear }
    public ASStates state = ASStates.waitingForStart;

    // Use this for initialization
    void Start()
    {
        ASI = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public List<HexTile> AStar(HexTile start, HexTile destination)
    {
        List<HexTile> temp = new List<HexTile>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            HexTile T = GetLowestETC();
            if (T != null && T != destination)
            {
                AddConnectionsToOpen(T);
            }
            else if (T == destination)
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
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = tempTile.ASI.root;
            }

            temp.Reverse();
        }
        state = ASStates.waitingForClear;

        return temp;
    }


    public List<HexTile> AStar()
    {
        List<HexTile> temp = new List<HexTile>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start);

        while (!destinationFound && !noValidPath)
        {
            HexTile T = GetLowestETC();
            if (T != null && T != destination)
            {
                AddConnectionsToOpen(T);
            }
            else if (T == destination)
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
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = tempTile.ASI.root;
            }

            temp.Reverse();
        }
        state = ASStates.waitingForClear;

        return temp;
    }

    HexTile GetLowestETC()
    {
        if (open.Count > 0)
        {
            HexTile tempTile = open[0];
            float lowestCost = tempTile.ASI.ETC;
            if (open.Count > 1)
            {
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].ASI.ETC < lowestCost)
                    {
                        lowestCost = open[i].ASI.ETC;
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
        if (hex != null)
        {
            foreach (HexTile c in hex.Connections)
            {
                //If the tested tile has connections it is not impassable
                if (c.Connections.Count > 0)
                {
                    if (!open.Contains(c) && !closed.Contains(c))
                    {
                        open.Add(c);
                        c.ASI.costSoFar = hex.ASI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f);
                        c.ASI.heuristic = c.hexTransform.CalcHexManhattanDist(destination.hexTransform);
                        c.ASI.ETC = c.ASI.costSoFar + c.ASI.heuristic;
                        c.ASI.root = hex;
                    }
                    else if (c.ASI.costSoFar > hex.ASI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f))
                    {
                        c.ASI.costSoFar = hex.ASI.costSoFar + (c.traverseSpeed / 2f) + (hex.traverseSpeed / 2f);
                        c.ASI.heuristic = c.hexTransform.CalcHexManhattanDist(destination.hexTransform);
                        c.ASI.ETC = c.ASI.costSoFar + c.ASI.heuristic;
                        c.ASI.root = hex;
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

    public void TileClicked(HexTile tile)
    {
        switch (state)
        {
            case ASStates.waitingForStart:
                start = tile;
                state = ASStates.waitingForEnd;
                break;
            case ASStates.waitingForEnd:
                destination = tile;
                state = ASStates.processing;
                AStar();
                break;
            case ASStates.processing:
                break;
            case ASStates.waitingForClear:
                foreach (HexTile t in open)
                {
                    //t.SetColour(Color.white);
                }
                open.Clear();
                foreach (HexTile t in closed)
                {
                    //t.SetColour(Color.white);
                }
                closed.Clear();
                state = ASStates.waitingForStart;
                break;
            default:
                break;
        }
    }
}
