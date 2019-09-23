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
    public event Action LevelLoaded = () => {};
    public Vector3 WPosition { get; private set; }
    public Vector3 Size { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if(Instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("DungeonManager destroyed.");
    }

    public void LoadLevel()
    {
        missionInfo = MissionManager.Instance.CurrentMission;
        Debug.Log($"DungeonManager: LoadLevel: {missionInfo.Level}");
        StartCoroutine(CoLoadLevel());
    }

    private IEnumerator Load()
    {
        string path = string.Format(LevelPrefix, missionInfo.Level);
        var loadGrid = Resources.Load<Grid>(path);
        Grid dungeon = Instantiate(loadGrid, Vector3.zero, Quaternion.identity, dungeonParent.transform);

        if (dungeon == null)
            throw new Exception("Dungeon grid don't load from resource");
            
        var tilemapGround = dungeon.transform.Find("Ground").GetComponent<Tilemap>();
        var tilemapWall = dungeon.transform.Find("Wall").GetComponent<Tilemap>();
        var tilemapBlack = dungeon.transform.Find("Black").GetComponent<Tilemap>();

        int cellUnit = (int) dungeon.cellSize.x;
        
        Size = tilemapWall.cellBounds.size * cellUnit;
        
        var center = tilemapWall.cellBounds.center * cellUnit;
        
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
        
        BlackTiles(tilemapGround, tilemapWall, tilemapBlack);
        
        RandomSpawnObjects(keyPrefab, itemsParent.transform, missionInfo.NeedKeys);
        RandomSpawnObjects(batteryPrefab,itemsParent.transform, missionInfo.BatteryCount);
        var player = SpawnPlayer(ps.position);
        
        var patrolPointsParent = dungeon.transform.Find("PatrolPoints");
        List<GameObject> patrolPointsForGhost = new List<GameObject>();
        for (int i = 0; i < patrolPointsParent.childCount; i++)
            patrolPointsForGhost.Add(patrolPointsParent.GetChild(i).gameObject);
        SpawnGhosts(player.transform, patrolPointsForGhost);
        
        yield break;
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

    private void SpawnGhosts(Transform targetPlayer, List<GameObject> patrolPoints)
    {
        GameObject patrolPoint = new GameObject("PatrolPoint");
        List<GameObject> ghosts = new List<GameObject>();
        var points = patrolPoints;
        bool flag = false;
        foreach (var point in points)
        {
            if(flag)
                continue;
            var ghost = Instantiate(ghostPrefab, point.transform.position, Quaternion.identity);
            var ghostScript = ghost.GetComponent<Ghost>();
            ghostScript.PatrolPoints = patrolPoints.ToArray();
            ghostScript.Target = targetPlayer.transform;
            ghosts.Add(ghost);
            flag = true;
        }

        GameplayManager.Instance.GhostOnMap = ghosts;
    }

    private IEnumerator CoLoadLevel()
    {
        yield return Load();
        Debug.Log("LevelLoaded: " + MissionManager.Instance.CurrentMission.MissionIndex);
        LevelLoaded();
    }
}
