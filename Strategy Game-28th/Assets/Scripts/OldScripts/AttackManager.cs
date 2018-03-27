using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public List<UnitData.Attack> attackOptions;
    public List<UnitData.Attack> defendOptions;

    private GameObject attacker;
    private GameObject defender;

    private GameObject attackMenu;

    public void Combat(UnitData.Attack attackerAttack, UnitData.Attack defenderAttack, GameObject attacker, GameObject defender)
    {
        attacker.GetComponent<UnitData>().canAttack = false;
        attackMenu = GameObject.FindGameObjectWithTag("AttackMenu");
        attackMenu.GetComponent<AttackMenuScript>().CloseMenu();
        GetComponent<SelectionManager>().selectedObject1 = null;
        GetComponent<SelectionManager>().selected1Type = SelectionManager.SelectType.None;
        GetComponent<SelectionManager>().selectedObject2 = null;
        GetComponent<SelectionManager>().selected2Type = SelectionManager.SelectType.None;
        bool attackerTurn;
        int attackerStrikesRemaining;
        int defenderStrikesRemaining;
        attackerStrikesRemaining = attackerAttack.strikes;
        defenderStrikesRemaining = defenderAttack.strikes;

        if (defender.GetComponent<UnitData>().unitTraits.Contains(UnitData.Trait.Defender))
        {
            Debug.Log("Combat Started With Defender");
            attackerTurn = false;
            for (int i = 0; i < (attackerAttack.strikes + defenderAttack.strikes); i++)
            {
                if (attacker.GetComponent<UnitData>().currHP > 0 && defender.GetComponent<UnitData>().currHP > 0)
                {
                    if (attackerStrikesRemaining > 0 && defenderStrikesRemaining > 0)
                    {
                        if (attackerTurn == true)
                        {
                            attacker.GetComponent<UnitData>().Strike(attacker, defender, attackerAttack);
                            attackerStrikesRemaining -= 1;
                            attackerTurn = false;
                        }
                        if (attackerTurn == false)
                        {
                            defender.GetComponent<UnitData>().Strike(defender, attacker, defenderAttack);
                            defenderStrikesRemaining -= 1;
                            attackerTurn = true;
                        }
                    }
                    if (defenderStrikesRemaining > 0 && attackerStrikesRemaining == 0)
                    {
                        defender.GetComponent<UnitData>().Strike(defender, attacker, defenderAttack);
                        defenderStrikesRemaining -= 1;
                    }
                    if (attackerStrikesRemaining > 0 && defenderStrikesRemaining == 0)
                    {
                        attacker.GetComponent<UnitData>().Strike(attacker, defender, attackerAttack);
                        attackerStrikesRemaining -= 1;
                    }
                }
            }
        }

        if (!defender.GetComponent<UnitData>().unitTraits.Contains(UnitData.Trait.Defender))
        {
            Debug.Log("Combat Started Without Defender");
            attackerTurn = true;
            if (attacker.GetComponent<UnitData>().currHP > 0 && defender.GetComponent<UnitData>().currHP > 0)
            {
                for (int i = 0; i < (attackerAttack.strikes + defenderAttack.strikes); i++)
                {
                    if (attacker.GetComponent<UnitData>().currHP > 0 && defender.GetComponent<UnitData>().currHP > 0)
                    {
                        if (attackerStrikesRemaining > 0 && defenderStrikesRemaining > 0)
                        {
                            if (attackerTurn == true)
                            {
                                attacker.GetComponent<UnitData>().Strike(attacker, defender, attackerAttack);
                                attackerStrikesRemaining -= 1;
                                attackerTurn = false;
                            }
                            if (attackerTurn == false)
                            {
                                defender.GetComponent<UnitData>().Strike(defender, attacker, defenderAttack);
                                defenderStrikesRemaining -= 1;
                                attackerTurn = true;
                            }
                        }
                        if (defenderStrikesRemaining > 0 && attackerStrikesRemaining == 0)
                        {
                            defender.GetComponent<UnitData>().Strike(defender, attacker, defenderAttack);
                            defenderStrikesRemaining -= 1;
                        }
                        if (attackerStrikesRemaining > 0 && defenderStrikesRemaining == 0)
                        {
                            attacker.GetComponent<UnitData>().Strike(attacker, defender, attackerAttack);
                            attackerStrikesRemaining -= 1;
                        }
                    }
                }
            }
        }
    }


    public virtual void Strike(GameObject strikeAttacker, GameObject strikeDefender, UnitData.Attack attack)
    {
        //Debug.Log("strike occurred " + gameObject.name);
        if (strikeAttacker == this.gameObject)
        {
            if(strikeDefender.GetComponent<UnitData>().armor < attack.damage)
            {
                strikeDefender.GetComponent<UnitData>().currHP -= attack.damage - strikeDefender.GetComponent<UnitData>().armor;
            }
        }
        if (strikeDefender == this.gameObject)
        {
            if (strikeAttacker.GetComponent<UnitData>().armor < attack.damage)
            {
                strikeAttacker.GetComponent<UnitData>().currHP -= attack.damage - strikeAttacker.GetComponent<UnitData>().armor;
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
