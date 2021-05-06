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

    public void Attack()
    {
        cooldown = attack.cooldown;
        attack.Attack();
    }

    public string GetName()
    {
        return attack.name;
    }
}
