using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    public static readonly float CheckRadius = 3f; 
    
    [SerializeField] private string enemyTag;

    public Action<GameObject> enemyChecked = g => { };

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, CheckRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                enemyChecked(collider.gameObject);
            }    
        }
    }
}
