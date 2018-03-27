using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : AttackManager {

    public enum Trait
    {
        Defender
    }

    public bool canAct = false;
    public bool canAttack = false;
    public bool canMove = false;

    public OnHitEffects onHitManager;
    public AttackMenuScript unitAttackMenu;
    public SelectionManager selector;
    public Pathfinding.Direction myFacing;
    public int faction;
    public int maxHP;
    public int currHP;
    public int maxMP;
    public int currMP;
    public int attacksRemaining;
    public bool blocker;
    public int armor;
    public TileData occupiedTile;
    private TileData.TileType terrainModifier1;
    private TileData.TileType terrainModifier2;
    private TileData.TileType terrainModifier3;
    public List<Attack> attacksInRange;
    public List<Trait> unitTraits;
    public GameObject aStar;
    private Attack response;

    Node[,] battleGrid;

    [Header("Attacks")]
    public List<Attack> unitAttacks = new List<Attack>();

	// Use this for initialization
	void Start () {
        myFacing = Pathfinding.Direction.South;
        currHP = maxHP;
        currMP = maxMP;
        aStar = GameObject.FindGameObjectWithTag("A*");
        battleGrid = aStar.GetComponent<AStarGrid>().battleGrid;
        onHitManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OnHitEffects>();
        selector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SelectionManager>();
        unitAttackMenu = GameObject.FindGameObjectWithTag("AttackMenu").GetComponent<AttackMenuScript>();
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(currHP <= 0)
        {
            Debug.Log("Unit Destroyed: " + gameObject.name);
        }
        if (canMove || canAttack)
        {
            canAct = true;
        }
    }
    void Update()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.GetComponent<TileData>() != null)
        {
            occupiedTile = other.gameObject.GetComponent<TileData>();
            other.gameObject.GetComponent<TileData>().isOccupied = true;
            other.gameObject.GetComponent<TileData>().occupiedBy = this.gameObject;
            other.gameObject.GetComponent<TileData>().isOccupiedByBlocker = blocker;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<TileData>() != null)
        {
            other.gameObject.GetComponent<TileData>().isOccupied = false;
            other.gameObject.GetComponent<TileData>().isOccupiedByBlocker = false;
        }
    }

    [System.Serializable]
    public class Attack
    {
        public enum AttackType
        {
            Melee,
            Ranged,
            None
        }
        public enum damageType
        {
            Slashing,
            Piercing,
            Impact,
            Magic,
            None
        }
        public string name;
        public int damage;
        public int strikes;
        public int range;
        public int minRange;
        public damageType damType;
        public AttackType attType;
        public List<OnHitEffects.OnHitEffect> onHitEffects;
        public Sprite attSprite;
    }

    public void StartOfTurn()
    {
        //Debug.Log(this.gameObject.name);
        currMP = maxMP;
        if(unitAttacks.Count > 1)
        {
            canAttack = true;

        }        
        canMove = true;
    }
    public void EndOfTurn()
    {
        Debug.Log(this.gameObject.name);
        canAct = false;
        canAttack = false;
        canMove = false;
    }

    void OnMouseOver()
    {
        selector.hoverSelect = this.gameObject;
        selector.hoverSelectType = SelectionManager.SelectType.Unit;
    }

    public void InitiateMovement(Pathfinding.Direction moveDir)
    {
        switch (moveDir)
        {
            case Pathfinding.Direction.North:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.NorthEast:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.NorthWest:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x - 2), Mathf.RoundToInt(transform.position.y)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.West:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x - 2), Mathf.RoundToInt(transform.position.y - 1)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.East:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.South:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y - 2)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.SouthEast:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 2)].transform.position;
                    myFacing = moveDir;
                    break;
                }
            case Pathfinding.Direction.SouthWest:
                {
                    //Debug.Log("Moved " + moveDir);
                    transform.position = battleGrid[Mathf.RoundToInt(transform.position.x - 2), Mathf.RoundToInt(transform.position.y - 2)].transform.position;
                    myFacing = moveDir;
                    break;
                }
        }
    }

    public List<GameObject> FindEnemiesInRange()
    {
        List<GameObject> possibleTargetsList = new List<GameObject>();
        foreach (Attack attack in unitAttacks)
        {
            foreach (Node node in aStar.GetComponent<AStarGrid>().GetNeighbors(this.occupiedTile.GetComponent<Node>(), attack.range, attack.minRange))
            {
                if(node.gameObject.GetComponent<TileData>().isOccupied == true)
                {
                    if (node.gameObject.GetComponent<TileData>().occupiedBy.GetComponent<UnitData>().faction != faction)
                    {
                        possibleTargetsList.Add(node.gameObject.GetComponent<TileData>().occupiedBy);
                    }
                }
            }
        }
        return possibleTargetsList;
    }

    public void InitiateAttack(GameObject target)
    {
        //Debug.Log(this.gameObject.name + " Initiated Attack Against " + target);
        unitAttackMenu.MenuStartup(this, target.GetComponent<UnitData>());
    }
    public override void Strike(GameObject strikeAttacker, GameObject strikeDefender, Attack attack)
    {
        base.Strike(strikeAttacker, strikeDefender, attack);
        onHitManager.OnStrikeEffects(attack, strikeAttacker.GetComponent<UnitData>(), strikeDefender.GetComponent<UnitData>(), strikeDefender.GetComponent<UnitData>().occupiedTile);
    }

    public void attackRangeFinder(GameObject target)
    {
        attacksInRange = new List<Attack>();
        foreach (Attack attack in unitAttacks)
        {
            //Debug.Log(attack.name);
            foreach (Node node in aStar.GetComponent<AStarGrid>().GetNeighbors(this.occupiedTile.GetComponent<Node>(), attack.range, attack.minRange))
            {
                if (node.gameObject.GetComponent<TileData>().isOccupied == true)
                {
                    if (node.gameObject.GetComponent<TileData>().occupiedBy == target)
                    {
                        attacksInRange.Add(attack);

                    }
                }
            }
        }
        if (attacksInRange.Count > 1)
        {
            canAttack = true;
        }
        if (attacksInRange.Count == 0)
        {
            canAttack = false;
        }
    }






    public Attack attackResponse(Attack.AttackType incType, UnitData attacker)
    {
        List<Attack> attackChoices = new List<Attack>();
        attackRangeFinder(attacker.gameObject);
        foreach (Attack attack in attacksInRange)
        {
            //Debug.Log(attack.name);
            if (attack.attType == incType)
            {
                attackChoices.Add(attack);
            }
            if (attack.attType != incType)
            {
                if(incType == Attack.AttackType.Ranged && attack.attType == Attack.AttackType.Melee)
                {
                    attackChoices.Add(attack);
                }
            }
        }
        int highestDamage = 0;
        foreach (Attack attack in attackChoices)
        {            
            int totalDamage;
            totalDamage = attack.damage * attack.strikes;
            if(totalDamage > highestDamage)
            {
                highestDamage = totalDamage;
            }
        }
        foreach (Attack attack in attackChoices)
        {
            int totalDamage;
            totalDamage = attack.damage * attack.strikes;
            if(totalDamage == highestDamage)
            {
                response = attack;
            }
        }
        Debug.Log(response.name);
        return response;
    }
}
