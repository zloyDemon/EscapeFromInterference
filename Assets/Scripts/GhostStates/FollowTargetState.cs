using System;
using UnityEngine;
using System.Collections;

public class FollowTargetState : IState<Ghost>
{
    private const float LostDistance = 100f;
    private Ghost currentGhost;
    private float oldAgentSpeed;

    public void Enter(Ghost ghost)
    {
        currentGhost = ghost;
        Debug.LogError("Follow state enter");
        oldAgentSpeed = currentGhost.Agent.AgentMoveSpeed;
        currentGhost.Agent.AgentMoveSpeed = oldAgentSpeed +20f;
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
        currentGhost.Agent.AgentMoveSpeed = oldAgentSpeed;
        currentGhost = null;   
    }
}
