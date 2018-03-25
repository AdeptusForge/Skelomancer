using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour {
   
    public List<Faction> factions;

    public List<GameObject> allUnits;

    [System.Serializable]
    public class Faction
    {
        public string factionName;
        public int factionNumber;
        public List<GameObject> myUnits;
        public bool isActive;
        public List<int> factionAllies;
        public List<int> factionEnemies;
    }


    // Use this for initialization
    void Awake () {
        allUnits = new List<GameObject>();
        
        foreach(GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            allUnits.Add(unit);
        }
        /*--Searches through all units and places them into their individual factions--*/
        foreach (GameObject unit in allUnits)
        {
            foreach (Faction team in factions)
            {
                if (unit.GetComponent<UnitData>().faction == team.factionNumber)
                {
                    team.myUnits.Add(unit);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
