using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    AStarGrid battleGrid;

    public Transform seeker;
    public Transform target;
    public SelectionManager manager;

    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SelectionManager>();
        battleGrid = GetComponent<AStarGrid>();
    }

    void Update()
    {
        //Debug.Log("Super mega check");
        //if(seeker != null & target != null)
        //{
        //    FindPath(seeker.position, target.position);
        //}
    }

    public void FindPath(Vector3 startPoint, Vector3 targetPoint)
    {
        Node startNode = battleGrid.NodeFromLocation(startPoint);
        Node targetNode = battleGrid.NodeFromLocation(targetPoint);
        //Debug.Log("Finding Path" + startNode + targetNode);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in battleGrid.GetNeighbors(currentNode, 1, 0))
            {
                //Debug.DrawLine(currentNode.transform.position, neighbor.transform.position, Color.black);
                if(!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if( newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }

                }
            }

        }
    }

    void RetracePath(Node start, Node end)
    {
        Debug.Log("retrace path");
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        //Debug.Log("Path Found");

        battleGrid.path = path;
        battleGrid.GeneratePathMap();
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }

    public void ClearPath()
    {

    }
}
