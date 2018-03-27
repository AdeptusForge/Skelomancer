using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "BattleTile")]
public class BattleTile : Tile
{
    public BattleTileInfo info;
    public bool occupied;
    public Entity occupiedBy;



    public BattleTile(BattleTileInfo _info)
    {
        info = _info;

    }
}