﻿using pathfinding;
using UnityEngine;

namespace Pathfinding
{
    public class Path
    {
        public readonly Vector2[] lookPoints;
        public readonly Line[] turnBoundaries;
        public readonly int finishLineIndex;
        public readonly int slowDownIndex;

        public Path(Vector2[] wayPoints, Vector2 startPos, float turnDst, float stoppingDst)
        {
            lookPoints = wayPoints;
            turnBoundaries = new Line[lookPoints.Length];
            finishLineIndex = turnBoundaries.Length - 1;

            Vector2 previousPoint = startPos;
            for (int i = 0; i < lookPoints.Length; i++)
            {
                Vector2 currentPoint = lookPoints[i];
                Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
                Vector2 turnBoundaryPoint =
                    (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
                turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
                previousPoint = turnBoundaryPoint;
            }

            float dstFromEndPoint = 0;
            for (int i = lookPoints.Length - 1; i > 0; i--)
            {
                dstFromEndPoint += Vector2.Distance(lookPoints[i], lookPoints[i - 1]);
                if (dstFromEndPoint > stoppingDst)
                {
                    slowDownIndex = i;
                    break;
                }
            }
        }

        public void DrawWithGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var p in lookPoints)
            {
                //Gizmos.DrawCube(p, Vector2.one * 0.23f*2f);
            }

            for (int i = 0; i < lookPoints.Length - 1; i++)
                Gizmos.DrawLine(lookPoints[i], lookPoints[i+1]);
            
            
            Gizmos.color = Color.white;
            foreach (var l in turnBoundaries)
            {
                l.DrawWithGizmos(5);
            }
        }
    }
}