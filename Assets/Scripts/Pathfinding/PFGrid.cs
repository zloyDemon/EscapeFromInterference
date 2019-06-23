using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

public class PFGrid : MonoBehaviour
{

    public bool displayGridGizmos;
    public LayerMask mask;
    public Vector2 worldGridSize;
    public float nodeRadius = 1f;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    private Node[,] nodeGrid;

    public event Action GridCreated = () => { };

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    private void Awake()
    {
        
        
    }

    private void OnDestroy()
    {
        DungeonManager.Instance.LevelLoaded -= LevelLoaded;
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += LevelLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LevelLoaded();
        }
    }

    private void LevelLoaded()
    {
        worldGridSize = DungeonManager.Instance.Size;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(worldGridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(worldGridSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        nodeGrid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBootomLeft = new Vector2(transform.position.x, transform.position.y) -
                                  Vector2.right * worldGridSize.x / 2 - Vector2.up * worldGridSize.y / 2;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 point = worldBootomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) +
                                Vector2.up * (y * nodeDiameter + nodeRadius);
                
                bool isWalkable = !Physics2D.OverlapCircle(point, nodeRadius - 0.1f, mask.value);
                nodeGrid[x, y] = new Node(point, isWalkable, x, y);
            }
        }

        GridCreated();
    }

    public List<Node> GetNeighboursNode(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                if(x == 0 && y == 0) continue;
                
                int X = node.gridX + x;
                int Y = node.gridY + y;
                if (X >= 0 && X < gridSizeX && Y >= 0 && Y < gridSizeY)
                {
                    neighbours.Add(nodeGrid[X,Y]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPosition(Vector2 position)
    {
        
        float perX = (position.x + worldGridSize.x / 2) / worldGridSize.x;
        float perY = (position.y + worldGridSize.y / 2) / worldGridSize.y;
        perX = Mathf.Clamp01(perX);
        perY = Mathf.Clamp01(perY);
        
        int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * perY);
        return nodeGrid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(worldGridSize.x, worldGridSize.y));
        if (nodeGrid != null && displayGridGizmos)
        {
            foreach (Node node in nodeGrid)
            {
                Gizmos.color = node.isWalkable ? Color.cyan : Color.red;
                Gizmos.DrawWireCube(node.worldPosition,new Vector2(nodeDiameter, nodeDiameter));
            }
        }
    }
}