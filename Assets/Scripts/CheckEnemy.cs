using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    public static readonly float CheckRadius = 100f;

    public enum CheckTargetType
    {
        None,
        Player,
        Ghost,
    }
    
    private CheckTargetType TargetType;
    private bool canCheck;
    public List<GameObject> Enemies { get; private set; }
    public event Action<GameObject[]> EnemyChecked = g => { };

    public void Init(CheckTargetType enemyType)
    {
        TargetType = enemyType;
    }
    
    private void Update()
    {
        if(canCheck)
            Check();
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += LevelLoaded;
    }

    private void LevelLoaded()
    {
        DungeonManager.Instance.LevelLoaded -= LevelLoaded;
        Enemies = GetEnemies(TargetType);
        canCheck = true;
    }

    private void Check()
    {
        var enemies = GetEnemiesArrayByTag(Enemies);
        EnemyChecked(enemies.ToArray());    
    }


    private List<GameObject> GetEnemies(CheckTargetType type)
    {
        List<GameObject> result = new List<GameObject>();
        switch (type)
        {
            case CheckTargetType.Ghost:
                result = GameplayManager.Instance.GhostOnMap;
                break;
            case CheckTargetType.Player:
                var player = GameplayManager.Instance.Player;
                result.Add(player.gameObject);
                break;
            default:
                throw new Exception("CheckEnemy: Type not defined");
        }

        return result;
    }

    private List<GameObject> GetEnemiesArrayByTag(List<GameObject> enemies)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag(TargetType.ToString()))
            {
                if (Vector2.Distance(enemy.transform.position, transform.position) < CheckRadius)
                    list.Add(enemy.gameObject);    
            }
        }

        if (list.Count > 0)
            return list.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).ToList();
        return list;
    }
}
