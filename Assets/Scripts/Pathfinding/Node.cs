using UnityEngine;

namespace pathfinding
{
    public class Node : IHeapItem<Node>
    {
        public Vector2 worldPosition;
        public bool isWalkable;
        public int GCost;
        public int HCost;
        public int gridX;
        public int gridY;
        public Node parent;
        
        private int _heapIndex;

        public int FCost
        {
            get { return HCost + GCost; }
        }

        public Node(Vector2 position, bool _isWalkable, int _gridX, int _gridY)
        {
            worldPosition = position;
            isWalkable = _isWalkable;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
                compare = HCost.CompareTo(nodeToCompare.HCost);
            return -compare;
        }

        public int HeapIndex
        {
            get { return _heapIndex; }
            set { _heapIndex = value; }
        }
    }
}