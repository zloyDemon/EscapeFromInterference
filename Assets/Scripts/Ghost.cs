﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private static readonly float maxDistanceDelta = 1f;
    private static readonly float minDistanceToPoint = 1f;
    
    [SerializeField] GameObject[] patrolPoints;
    [SerializeField] Transform target;
    
    public Agent Agent { get { return agent; } }
    public GameObject[] PatrolPoints 
    { 
        get { return patrolPoints; }
        set { patrolPoints = value; }
    }

    public Transform Target
    {
        get { return target; } 
        set { target = value; }
    }

    private CheckEnemy checkEnemy;
    private Vector2 currentPoint;
    private IState<Ghost> currentState;
    private Agent agent;

    private void Awake()
    {
        checkEnemy = GetComponent<CheckEnemy>();
        checkEnemy.EnemyChecked += EnemyChecked;
        agent = GetComponent<Agent>();
    }

    private void Start()
    {
        PathRequestManager.Instance.PfGrid.GridCreated += PfGridOnGridCreated;
    }

    private void OnDestroy()
    {
        checkEnemy.EnemyChecked -= EnemyChecked;
        PathRequestManager.Instance.PfGrid.GridCreated -= PfGridOnGridCreated;
    }

    private void Update()
    {
        if(currentState != null)
            currentState.Update();
    }

    public void FollowToTarget(Vector2 target)
    {
        Agent.FollowPath(target);
    }

    public void StopFollow()
    {
        Agent.StopFollow();
    }

    private void PfGridOnGridCreated()
    {
        ChangeState(new PatrolState());
    }

    private void EnemyChecked(GameObject[] obj)
    {
        
    }
    
    private Vector2 GetNextPointPosition()
    {
        var newPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        return newPoint.transform.position;
    }

    public void ChangeState(IState<Ghost> newState)
    {
        if(currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    private void MoveToCurrentPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint, maxDistanceDelta * Time.deltaTime);
        if (Vector2.Distance(transform.position, currentPoint) < minDistanceToPoint)
            currentPoint = GetNextPointPosition();
    }
}
