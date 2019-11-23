using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private static readonly float maxDistanceDelta = 1f;
    private static readonly float minDistanceToPoint = 1f;

    [SerializeField] Transform target;
    
    private CheckEnemy checkEnemy;
    private Vector2 currentPoint;
    private IState<Ghost> currentState;
    private Agent agent;
    private bool isTakingTarget;
    private Coroutine corTakingTarget;
    
    public Agent Agent { get { return agent; } }
    public GameObject[] PatrolPoints { get; set; }

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
        currentState?.Update();
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
        var newPoint = PatrolPoints[Random.Range(0, PatrolPoints.Length)];
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }
}