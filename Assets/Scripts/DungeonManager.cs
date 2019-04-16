using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemapWall;
    [SerializeField] Tilemap tilemapFloor;
    [SerializeField] Tilemap tilemapBlack;
    [SerializeField] Tile blackTile;
    
    private void Awake()
    {
        Debug.Log("Tilemap count: " + tilemapWall.cellBounds.max);
        foreach (var tilePosition in tilemapWall.cellBounds.allPositionsWithin)
            tilemapBlack.SetTile(tilePosition, blackTile);
        foreach (var tilePosition in tilemapFloor.cellBounds.allPositionsWithin)
            tilemapBlack.SetTile(tilePosition, blackTile);
    }
}
