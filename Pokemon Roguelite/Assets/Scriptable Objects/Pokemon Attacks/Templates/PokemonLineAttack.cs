using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PokemonLineAttack", menuName = "ScriptableObjects/Attacks/LineAttack", order = 3)]

public class PokemonLineAttack : PokemonAttack
{
    public override void Attack(SquareCell fromCell, SquareCell toCell, string tag)
    {
        GameObject go = Instantiate(particle);
        SquareDirection d;
        List<SquareCell> cellsToAttack = ConstructAttackPath(fromCell, toCell, out d);
        foreach (SquareCell cell in cellsToAttack)
            EventHandler.current.AttackTile(cell, this, tag);
    }

    private List<SquareCell> ConstructAttackPath(SquareCell fromCell, SquareCell toCell, out SquareDirection d)
    {
        List<SquareCell> cellsToAttack = new List<SquareCell>();

        for (SquareDirection direction = SquareDirection.UP; direction <= SquareDirection.LEFT; direction++)
        {
            int cost = 0;
            SquareCell neighbor = fromCell.GetNeighbor(direction);
            cellsToAttack.Add(neighbor);

            if (neighbor == null)
                continue;
            while (cost < range)
            {
                if (neighbor == null)
                    break;
                neighbor = neighbor.GetNeighbor(direction);
                cellsToAttack.Add(neighbor);
                if (neighbor == toCell)
                {
                    d = direction;
                    return cellsToAttack;
                }
                cost++;
            }
            cellsToAttack.Clear();
        }

        d = (SquareDirection)5;
        return cellsToAttack;
    }

}