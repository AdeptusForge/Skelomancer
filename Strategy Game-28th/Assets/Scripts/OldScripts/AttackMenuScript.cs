using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenuScript : MonoBehaviour {

    public List<UnitData.Attack> availableAttacks;
    public GameObject menuManager;
    public GameObject button;
    public GameObject panel;
    public List<GameObject> buttonSlots;
    public GameObject attackSprite;
    public GameObject defendSprite;

    void Start()
    {
        menuManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void MenuStartup(UnitData attacker, UnitData target)
    {
        //Debug.Log("Menu Startup Successful");
        availableAttacks = new List<UnitData.Attack>();
        attacker.GetComponent<UnitData>().attackRangeFinder(target.gameObject);
        availableAttacks = attacker.attacksInRange;
        panel.SetActive(true);
        attackSprite.GetComponent<Image>().sprite = attacker.GetComponent<SpriteRenderer>().sprite;
        defendSprite.GetComponent<Image>().sprite = target.GetComponent<SpriteRenderer>().sprite;
        GenerateMenu(attacker, target);
    }

    private void GenerateMenu(UnitData attacker, UnitData target)
    {
        if (availableAttacks.Count > 1)
        {
            for (int i = 1; i < availableAttacks.Count;  i++)
            {
                if(availableAttacks[i].name != "NoAttack")
                {
                    GameObject attackButton;
                    attackButton = Instantiate(button, buttonSlots[i-1].transform.position, Quaternion.identity, buttonSlots[i-1].gameObject.transform);
                    attackButton.name = "AttackButton_" + availableAttacks[i].name;
                    attackButton.GetComponent<AttackButtonScript>().attackerChoice = availableAttacks[i];
                    attackButton.GetComponent<AttackButtonScript>().response = target.attackResponse(availableAttacks[i].attType, attacker);
                }
            }
        }
    }

    public void CloseMenu()
    {
        menuManager.GetComponent<SelectionManager>().selectedObject1 = null;
        menuManager.GetComponent<SelectionManager>().selected1Type = SelectionManager.SelectType.None;
        menuManager.GetComponent<SelectionManager>().selectedObject2 = null;
        menuManager.GetComponent<SelectionManager>().selected2Type = SelectionManager.SelectType.None;
        panel.SetActive(false);
        foreach (GameObject buttonSlot in buttonSlots)
        {
            foreach (Transform child in buttonSlot.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
