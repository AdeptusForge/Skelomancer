using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public int turnCheck = 0;

    public FactionManager factionManager;
    public List<FactionManager.Faction> factions;
    public List<FactionManager.Faction> possibleFactionTurn = new List<FactionManager.Faction>();

    public FactionManager.Faction playerFaction;
    public FactionManager.Faction factionWhosTurnItIs;

    void Start()
    {
        factions = factionManager.factions;
        factionWhosTurnItIs = factions[0];
        StartTurn(factionWhosTurnItIs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartTurn(factionWhosTurnItIs);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            EndTurn();
        }
    }



    void StartTurn(FactionManager.Faction factionTurn)
    {
        Debug.Log("turn started");
        foreach (GameObject unit in factionTurn.myUnits)
        {
            unit.GetComponent<UnitData>().StartOfTurn();
        }
    }




    public void EndTurn()
    {
        possibleFactionTurn = new List<FactionManager.Faction>();
        foreach(FactionManager.Faction faction in factions)
        {
            if(faction.isActive == true)
            {
                possibleFactionTurn.Add(faction);
            }
        }

        foreach (GameObject unit in factionWhosTurnItIs.myUnits)
        {
            unit.GetComponent<UnitData>().EndOfTurn();
        }
        turnCheck += 1;
        if (turnCheck >= possibleFactionTurn.Count)
        {
            Debug.Log(possibleFactionTurn.Count + "Turn Check above threshold" + turnCheck);
            turnCheck = 0;
        }
        if (turnCheck < possibleFactionTurn.Count)
        {
            Debug.Log(possibleFactionTurn.Count + "Turn Check below threshold" + turnCheck);
        }
        if(factions[turnCheck].isActive == true)
        {
            factionWhosTurnItIs = factions[turnCheck];
            StartTurn(factionWhosTurnItIs);
        }
        if(factions[turnCheck].isActive == false)
        {
            turnCheck += 1;
            factionWhosTurnItIs = factions[turnCheck];
            StartTurn(factionWhosTurnItIs);
        }

    }
}
