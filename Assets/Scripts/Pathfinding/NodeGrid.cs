using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public float nodeRadius;

    Vector2Int gridWorldSize;
    AStarNode[,] grid;
    MapController mapController;

    int gridSizeX;
    int gridSizeY;
    float nodeDiameter;
    public int obstacleProximityPenalty = 10;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    Vector2 gridLoc;


    #region Singleton

    public static NodeGrid instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one NodeGrid found!");
            return;
        }
        instance = this;
    }

    #endregion

    private void Start()
    {
        mapController = MapController.instance;
        gridWorldSize = new Vector2Int(mapController.width, mapController.height);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridLoc = new Vector2(transform.position.x, transform.position.y);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];

        for (int x=0;x<gridSizeX;x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = gridLoc + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !mapController.worldTiles.GetTile(mapController.worldTiles.WorldToCell(new Vector3(worldPoint.x, worldPoint.y, 0)));
                int movementPenalty = 0;

                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[x, y] = new AStarNode(null,0,0,walkable,new Vector2Int(x,y), worldPoint, movementPenalty);
            }
        }

        BlurPenaltyMap(3);
    }

    public void UpdateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                bool walkable = !mapController.worldTiles.GetTile(mapController.worldTiles.WorldToCell(new Vector3(grid[x,y].GetWorldLocation().x, grid[x,y].GetWorldLocation().y, 0)));
                grid[x, y].traversable=walkable;
            }
        }
    }

    public void UpdateGrid(int x, int y)
    {
        bool walkable = !mapController.worldTiles.GetTile(mapController.worldTiles.WorldToCell(new Vector3(grid[x, y].GetWorldLocation().x, grid[x, y].GetWorldLocation().y, 0)));
        grid[x, y].traversable = walkable;
    }

    public AStarNode GetNodeFromPosition(Vector2 loc)
    {
        float percentX = Mathf.Clamp01(loc.x / gridWorldSize.x);
        float percentY = Mathf.Clamp01(loc.y / gridWorldSize.y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void BlurPenaltyMap(int blursize)
    {
        int kernelSize = (blursize * 2) + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y=0;y<gridSizeY;y++)
        {
            for (int x=-kernelExtents;x<=kernelExtents;x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x=1;x<gridSizeX;x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents-1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x+kernelExtents, 0, gridSizeX-1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y-1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;
                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<AStarNode> GetNeighbors(AStarNode node)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y == 0 & x == 0)
                {
                    continue;
                }

                int checkX = node.GetGridLocation().x + x;
                int checkY = node.GetGridLocation().y + y;

                if (checkY >= 0 && checkY < gridSizeY && checkX >= 0 && checkX < gridSizeX)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    private void OnDrawGizmos()
    {

        foreach(AStarNode n in grid)
        {
            Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
            Gizmos.color = (n.traversable) ? Gizmos.color : Color.red;
            Gizmos.DrawWireCube(n.GetWorldLocation(), Vector3.one*(nodeDiameter-0.1f));
        }
        
    }
}
