using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonAttack", menuName = "ScriptableObjects/PokemonAttacks", order = 2)]
public class PokemonAttack : ScriptableObject
{
    public string attackName;
    public int damage;
    public int range;

    public Type type;

}
