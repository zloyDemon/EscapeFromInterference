using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private const float MoveDuration = 0.5f;
    private const float DistanceToPoint = 0.5f;
    
    private Path path;
    private Coroutine coFollowPath;
    private List<Vector2> currentPath;
    private Vector2 currentTargetPoint;
    private bool isMoving;
    private int pointIndex;
    
    public List<Vector2> CurrentPath { get { return currentPath; } }
    
    private void Start()
    {
        PathRequestManager.Instance.PfGrid.GridCreated += PfGridOnGridCreated;
    }

    private void OnDestroy()
    {
        PathRequestManager.Instance.PfGrid.GridCreated -= PfGridOnGridCreated;
    }

    public void FollowPath(Vector2 targetPoint)
    {
        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPoint, OnPathFound));
    }

    private void OnPathFound(Vector2[] pathPoints, bool isSuccess)
    {
        if (!isSuccess)
        {
            Debug.LogWarning("Agent: Path found not success");
            return;
        }

        StopFollow();
        
        path = new Path(pathPoints, transform.position, 3, 2);
        
        currentPath = new List<Vector2>(pathPoints);
        isMoving = true;
        Debug.Log("Path found: " + pathPoints.Length);
    }

    private void PfGridOnGridCreated()
    {
        
    }

    public void StopFollow()
    {
        isMoving = false;
        pointIndex = 0;
        if (currentPath != null)
        {
            currentPath.Clear();
            currentPath = null;
        }
            
    }

    private void Update()
    {
        if(isMoving)
            FollowingPath();
    }

    private void FollowingPath()
    {
        if (currentPath.Count == 0)
            return;

        var currentPoint = currentPath[pointIndex];
        transform.position = Vector2.MoveTowards(transform.position, currentPoint, MoveDuration * Time.deltaTime);
        if (Vector2.Distance(transform.position, currentPoint) <= DistanceToPoint)
            pointIndex++;
        
        if (currentPath.Count <= pointIndex)
            StopFollow();
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
