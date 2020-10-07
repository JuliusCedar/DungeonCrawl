using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using System.Diagnostics;

public class MapGenerator : MonoBehaviour
{
    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;
    public int smoothIterations;

    public GameObject enemyParent;

    public NodeGrid grid;

    public GridInformation worldInfo;

    public Tilemap obstacles;

    public Obstacle wallObs;
    public Obstacle lanternObs;

    public Tilemap background;
    public TileBase backgroundTile;
    public TileBase pathTile;

    public GameObject enemyPrefab;

    private System.Random prng;

    MapController mapController;

    Stopwatch stopwatch;

    private void Start()
    {
        stopwatch = new Stopwatch();
        mapController = GetComponent<MapController>();
        prng = new System.Random(useRandomSeed ? System.DateTime.Now.Ticks.ToString().GetHashCode() : seed.GetHashCode());
    }

    public void GenerateMap(int width, int height)
    {
        int[,] wallMap = GenerateCellMap(width, height, randomFillPercent, smoothIterations);
        int[,] pathMap = GenerateCellMap(width, height, randomFillPercent - 3, smoothIterations / 2);

        FloodTileMap(background, backgroundTile, width, height);

        TranscribeTiles(background, pathTile, pathMap, width, height);

        TranscribeObstacles(obstacles, wallObs, wallMap, width, height);

        for (int i=0;i<15;i++)
        {
            Vector3Int tilePos = GetRandomOpenTile(obstacles, width, height);
            obstacles.SetTile(tilePos, lanternObs.tile);
            worldInfo.SetPositionProperty(tilePos, "durability", lanternObs.durability);
        }
        
        for (int i=0;i<50;i++)
        {
            GameObject.Instantiate(enemyPrefab, GetRandomOpenTile(obstacles, width, height), Quaternion.identity, enemyParent.transform);
            //GetRandomOpenTile(obstacles, width, height);
        }
        
        grid.UpdateGrid();
    }

    //Generates a random map, smoothed to have cavelike structures
    private int[,] GenerateCellMap(int w, int h, int fillPercent, int smoothNum)
    {
        int[,] output = RandomFillMap(w, h, fillPercent);

        for (int i = 0; i < smoothNum; i++)
        {
            output = SmoothMap(output, w, h);
        }

        return output;
    }

    private int[,] RandomFillMap(int w, int h, int fillPercent)
    {
        int[,] output = new int[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                {
                    output[x, y] = 1;
                }
                else
                {
                    output[x, y] = (prng.Next(0, 100) < fillPercent) ? 1 : 0;
                }
            }
        }

        return output;
    }

    private int[,] SmoothMap(int[,] input, int w, int h)
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int neighborWallTiles = GetSurroundingActiveCellCount(input, x, y);

                if (neighborWallTiles > 4)
                {
                    input[x, y] = 1;
                }
                else if (neighborWallTiles < 4)
                {
                    input[x, y] = 0;
                }
            }
        }

        return input;
    }

    private int GetSurroundingActiveCellCount(int[,] input, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < MapController.instance.width && neighborY >= 0 && neighborY < MapController.instance.height)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += input[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    private void FloodTileMap(Tilemap input, TileBase tile, int w, int h)
    {
        input.ClearAllTiles();

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                input.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void TranscribeObstacles(Tilemap input, Obstacle obs, int[,] reference, int w, int h)
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3Int tempLoc = new Vector3Int(x, y, 0);
                if (reference[x,y] == 1 && input.GetTile(tempLoc) == null)
                {
                    input.SetTile(new Vector3Int(x, y, 0), obs.tile);
                    worldInfo.SetPositionProperty(tempLoc, "durability", obs.durability);
                }
            }
        }
    }

    private void TranscribeTiles(Tilemap input, TileBase brush, int[,] reference, int w, int h)
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3Int tempLoc = new Vector3Int(x, y, 0);
                if (reference[x, y] == 1)
                {
                    input.SetTile(new Vector3Int(x, y, 0), brush);
                }
            }
        }
    }

    private Vector3Int GetRandomOpenTile(Tilemap input, int w, int h)
    {
        bool found = false;
        Vector3Int temp = new Vector3Int(-1,-1,-1);

        while (!found)
        {
            temp = new Vector3Int(prng.Next(0,w),prng.Next(0,h),0);
            if (input.GetTile(temp) == null)
            {
                found = true;
            }
        }

        return temp;
    }
}
