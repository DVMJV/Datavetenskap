using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokemonAttack : ScriptableObject
{
    public string attackName;
    public int damage;
    public int range;
    public Type type;
    public int id;
    public int cooldown;

    public enum SecondaryEffect
    {
        STUN,
        KNOCKBACK,
        BLEED,
        NONE
    }

    public abstract void Attack();
    public void ResetEvent()
    {
        id = 0;
       // EventHandler.current.onMoveSelected -= Attack;
    }
    
}
