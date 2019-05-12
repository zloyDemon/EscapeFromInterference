using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    private const string LevelPrefix = "Levels/Level_{0}";

    [SerializeField] GameObject dungeonParent;
    [SerializeField] GameObject itemsParent;
    [SerializeField] Tile blackTile;
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] GameObject keyPrefab;
    
    
    private List<Vector3> availablePosition = new List<Vector3>();
    private List<Vector3> occupPosition = new List<Vector3>();
    private int ghostsNumber;
    private int batteryCount;
    private int keyCount;
    private Tilemap groundTileMap;
    private Grid _currentPfGrid;

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
        batteryCount = 9;
        keyCount = 3;
    }

    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    private void Load()
    {
        string path = string.Format(LevelPrefix, DungLevel);
        var loadGrid = Resources.Load<Grid>(path);
        Grid dungeon = Instantiate(loadGrid, Vector3.zero, Quaternion.identity, dungeonParent.transform);
        _currentPfGrid = dungeon;
        
        var tilemapGround = dungeon.transform.Find("Ground").GetComponent<Tilemap>();
        
        if(tilemapGround == null)
            throw new Exception("Ground tilemap not founded");

        groundTileMap = tilemapGround;
        
        foreach (var position in tilemapGround.cellBounds.allPositionsWithin)
        {
            if (tilemapGround.HasTile(position))
            {
                var worldPosition = tilemapGround.CellToWorld(position);
                worldPosition = new Vector3(worldPosition.x + 0.5f, worldPosition.y + 0.5f, -1);
                availablePosition.Add(worldPosition);
            }
        }

        var tilemapWall = dungeon.transform.Find("Wall").GetComponent<Tilemap>();
        var tilemapFloor = dungeon.transform.Find("Ground").GetComponent<Tilemap>();
        var tilemapBlack = dungeon.transform.Find("Black").GetComponent<Tilemap>();
        
        //BlackTiles(tilemapFloor, tilemapWall, tilemapBlack);
        
        RandomSpawnObjects(keyPrefab, keyCount);
        RandomSpawnObjects(batteryPrefab, batteryCount);
        SpawnPlayer();
        
        Debug.Log("MapSize: " + tilemapWall.cellBounds.xMin + " " + tilemapWall.cellBounds.xMax);
    }

    private void BlackTiles(Tilemap tilemapFloor, Tilemap tilemapWall, Tilemap tilemapBlack)
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
            var random = Random.Range(-10, 10);
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

    private void SpawnBattery()
    {
        int spawnedBattery = 0;
        for (int i = 0; i < availablePosition.Count; i++)
        {
            int random = Random.Range(-1000, 100);
            bool isOccup = occupPosition.Any(e => e == availablePosition[i]);
            bool isCanSpawn = random > 0 && !isOccup;
            Debug.Log("Battery spawn: " + isCanSpawn + " " + random);
            if (isCanSpawn)
            {
                Instantiate(batteryPrefab, availablePosition[i], Quaternion.identity);
                occupPosition.Add(availablePosition[i]);
                spawnedBattery++;
            }

            if (spawnedBattery >= batteryCount)
                break;
        }
    }

    public void RandomSpawnObjects(GameObject prefab, int maxCount)
    {
        int spawnedObjects = 0;
        int mid = availablePosition.Count / maxCount;
        int lEdge = mid - (mid / 2);
        int rEdge = mid + (mid / 2);
        bool isFirst = true;
        int lastIndex = 0;
        
        while (spawnedObjects < maxCount)
        {
            if (isFirst)
            {
                lastIndex = Random.Range(0, availablePosition.Count - 1);
                isFirst = false;
            }
            
            int randomOffset = Random.Range(lEdge, rEdge);
            lastIndex = lastIndex + randomOffset;
            if (lastIndex >= availablePosition.Count)
                lastIndex = lastIndex - availablePosition.Count;
            
            bool isOccup = occupPosition.Any(e => e == availablePosition[lastIndex]);
            if (!isOccup)
            {
                Instantiate(prefab, availablePosition[lastIndex], Quaternion.identity, itemsParent.transform);
                occupPosition.Add(availablePosition[lastIndex]);
                spawnedObjects++;
            }
        }
    }

    private void SpawnPlayer()
    {
        int index = Random.Range(0, AvailablePosition.Count);
        var ps = _currentPfGrid.transform.Find("PlayerSpawnPos");
        var spawnPosition = ps.position;
        var player = Instantiate(playerPrefab);
        player.transform.position = spawnPosition;
    }

    private IEnumerator LoadLevel()
    {
        Load();
        LevelLoaded();
        yield break;
    }
    
    
}
