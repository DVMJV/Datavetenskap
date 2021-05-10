using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonSingleAttack", menuName = "ScriptableObjects/Attacks/SingleAttack", order = 2)]
public class PokemonSingleAttack : PokemonAttack
{
    public override void Attack(SquareCell fromCell, SquareCell toCell, string tag)
    {
    }
}
