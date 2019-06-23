using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private const float minPathUpdateTime = .2f;
	private const float pathUpdateMoveThreshold = .5f;
	
	public Transform target;
	private float speed = 5;
	public float turnDst = 1;
	public float turnSpeed = 5;
	public float stoppingDist = 10f;
	private Path path;

	private void Awake()
	{
		PathRequestManager.Instance.PfGrid.GridCreated += InstanceOnLevelLoaded;
	}

	

	private void Start()
	{
		
	}

	private void OnDestroy()
	{
		PathRequestManager.Instance.PfGrid.GridCreated -= InstanceOnLevelLoaded;
	}

	private void InstanceOnLevelLoaded()
	{
		Debug.Log("Level loaded Unit");
		StartCoroutine(UpdatePath());
	}

	public void OnPathFound(Vector2[] wayPoints, bool pathSuccessful)
	{
		Debug.Log("Path founded: " + wayPoints.Length + " " + pathSuccessful);
		if (pathSuccessful)
		{
			
			path = new Path(wayPoints, transform.position,turnDst, stoppingDist);
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath()
	{
		if (Time.timeSinceLevelLoad < .3f)
		{
			yield return  new WaitForSeconds(.3f);
		}
		PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
		
		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;
		while (true)
		{
			yield return  new WaitForSeconds(minPathUpdateTime);
			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
			{
				PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
				targetPosOld = target.position;
			}			
		}
	}
	
	IEnumerator FollowPath()
	{
		bool folowingPath = true;
		int pathIndex = 0;
		
		float speedPercent = 1;
		while (folowingPath)
		{
			Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
			{
				if (pathIndex == path.finishLineIndex)
				{
					folowingPath = false;
					break;
				}
				else
				{
					pathIndex++;
				}
			}

			if (folowingPath)
			{
				if (pathIndex >= path.slowDownIndex && stoppingDist > 0)
				{
					speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDist);
					if (speedPercent < 0.01f)
					{
						folowingPath = false;
					}
				}

				transform.Translate(Vector2.up * speed * speedPercent * Time.deltaTime);
				Debug.DrawLine(transform.position, path.lookPoints[pathIndex], Color.red);
			}
			yield return null;
		}
	}

	private void OnDrawGizmos()
	{
		if (path != null)
		{
			path.DrawWithGizmos();
		}
	}
}
