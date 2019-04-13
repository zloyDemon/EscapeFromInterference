using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    public static readonly float CheckRadius = 3f;

    public enum CheckTargetType
    {
        None,
        Player,
        Ghost,
    }
    
    [SerializeField] private string enemyTag;

    public Action<GameObject[]> EnemyChecked = g => { };
    public CheckTargetType TargetType = CheckTargetType.None;

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        if (TargetType == CheckTargetType.None) return;
        var colliders = Physics2D.OverlapCircleAll(transform.position, CheckRadius);
        var enemies = GetEnemiesArrayByTag(colliders);
        EnemyChecked(enemies.ToArray());    
    }

    private List<GameObject> GetEnemiesArrayByTag(Collider2D[] enemies)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var enemy in enemies)
        {
            if(enemy.CompareTag(TargetType.ToString()))
                list.Add(enemy.gameObject);
        }

        if (list.Count > 0)
            return list.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).ToList();
        return list;
    }
}
