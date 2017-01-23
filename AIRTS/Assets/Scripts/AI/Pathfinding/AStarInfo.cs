using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarInfo<T>
{
    public object root;
    public float ETC;
    public float costSoFar;
    public float heuristic;
    public List<AStarInfo<T>> Connections = new List<AStarInfo<T>>();
    public float cost;
    public T current;

    public AStarInfo(T Current, float Cost)
    {
        current = Current;
        cost = Cost;
    }

    public void SetConnections(List<AStarInfo<T>> ListOFConnections)
    {
        Connections.Clear();
        Connections = ListOFConnections;
    }

}
