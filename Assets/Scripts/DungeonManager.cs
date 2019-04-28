using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonManager : MonoBehaviour
{
    private const string LevelPrefix = "Levels/Level_{0}";
    
    [SerializeField] Tilemap tilemapWall;
    [SerializeField] Tilemap tilemapFloor;
    [SerializeField] Tilemap tilemapBlack;
    [SerializeField] Tile blackTile;
    [SerializeField] private GameObject ghostPrefab;
    
    private List<Vector3> availablePosition = new List<Vector3>();
    private int ghostsNumber;
    private Tilemap groundTileMap;

    public static DungeonManager Instance { get; private set; }
    public int DungLevel { get; private set; }
    public List<Vector3> AvailablePosition { get { return availablePosition; }}
    public event Action LevelLoaded = () => {};

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if(Instance != this)
            Destroy(gameObject);
              
        DungLevel = 1;
        ghostsNumber = 4;
    }

    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    private void Load()
    {
        string path = string.Format(LevelPrefix, DungLevel);
        var loadGrid = Resources.Load<Grid>(path);
        Grid dungeon = Instantiate(loadGrid, Vector3.zero, Quaternion.identity);

        var tilemapGround = dungeon.transform.Find("Ground").GetComponent<Tilemap>();
        if(tilemapGround == null)
            throw new Exception("Ground tilemap not founded");

        groundTileMap = tilemapGround;
        
        for (int i = tilemapGround.cellBounds.yMin; i < tilemapGround.cellBounds.yMax; i++)
        {
            for (int j = tilemapGround.cellBounds.xMin; j < tilemapGround.cellBounds.yMax; j++)
            {
                var localPose = new Vector3Int(i, j, 0);
                if (tilemapGround.HasTile(localPose))
                {
                    var worldPosition = tilemapGround.CellToWorld(new Vector3Int(i, j, 0));
                    worldPosition = new Vector3(worldPosition.x + 0.5f, worldPosition.y + 0.5f, 0);
                    availablePosition.Add(worldPosition);
                }
            }  
        }
        
        SpawnGhosts();
    }

    private void BlackTiles()
    {
        foreach (var tilePosition in tilemapWall.cellBounds.allPositionsWithin)
            tilemapBlack.SetTile(tilePosition, blackTile);
        foreach (var tilePosition in tilemapFloor.cellBounds.allPositionsWithin)
            tilemapBlack.SetTile(tilePosition, blackTile);
    }

    private void SpawnGhosts()
    {
        if (availablePosition == null || availablePosition.Count == 0)
            return;
        int spawnedGhosts = 0;
        for (var i = 0; i < availablePosition.Count; i++)
        {
            var random = UnityEngine.Random.Range(-10, 10);
            bool isCanSpawn = random > 0;
            if (isCanSpawn)
            {
                Instantiate(ghostPrefab, availablePosition[i], Quaternion.identity);
                spawnedGhosts++;
            }
            if(spawnedGhosts >= ghostsNumber)
                break;
        }
    }

    private IEnumerator LoadLevel()
    {
        Load();
        LevelLoaded();
        yield break;
    }
    
    
}
