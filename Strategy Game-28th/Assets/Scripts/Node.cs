using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{

    public bool walkable;
    public bool travelable;
    public Vector2 worldLocation;
    public GameObject myGridMatrix;
    public Node parent;

    public int gridX;
    public int gridY;

    public int mCost;
    public int gCost;
    public int hCost;

    public void Start()
    {
        walkable = true;
        worldLocation = this.gameObject.transform.position;
        myGridMatrix.GetComponent<AStarGrid>().battleGrid[Mathf.RoundToInt(transform.position.x)-1, Mathf.RoundToInt(transform.position.y)-1] = this;
        gridX = Mathf.RoundToInt(worldLocation.x);
        gridY = Mathf.RoundToInt(worldLocation.y);
    }


    public void Update()
    {
        mCost = gameObject.GetComponent<TileData>().tileCost;
        walkable = !gameObject.GetComponent<TileData>().isOccupiedByBlocker;
        travelable = !gameObject.GetComponent<TileData>().isOccupied;
    }

    public int fCost
    {
        get
        {
            return (gCost * mCost) + hCost;
        }
    }
}
