using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffects : MonoBehaviour {

    [System.Serializable]
    public class OnHitEffect
    {
        public enum effectName
        {
            Corrosive,

            None
        }
        public effectName name;
        public int effectTier;
        public void effectScript(UnitData.Attack attack, UnitData attacker, UnitData defender, TileData defenderTile)
        {
            switch (name)
            {
                case effectName.Corrosive:
                    {
                        Corrosive(attack, attacker, defender, defenderTile, effectTier);
                        break;
                    }
                case effectName.None:
                    {
                        Debug.Log(name);
                        break;
                    }
            }
        }
    }
    public List<OnHitEffects> onStrikeTraits = new List<OnHitEffects>();



    public void OnStrikeEffects(UnitData.Attack attack, UnitData attacker, UnitData defender, TileData defenderTile)
    {
        if(attack.onHitEffects.Count > 0)
        {
            foreach(OnHitEffect effect in attack.onHitEffects)
            {
                effect.effectScript(attack, attacker, defender, defenderTile);
            }
        }
    }


    private static void Corrosive(UnitData.Attack attack, UnitData attacker, UnitData defender, TileData defenderTile, int tierRating)
    {
        if(defender.armor > 0)
        {
            defender.armor -= tierRating;
        }
    }
}
