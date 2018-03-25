using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {


    public enum SelectType
    {
        Unit,
        Tile,
        None
    }
    float timeMouse0Held = 0;
    float doubleClickTime = 0;
    
    public float singleClickCheck;
    public float doubleClickCheck;
    public float holdClickCheck;

    public GameObject hoverSelect;
    public SelectType hoverSelectType;
    public GameObject selectedObject1;
    public SelectType selected1Type = SelectType.None;
    public GameObject selectedObject2;
    public SelectType selected2Type = SelectType.None;

    public GameObject aStar;
    bool doubleClickActive;

    void Start()
    {
        aStar = GameObject.FindGameObjectWithTag("A*");
    }

    void Update()
    {
        /*--Tracks the amount of time the mouse is pressed. By checking how long it was held down for, we can determine if it was click selection or a click & hold selection.--*/
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Debug.Log(timeMouse0Held);
            if (doubleClickTime == 0f || doubleClickTime > doubleClickCheck)
            {
                //Debug.Log("merp");
                timeMouse0Held = timeMouse0Held + Time.deltaTime;
            }
            if (doubleClickTime > 0 && doubleClickTime <= doubleClickCheck)
            {
                //Debug.Log("derp");
                timeMouse0Held = 0f;
                timeMouse0Held = timeMouse0Held = timeMouse0Held + Time.deltaTime;
            }
        }
        /*--After the mouse button is let go, if it was not a click & hold selection, it will activate a check to see if a double click has occurred. If one doesn't, it'll just get reset.--*/
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Debug.Log(timeMouse0Held);
            if (timeMouse0Held >= singleClickCheck && timeMouse0Held < holdClickCheck)
            {
                SingleClickSelect();
            }

            if (timeMouse0Held >= singleClickCheck && timeMouse0Held > holdClickCheck)
            {
                ClickAndHoldSelect();
            }
            if (doubleClickTime > 0 && doubleClickTime <= doubleClickCheck && timeMouse0Held >= singleClickCheck && timeMouse0Held < holdClickCheck)
            {
                DoubleClickSelect();
            }
            timeMouse0Held = 0;
            doubleClickActive = true;           
        }
        if (doubleClickActive)
        {
            doubleClickTime = doubleClickTime + Time.deltaTime;
        }
        if (doubleClickTime > doubleClickCheck)
        {
            doubleClickActive = false;
            doubleClickTime = 0f;
        }
    }

    void SingleClickSelect()
    {
        bool movementOccurred = false;
        bool attackOccurred = false;
        //Debug.Log("Single Click Select");
        if (hoverSelect != null)
        {
            if (hoverSelectType == SelectType.Tile)
            {
                if (selectedObject1 == null)
                {
                    selectedObject1 = hoverSelect;
                    selected1Type = SelectType.Tile;
                }
                if (selectedObject1 != null)
                {
                    if (selected1Type == SelectType.Unit)
                    {
                        if(selectedObject1.GetComponent<UnitData>().faction == 1)
                        {
                            if (selectedObject2 != null)
                            {
                                if (selected2Type == SelectType.Tile)
                                {
                                    if (hoverSelect == selectedObject2)
                                    {
                                        if (CalculatePathCost(aStar.GetComponent<AStarGrid>().path) <= selectedObject1.GetComponent<UnitData>().currMP)
                                        {
                                            foreach (Pathfinding.Direction dir in aStar.GetComponent<AStarGrid>().pathMap)
                                            {
                                                selectedObject1.GetComponent<UnitData>().InitiateMovement(dir);
                                                selectedObject1.GetComponent<UnitData>().currMP -= selectedObject1.GetComponent<UnitData>().occupiedTile.tileCost;
                                            }
                                            //Debug.Log("wipe is occurring");
                                            movementOccurred = true;
                                            selectedObject2 = null;
                                            selected2Type = SelectType.None;
                                            aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                            aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                        }
                                        selectedObject2 = null;
                                        selected2Type = SelectType.None;
                                        aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                        aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                    }
                                    if (hoverSelect != selectedObject2 && selectedObject2 != null)
                                    {
                                        selectedObject2 = hoverSelect;
                                        selected2Type = SelectType.Tile;
                                        aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                        aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                        aStar.GetComponent<Pathfinding>().FindPath(selectedObject1.transform.position, selectedObject2.transform.position);
                                    }
                                }
                            }
                            if (selectedObject2 == null && movementOccurred == false)
                            {
                                Debug.Log(hoverSelect + "and " + selectedObject2);
                                selectedObject2 = hoverSelect;
                                selected2Type = SelectType.Tile;
                                aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                aStar.GetComponent<Pathfinding>().FindPath(selectedObject1.transform.position, selectedObject2.transform.position);
                            }                            
                        }
                    }
                    if (selected1Type == SelectType.Tile)
                    {
                        selectedObject1 = hoverSelect;
                    }
                }
            }
            if (hoverSelectType == SelectType.Unit)
            {
                if (selectedObject1 == null)
                {
                    selectedObject1 = hoverSelect;
                    selected1Type = SelectType.Unit;
                }
                if (selectedObject1 != null)
                {
                    if (selected1Type == SelectType.Unit)
                    {
                        if(selectedObject2 == null)
                        {
                            if (selectedObject1.GetComponent<UnitData>().faction == 1 && hoverSelect.GetComponent<UnitData>().faction != 1)
                            {
                                selectedObject2 = hoverSelect;
                                if (selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2) && selectedObject1.GetComponent<UnitData>().canAttack == true)
                                {
                                    selectedObject1.GetComponent<UnitData>().InitiateAttack(selectedObject2);
                                    attackOccurred = true;
                                }
                                if (!selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2))
                                {
                                    selectedObject1 = hoverSelect;
                                    selectedObject2 = null;
                                }
                            }
                            if (selectedObject1.GetComponent<UnitData>().faction == 1 && hoverSelect.GetComponent<UnitData>().faction == 1)
                            {
                                if(hoverSelect != selectedObject1)
                                {
                                    selectedObject1 = hoverSelect;
                                }
                            }
                        }
                        if (selectedObject2 != null)
                        {
                            if (selected2Type == SelectType.Tile && hoverSelect == selectedObject1)
                            {
                                selectedObject2 = null;
                                selected2Type = SelectType.None;
                                aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                            }
                            if(selected2Type == SelectType.Tile && hoverSelect != selectedObject1)
                            {
                                if(hoverSelectType == SelectType.Unit)
                                {
                                    if(hoverSelect.GetComponent<UnitData>().faction == selectedObject1.GetComponent<UnitData>().faction)
                                    {
                                        selectedObject1 = hoverSelect;
                                        selected1Type = SelectType.Unit;
                                        selectedObject2 = null;
                                        selected2Type = SelectType.None;
                                        aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                        aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                    }
                                    if (hoverSelect.GetComponent<UnitData>().faction != selectedObject1.GetComponent<UnitData>().faction)
                                    {
                                        Debug.Log("tile to enemy unit");
                                        selectedObject2 = hoverSelect;
                                        selected2Type = SelectType.Unit;
                                        aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                        aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                        Debug.Log(selectedObject2);

                                        if (selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2) && selectedObject1.GetComponent<UnitData>().canAttack == true)
                                        {
                                            selectedObject1.GetComponent<UnitData>().InitiateAttack(selectedObject2);
                                        }
                                        if (!selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2))
                                        {
                                            selectedObject1 = hoverSelect;
                                        }

                                    }
                                }
                                if (hoverSelectType == SelectType.Tile)
                                {
                                    selectedObject2 = hoverSelect;
                                    selected2Type = SelectType.Tile;
                                    aStar.GetComponent<AStarGrid>().path = new List<Node>();
                                    aStar.GetComponent<AStarGrid>().pathMap = new List<Pathfinding.Direction>();
                                }
                            }
                            if (selectedObject1.GetComponent<UnitData>().faction == 1 && selectedObject2.GetComponent<UnitData>().faction != 1)
                            {
                                if(hoverSelect.GetComponent<UnitData>().faction == 1)
                                {
                                    if (hoverSelect != selectedObject1)
                                    {
                                        selectedObject1 = hoverSelect;
                                        selectedObject2 = null;
                                        selected2Type = SelectType.None;
                                    }
                                    if (hoverSelect == selectedObject1)
                                    {
                                        selectedObject1 = selectedObject1.GetComponent<UnitData>().occupiedTile.gameObject;
                                        selectedObject2 = null;
                                        selected2Type = SelectType.None;
                                    }

                                }
                                if (hoverSelect.GetComponent<UnitData>().faction != 1 && attackOccurred == false)
                                {
                                    if (selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2) && selectedObject1.GetComponent<UnitData>().canAttack == true)
                                    {
                                        selectedObject1.GetComponent<UnitData>().InitiateAttack(selectedObject2);
                                    }
                                    if (!selectedObject1.GetComponent<UnitData>().FindEnemiesInRange().Contains(selectedObject2))
                                    {
                                        selectedObject1 = hoverSelect;
                                    }
                                }

                            }
                        }
                    }
                    if (selected1Type == SelectType.Tile)
                    {
                        selectedObject1 = hoverSelect;
                        selected1Type = SelectType.Unit;
                    }
                }
            }
        }
        movementOccurred = false;
    }

    void DoubleClickSelect()
    {
        //Debug.Log("Double Click Select");
        //if(selectedObject1.GetComponent<UnitData>() != null)
        //{
        //    selectedObject1 = selectedObject1.GetComponent<UnitData>().occupiedTile.gameObject;
        //    selected1Type = SelectType.Tile;
        //}
    }

    void ClickAndHoldSelect()
    {
        Debug.Log("Click And Hold Select");
    }

    int CalculatePathCost(List<Node> nodeList)
    {
        int pathCost = 0;
        for (var i = 0; i < nodeList.Count-1; i++)
        {
            pathCost += nodeList[i].gameObject.GetComponent<TileData>().tileCost;
        }
        return pathCost;
    }
}
