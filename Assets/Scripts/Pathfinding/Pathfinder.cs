using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Pathfinder : MonoBehaviour
{

    public Tilemap obstacleReference;
    NodeGrid grid;

    private void Awake()
    {
        grid = GetComponent<NodeGrid>();
    }

    public void GeneratePath(PathRequest request, Action<PathResult> callback)
    {
        AStarNode startNode = grid.GetNodeFromPosition(request.pathStart);
        AStarNode endNode = grid.GetNodeFromPosition(request.pathEnd);

        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        if (startNode.traversable && endNode.traversable)
        {
            Heap<AStarNode> open = new Heap<AStarNode>(grid.MaxSize);
            HashSet<AStarNode> closed = new HashSet<AStarNode>();

            open.Add(startNode);

            while (open.Count > 0)
            {
                AStarNode currentNode = open.RemoveFirst();
                closed.Add(currentNode);

                if (currentNode.GetGridLocation() == endNode.GetGridLocation())
                {
                    //Found Path
                    pathSuccess = true;
                    break;
                }

                foreach (AStarNode neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.traversable || closed.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                    if (newMovementCostToNeighbor < neighbor.gCost || !open.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, endNode);
                        neighbor.parent = currentNode;

                        if (!open.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                        else
                        {
                            open.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, endNode);
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector2[] RetracePath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector2[] SimplifyPath(List<AStarNode> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i=1;i<path.Count;i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].GetGridLocation().x - path[i].GetGridLocation().x, path[i - 1].GetGridLocation().y - path[i].GetGridLocation().y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].GetWorldLocation());
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    public int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GetGridLocation().x - nodeB.GetGridLocation().x);
        int dstY = Mathf.Abs(nodeA.GetGridLocation().y - nodeB.GetGridLocation().y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
