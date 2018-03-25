using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour{

    AttackButtonScript myButton;

	// Use this for initialization
	void Start () {
        myButton = GetComponentInParent<AttackButtonScript>();
        Debug.Log(myButton.gameObject.name);            
        GetComponentInChildren<Text>().text = myButton.attackerChoice.damage + "  -  " + myButton.attackerChoice.strikes + "         " + myButton.response.damage + "  -  " + myButton.response.strikes + "\n" + myButton.attackerChoice.damType + "       " + myButton.response.damType;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
