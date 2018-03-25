using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorScript : MonoBehaviour {

    public GameObject selectee;
    public SelectionManager manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SelectionManager>();
        selectee = transform.parent.gameObject;
    }
    /*--Checks to make sure if the mouse is over this object, then sends it along to the selection manager.
     *  There is a similar script in the TileData script, specfically because OnMouseOver doesn't work with overlapping colliders--*/
    void OnMouseOver()
    {
        if (transform.parent.GetComponent<TileData>().isOccupied == false)
        {
            manager.hoverSelect = transform.parent.gameObject;
            manager.hoverSelectType = SelectionManager.SelectType.Tile;
        }
        if (transform.parent.GetComponent<TileData>().isOccupied == true)
        {
            manager.hoverSelect = transform.parent.GetComponent<TileData>().occupiedBy;
            manager.hoverSelectType = SelectionManager.SelectType.Unit;
        }
    }

    /*--Just to make sure nothing goes wrong with having the mouse not over something and it still being selectable.--*/
    void OnMouseExit()
    {
        manager.hoverSelect = null;
    }
}
