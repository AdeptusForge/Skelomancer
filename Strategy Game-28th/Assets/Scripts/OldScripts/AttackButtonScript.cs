using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonScript : MonoBehaviour {

    public UnitData.Attack attackerChoice;
    public UnitData.Attack response;
    private GameObject manager;
    Button btn;
    bool selected = false;
    private List<GameObject> allButtons;

	// Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectOnClick);
        allButtons = new List<GameObject>();
        foreach(GameObject buttonSlot in GameObject.FindGameObjectWithTag("AttackMenu").GetComponent<AttackMenuScript>().buttonSlots)
        {
            if(buttonSlot.transform.childCount > 0)
            {
                allButtons.Add(buttonSlot.transform.GetChild(0).gameObject);
            }
        }
    }
	
    void SelectOnClick()
    {
        if (selected == true)
        {
            Debug.Log(gameObject.name + " selection confirmed");
            manager.GetComponent<AttackManager>().Combat(attackerChoice, response, manager.GetComponent<SelectionManager>().selectedObject1, manager.GetComponent<SelectionManager>().selectedObject2);
        }
        if (selected == false)
        {
            foreach (GameObject button in allButtons)
            {
                if (button != this.gameObject)
                {
                    Debug.Log(button.gameObject.name + " deselected");
                    button.GetComponent<AttackButtonScript>().selected = false;
                }
            }
            selected = true;
            Debug.Log(gameObject.name + "Selected");
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
