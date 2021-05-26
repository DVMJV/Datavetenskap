using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonSingleAttack", menuName = "ScriptableObjects/Attacks/SingleAttack", order = 2)]
public class PokemonSingleAttack : PokemonAttack
{
    public override void Attack(SquareCell fromCell, SquareCell toCell, string tag)
    {
        GameObject go = Instantiate(particle);
        Destroy(go, 2f);
        var position = fromCell.transform.position;
        go.transform.position = position;
        Vector3 directionVector = toCell.transform.position - position;
        go.transform.forward = directionVector.normalized;
        EventHandler.current.AttackTile(toCell, this, tag);
    }
}
