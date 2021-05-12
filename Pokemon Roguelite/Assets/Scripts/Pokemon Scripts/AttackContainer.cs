using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackContainer
{
    PokemonAttack attack;
    private int cooldown;
    List<SquareCell> attackableCells;
    public AttackContainer(PokemonAttack attack)
    {
        this.attack = attack;
    }
    public PokemonAttack GetAttack()
    {
        return attack;
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
        else if(attack is PokemonSingleAttack)
            SingleAttackSearch(fromCell, attack);
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
                neighbor = neighbor.GetNeighbor(direction);
                cost++;
            }
        }
    }

    private void SingleAttackSearch(SquareCell fromCell, PokemonAttack attack)
    {
        Queue<SquareCell> openSet = new Queue<SquareCell>();
        fromCell.Distance = 0;
        openSet.Enqueue(fromCell);

        while(openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current.Distance >= attack.range && current.Distance != int.MaxValue)
                continue;

            for(SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);

                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 1)
                    continue;
                else if(neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    if (neighbor.Distance > attack.range)
                        continue;
                    else
                    {
                        openSet.Enqueue(neighbor);
                        attackableCells.Add(neighbor);
                    }
                }
            }
        }
    }
    
    public void HighlightAttack()
    {
        foreach(SquareCell cell in attackableCells)
        {
            cell.EnableHighlight(Color.red);
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
