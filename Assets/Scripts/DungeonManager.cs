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
    [SerializeField] GameObject pointsParent;
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject batteryPrefab;
    [SerializeField] GameObject keyPrefab;
    [SerializeField] Tile blackTile;
    
    private List<Vector3> availablePosition = new List<Vector3>();
    private List<Vector3> occupPosition = new List<Vector3>();
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

        if (dungeon == null)
            throw new Exception("Dungeon grid don't load from resource");
            
        var tilemapGround = dungeon.transform.Find("Ground").GetComponent<Tilemap>();
        var tilemapWall = dungeon.transform.Find("Wall").GetComponent<Tilemap>();
        var tilemapBlack = dungeon.transform.Find("Black").GetComponent<Tilemap>();
        
        Size = tilemapWall.cellBounds.size;
        var center = tilemapWall.cellBounds.center;
        
        Vector3 newDungPosition = new Vector3(dungeon.transform.position.x - center.x, dungeon.transform.position.y - center.y, 0);
        dungeon.transform.position = newDungPosition;
        
        WPosition = tilemapWall.cellBounds.center;
        
        foreach (var position in tilemapGround.cellBounds.allPositionsWithin)
        {
            if (tilemapGround.HasTile(position))
            {
                var worldPosition = tilemapGround.CellToWorld(position);
                worldPosition = new Vector3(worldPosition.x + 0.5f, worldPosition.y + 0.5f, -1);
                AvailablePosition.Add(worldPosition);
            }
        }
        
        var ps = dungeon.transform.Find("PlayerSpawnPos");
        
        //TODO For testing
        //BlackTiles(tilemapGround, tilemapWall, tilemapBlack);
        
        RandomSpawnObjects(keyPrefab, itemsParent.transform, missionInfo.NeedKey);
        RandomSpawnObjects(batteryPrefab,itemsParent.transform, missionInfo.BatteryCount);
        var player = SpawnPlayer(ps.position);
        SpawnGhosts(player.transform);

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

    private List<GameObject> RandomSpawnObjects(GameObject prefab, Transform parent, int maxCount, bool ignoreOccup = false)
    {
        int spawnedObjects = 0;
        int mid = AvailablePosition.Count / maxCount;
        int lEdge = mid - (mid / 2);
        int rEdge = mid + (mid / 2);
        bool isFirst = true;
        int lastIndex = 0;
        List<GameObject> result = new List<GameObject>();
        
        while (spawnedObjects < maxCount)
        {
            if (isFirst)
            {
                lastIndex = Random.Range(0, AvailablePosition.Count - 1);
                isFirst = false;
            }

            var currentAvailablePosition = AvailablePosition[lastIndex];
            int randomOffset = Random.Range(lEdge, rEdge);
            lastIndex = lastIndex + randomOffset;
            if (lastIndex >= AvailablePosition.Count)
                lastIndex = lastIndex - AvailablePosition.Count;
            
            bool isOccup = occupPosition.Any(e => e == currentAvailablePosition);
            if (!isOccup || ignoreOccup)
            {
                var createdObj = Instantiate(prefab, currentAvailablePosition, Quaternion.identity, parent);
                occupPosition.Add(currentAvailablePosition);
                result.Add(createdObj);
                spawnedObjects++;
            }
        }

        return result;
    }

    private GameObject SpawnPlayer(Vector3 spawnPosition)
    {
        var player = Instantiate(playerPrefab);
        player.transform.position = spawnPosition;
        return player.gameObject;
    }

    private void SpawnGhosts(Transform targetPlayer)
    {
        GameObject patrolPoint = new GameObject("PatrolPoint");
        var points = RandomSpawnObjects(patrolPoint, pointsParent.transform, 3, true);
        foreach (var point in points)
        {
            var ghost = Instantiate(ghostPrefab, point.transform.position, Quaternion.identity);
            var ghostScript = ghost.GetComponent<Ghost>();
            ghostScript.PatrolPoints = points.ToArray();
            ghostScript.Target = targetPlayer.transform;
        }
    }

    private IEnumerator CoLoadLevel()
    {
        Load();
        yield return new WaitForSeconds(1);
        Debug.Log("LevelLoaded: " + CurrentMissionInfo.MissionId);
        LevelLoaded();
    }
}
