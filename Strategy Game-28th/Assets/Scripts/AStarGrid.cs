using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour {

    public LayerMask unTravelable;
    public LayerMask unWalkable;
    public Vector2 gridSize;
    public float nodeRadius;
    public Node[,] battleGrid;

    public List<Pathfinding.Direction> pathMap = new List<Pathfinding.Direction>();

    public Vector3 testLocation;
    public Node testNode;


    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        battleGrid = new Node[gridSizeX, gridSizeY];
    }

    public Node NodeFromLocation(Vector3 checkLocation)
    {
        int x = 0;
        int y = 0;

        if (checkLocation.x > 0f && checkLocation.x <= gridSizeX)
        {
            x = Mathf.RoundToInt(checkLocation.x);
        }
        if(checkLocation.x <= 0f || checkLocation.x > gridSizeX)
        {
            x = 1;
        }

        if (checkLocation.y > 0f && checkLocation.y <= gridSizeY)
        {
            y = Mathf.RoundToInt(checkLocation.y);
        }
        if(checkLocation.y <= 0f || checkLocation.y > gridSizeY)
        {
            y = 1;
        }

        return battleGrid[x-1,y-1];
    }

    /*----*/
    public List<Node> GetNeighbors(Node node, int checkDistance, int minimumDistance)
    {
        List<Node> neighbors = new List<Node>();

        if(checkDistance == minimumDistance)
        {
            Debug.Log("CheckDistance and MinimumDistance are equal, making GetNeighbors irrelevent");
        }
        /*--Checks all adjacent tiles by going through all of the nodes within 1 tile in the X and Y dimensions--*/
        if (node != null)
        {
            for (int x = -checkDistance; x <= checkDistance; x++)
            {
                for (int y = -checkDistance; y <= checkDistance; y++)
                {
                    /*--Specifically so that the target node isn't counted as occupied--*/
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    if (minimumDistance > 0)
                    {
                        if (x <= minimumDistance && x >= -minimumDistance)
                        {
                            if (y <= minimumDistance && y >= -minimumDistance)
                            {
                                continue;
                            }
                        }
                    }

                    int checkX = node.gridX + x - 1;
                    int checkY = node.gridY + y - 1;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbors.Add(battleGrid[checkX, checkY]);
                    }
                }
            }
        }
        return neighbors;
    }

    void Update()
    {
        testLocation = new Vector3(3,3,0);
        testNode = NodeFromLocation(testLocation);
    }

    public List<Node> path;
    public List<Pathfinding.Direction> GeneratePathMap()
    {
        foreach(Node node in path)
        {
            if(node.parent != null)
            {
                if (node.parent.transform.position.y < node.transform.position.y)
                {
                    if (node.parent.transform.position.x < node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.NorthEast);
                    }
                    if (node.parent.transform.position.x == node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.North);
                    }
                    if (node.parent.transform.position.x > node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.NorthWest);
                    }
                }
                if (node.parent.transform.position.y == node.transform.position.y)
                {
                    if (node.parent.transform.position.x < node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.East);
                    }
                    if (node.parent.transform.position.x == node.transform.position.x)
                    {
                        Debug.Log("Node is a parent of itself ERROR");
                    }
                    if (node.parent.transform.position.x > node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.West);
                    }
                }
                if (node.parent.transform.position.y > node.transform.position.y)
                {
                    if (node.parent.transform.position.x < node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.SouthEast);
                    }
                    if (node.parent.transform.position.x == node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.South);
                    }
                    if (node.parent.transform.position.x > node.transform.position.x)
                    {
                        pathMap.Add(Pathfinding.Direction.SouthWest);
                    }
                }
            }
        }
        return pathMap;
    }


    /*--Entirely for debugging purposes. Nodes that you can move to are WHITE, nodes that you can move through but not to are BLUE, nodes you cannot move through nor move to are RED--*/
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));

        if (battleGrid != null)
        {
            foreach (Node node in battleGrid)
            {

                Gizmos.color = (node.walkable && node.travelable) ? Color.white : (node.walkable && !node.travelable) ? Color.blue : Color.red;
                if (path != null)
                {
                    //Debug.Log("Path Exists");
                    if (path.Contains(node))
                    {
                        //Debug.Log("Node exists in path");
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.worldLocation, Vector3.one * (nodeDiameter - 0.1f));
            }


            /*--All Adjacent Nodes are shown as GREEN--*/
            //foreach (Node node in GetNeighbors(testNode,2,2))
            //{
            //    Gizmos.color = Color.green;
            //    Gizmos.DrawCube(node.worldLocation, Vector3.one * (nodeDiameter - 0.1f));
            //}
        }
    }
}
