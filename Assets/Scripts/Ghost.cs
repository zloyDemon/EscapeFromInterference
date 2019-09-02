using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private static readonly float maxDistanceDelta = 1f;
    private static readonly float minDistanceToPoint = 1f;
    
    [SerializeField] GameObject[] patrolPoints;
    [SerializeField] Transform target;
    
    private CheckEnemy checkEnemy;
    private Vector2 currentPoint;
    private IState<Ghost> currentState;
    private Agent agent;
    private bool isTakingTargt;
    private Coroutine corTakingTarget;
    
    public Agent Agent { get { return agent; } }
    public GameObject[] PatrolPoints 
    { 
        get => patrolPoints;
        set => patrolPoints = value;
    }

    public Transform Target
    {
        get => target;
        set => target = value;
    }


    private void Awake()
    {
        checkEnemy = GetComponent<CheckEnemy>();
        agent = GetComponent<Agent>();
    }

    private void Start()
    {
        PathRequestManager.Instance.PfGrid.GridCreated += PfGridOnGridCreated;
    }

    private void OnDestroy()
    {
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
    
    private Vector2 GetNextPointPosition()
    {
        var newPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        return newPoint.transform.position;
    }

    public void ChangeState(IState<Ghost> newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public float DistanceToTarget()
    {
        var distance = Vector3.Distance(transform.position, Target.position);
        return distance;
    }
}