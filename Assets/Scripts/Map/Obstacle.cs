using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewObstacle", menuName = "Obstacle")]
public class Obstacle : ScriptableObject
{
    public TileBase tile;
    public int durability;

}
