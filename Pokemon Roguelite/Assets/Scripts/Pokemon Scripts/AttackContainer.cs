using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackContainer
{
    PokemonAttack attack;
    int cooldown;

    public AttackContainer(PokemonAttack attack)
    {
        this.attack = attack;
    }

    public void LowerCooldown()
    {
        if (cooldown == 0)
            return;
        else
        {
            cooldown--;
        }
    }

    public void FindAttackableTiles(SquareCell startCell)
    {
        EventHandler.current.FindAttackableTiles(startCell, attack);
    }

    public void Attack()
    {
        cooldown = attack.cooldown;
    }

    public string GetName()
    {
        return attack.name;
    }
}
