using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokemonAttack : ScriptableObject
{
    public string attackName;
    public int damage;
    public int range;
    public Type type;


    public abstract void Attack();
}
