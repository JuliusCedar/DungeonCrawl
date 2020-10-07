using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IHeapItem<AStarNode>
{
    public AStarNode parent;
    public int gCost;
    public int hCost;
    public bool traversable;
    public int movementPenalty;
    Vector2Int gridLocation;
    Vector2 worldLocation;

    int heapIndex;

    public AStarNode(AStarNode p, int h, int g, bool t, Vector2Int gl, Vector2 wl, int mp)
    {
        parent = p;
        hCost = h;
        gCost = g;
        traversable = t;
        movementPenalty = mp;
        gridLocation = gl;
        worldLocation = wl;
    }

    public Vector2Int GetGridLocation()
    {
        return gridLocation;
    }

    public Vector2 GetWorldLocation()
    {
        return worldLocation;
    }

    public int GetFCost()
    {
        return gCost + hCost;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(AStarNode compNode)
    {
        int compare = GetFCost().CompareTo(compNode.GetFCost());
        if (compare == 0)
        {
            compare = hCost.CompareTo(compNode.hCost);
        }
        return -compare;
    }
}
