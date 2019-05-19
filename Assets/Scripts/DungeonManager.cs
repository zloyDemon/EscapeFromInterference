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
    private Tilemap groundTileMap;
    private Grid currentTileGrid;
    private MissionInfo missionInfo;

    public static DungeonManager Instance { get; private set; }
    public int DungLevel { get { return missionInfo.Level; }}
    public List<Vector3> AvailablePosition { get { return availablePosition; }}
    public MissionInfo CurrentMissionInfo { get { return missionInfo; }}
    public event Action LevelLoaded = () => {};
    public Vector3 WPosition { get; private set; }
    public Vector3 Size { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if(Instance != this)
            Destroy(gameObject);

        var missionId = GameConfig.Instance.Level;
        missionInfo = new MissionInfo(missionId,3,5,3);
    }

    public void LoadLevel()
    {
        StartCoroutine(CoLoadLevel());
    }

    private void Load()
    {
        string path = string.Format(LevelPrefix, missionInfo.MissionId);
        var loadGrid = Resources.Load<Grid>(path);
        Grid dungeon = Instantiate(loadGrid, Vector3.zero, Quaternion.identity, dungeonParent.transform);
        currentTileGrid = dungeon;
        
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
        
        BlackTiles(tilemapFloor, tilemapWall, tilemapBlack);
        
        RandomSpawnObjects(keyPrefab, missionInfo.NeedKey);
        RandomSpawnObjects(batteryPrefab, missionInfo.BatteryCount);
        SpawnPlayer();

        var dungPos = tilemapWall.GetCellCenterWorld(tilemapWall.cellBounds.position);
        
        Size = tilemapWall.cellBounds.size;
        WPosition = new Vector3(Mathf.FloorToInt(dungPos.x + (Size.x / 2)), Mathf.FloorToInt(dungPos.y + (Size.y / 2)), dungPos.z);
        WPosition = tilemapWall.cellBounds.center;

        Debug.Log("MapSize: " + tilemapWall.CellToWorld(tilemapWall.cellBounds.position) + " " + tilemapWall.size + " " + tilemapWall.cellBounds.center);
    }

    private void BlackTiles(Tilemap tilemapFloor, Tilemap tilemapWall, Tilemap tilemapBlack)
    {
        foreach (var tilePosition in tilemapWall.cellBounds.allPositionsWithin)
            if(tilemapWall.HasTile(tilePosition))
                tilemapBlack.SetTile(tilePosition, blackTile);
        
        foreach (var tilePosition in tilemapFloor.cellBounds.allPositionsWithin)
            if(tilemapFloor.HasTile(tilePosition))
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
            if(spawnedGhosts >= missionInfo.GhostCount)
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

            if (spawnedBattery >= missionInfo.BatteryCount)
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
        var ps = currentTileGrid.transform.Find("PlayerSpawnPos");
        var spawnPosition = ps.position;
        var player = Instantiate(playerPrefab);
        player.transform.position = spawnPosition;
    }

    private IEnumerator CoLoadLevel()
    {
        Load();
        Debug.Log("LevelLoaded:");
        yield return new WaitForSeconds(1);
        LevelLoaded();
    }
}
