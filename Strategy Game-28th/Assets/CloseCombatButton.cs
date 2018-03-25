using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseCombatButton : MonoBehaviour {

    GameObject buttonAttackMenu;
    Button btn;

    // Use this for initialization
    void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectOnClick);
        buttonAttackMenu = GameObject.FindGameObjectWithTag("AttackMenu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void SelectOnClick()
    {
        //Debug.Log("Should close");
        buttonAttackMenu.GetComponent<AttackMenuScript>().CloseMenu();
    }
}
