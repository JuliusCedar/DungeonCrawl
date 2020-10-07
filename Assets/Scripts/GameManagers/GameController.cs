using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    #region Singleton

    public static GameController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Game Controller found!");
            return;
        }
        instance = this;
    }

    #endregion

    PlayerController player;
    
    public Tilemap obstacleTiles;
    
    MapGenerator mapGen;
    MapController mapCon;

    public AudioSource backGroundMusic;

    System.Random rand;

    void Start()
    {
        Physics2D.gravity = Vector2.zero;
        rand = new System.Random();
        mapGen = GetComponent<MapGenerator>();
        mapCon = MapController.instance;
        player = PlayerController.instance;
    }

    public void StartGame()
    {
        backGroundMusic.Play();
        mapGen.GenerateMap(mapCon.width, mapCon.height);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3Int loc;

        bool found = false;

        //Find an unoccupied spot within the map to place the player
        while (!found)
        {
            loc = new Vector3Int(rand.Next(0, mapCon.width), rand.Next(0, mapCon.height), 0);
            if (!obstacleTiles.HasTile(loc))
            {
                found = true;
                player.transform.position = obstacleTiles.CellToWorld(loc);
            }
        }
    }
}