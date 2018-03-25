using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {

    public enum TileType
    {
        Grass,
        TallGrass,
        Dirt,
        Water,
        Road,
        None
    }
    public SelectionManager manager;
    public TileType type1;
    public TileType type2;
    public int tileCost;
    public GameObject occupiedBy;
    public bool isOccupied;
    public bool isOccupiedByBlocker;
    public GameObject selector;

    public List<Wall> walls = new List<Wall>();


    public TileModifier tileModifier;


	// Use this for initialization
	void Start () {
        /*--Just for use with the selection manager--*/
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SelectionManager>();
        Instantiate(selector , this.transform);
    }

    // Update is called once per frame
    void Update () {
		
	}
    [System.Serializable]
    public class Wall
    {
        public enum WallDirection
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW            
        }
        public WallDirection wallDir;
        public int wallMod;
    }

    void OnMouseOver()
    {
        if (isOccupied == false)
        {
            manager.hoverSelect = this.gameObject;
            manager.hoverSelectType = SelectionManager.SelectType.Tile;
        }
        if (isOccupied == true)
        {
            manager.hoverSelect = occupiedBy;
            manager.hoverSelectType = SelectionManager.SelectType.Unit;
        }
    }

    [System.Serializable]
    public class TileModifier
    {
        int mpModifier;
    }
}
