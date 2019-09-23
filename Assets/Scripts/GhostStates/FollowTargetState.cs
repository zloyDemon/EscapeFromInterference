using System;
using UnityEngine;
using System.Collections;

public class FollowTargetState : IState<Ghost>
{
    private const float LostDistance = 60f;
    private Ghost currentGhost;

    public void Enter(Ghost ghost)
    {
        currentGhost = ghost;
        Debug.LogError("Follow state enter");
    }

    public void Update()
    {
        currentGhost.FollowToTarget(currentGhost.Target.transform.position);
        var distanceToTarget = currentGhost.DistanceToTarget();
        if (distanceToTarget > LostDistance)
            currentGhost.ChangeState(new PatrolState());
    }

    public void Exit()
    {
        currentGhost = null;   
    }
}
