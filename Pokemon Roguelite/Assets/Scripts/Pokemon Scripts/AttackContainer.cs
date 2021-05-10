using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackContainer
{
    PokemonAttack attack;
    int cooldown;
    List<SquareCell> attackableCells;
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
    public void FindAttackableTiles(SquareCell fromCell)
    {
        attackableCells = new List<SquareCell>();
        if (attack is PokemonLineAttack)
            LineAttackSearch(fromCell, attack);
    }

    void LineAttackSearch(SquareCell fromCell, PokemonAttack attack)
    {
        for (SquareDirection direction = SquareDirection.UP; direction <= SquareDirection.LEFT; direction++)
        {
            int cost = 0;
            SquareCell neighbor = fromCell.GetNeighbor(direction);
            if (neighbor == null)
                continue;
            while (cost < attack.range)
            {
                if (neighbor == null)
                    break;
                attackableCells.Add(neighbor);
                neighbor.EnableHighlight(Color.black);
                neighbor = neighbor.GetNeighbor(direction);
                cost++;
            }
        }
    }

    public void Attack(SquareCell fromCell, SquareCell toCell, string tag)
    {
        if(attackableCells.Contains(toCell))
        {
            cooldown = attack.cooldown;
            attack.Attack(fromCell, toCell, tag);
        }
    }

    public string GetName()
    {
        return attack.name;
    }
}
