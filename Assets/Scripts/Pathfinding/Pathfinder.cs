using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private PFGrid _pfGrid;

    private void Awake()
    {
        _pfGrid = GetComponent<PFGrid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector2[] wayPoints = new Vector2[0];
        bool pathSuccess = false;

        Node startNode = _pfGrid.NodeFromWorldPosition(request.pathStart);
        Node targetNode = _pfGrid.NodeFromWorldPosition(request.pathEnd);

        if (startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<Node> openSet = new Heap<Node>(_pfGrid.MaxSize);
            List<Node> closedSet = new List<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (var neighbour in _pfGrid.GetNeighboursNode(currentNode))
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour)) continue;

                    int newMovementToNeighB = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementToNeighB < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementToNeighB;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
            pathSuccess = wayPoints.Length > 0;
        }
        callback(new PathResult(wayPoints, pathSuccess, request.callback));
    }

    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

//        Vector2[] wayPoints = SimplifyPath(path);
        List<Vector2> list = new List<Vector2>();
        bool flag = true;
        foreach (var node in path)
        {
            if(flag)
                list.Add(node.worldPosition);
            flag = !flag;
        }

        var wayPoints = list.ToArray();
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                wayPoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return wayPoints.ToArray();
    }
    
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}