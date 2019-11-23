using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState<Ghost>
{ 
    private const float PointDistance = 10f;
    private const float CheckTargetDistance = 50f;
    
    private Ghost currentGhost;
    private GameObject[] patrolPoints;
    private Vector2 currentPoint;
    
    public void Enter(Ghost ghost)
    {
        currentGhost = ghost;
        patrolPoints = ghost.PatrolPoints;
        currentPoint = GetNewPatrolPoint();
        currentGhost.FollowToTarget(currentPoint);
    }

    public void Update()
    {     
        if (Vector2.Distance(currentGhost.transform.position, currentPoint) <= PointDistance)
        {
            currentPoint = GetNewPatrolPoint();
            currentGhost.FollowToTarget(currentPoint);
        }
        
        CheckForNewState();
    }

    public void Exit()
    {
        currentGhost.StopFollow();
    }

    private Vector2 GetNewPatrolPoint()
    {
        var index = Random.Range(0, patrolPoints.Length);
        return patrolPoints[index].transform.position;
    }

    private void CheckForNewState()
    {
        if (Vector2.Distance(currentGhost.transform.position, currentGhost.Target.position) <= CheckTargetDistance)  
            currentGhost.ChangeState(new FollowTargetState()); 
    }
    
}
