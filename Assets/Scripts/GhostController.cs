using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private static readonly float maxDistanceDelta = 1f;
    private static readonly float minDistanceToPoint = 1f;
    private CheckEnemy checkEnemy;

    public Transform[] patrolPoints;
    private Vector2 currentPoint;

    private void Awake()
    {
        checkEnemy = GetComponent<CheckEnemy>();
        checkEnemy.EnemyChecked += EnemyChecked;
        currentPoint = patrolPoints[0].position;
    }

    private void OnDestroy()
    {
        checkEnemy.EnemyChecked -= EnemyChecked;
    }

    private void Update()
    {
        //MoveToCurrentPoint();
    }

    private Vector2 GetNextPointPosition()
    {
        var newPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        return newPoint.position;
    }

    private void EnemyChecked(GameObject[] obj)
    {
        
    }

    private void MoveToCurrentPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint, maxDistanceDelta * Time.deltaTime);
        if (Vector2.Distance(transform.position, currentPoint) < minDistanceToPoint)
            currentPoint = GetNextPointPosition();
    }
}
