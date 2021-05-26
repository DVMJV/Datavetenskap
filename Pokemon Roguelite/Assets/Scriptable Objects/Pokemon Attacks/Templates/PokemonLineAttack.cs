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
        Destroy(go, 2f);

        var position = fromCell.transform.position;
        go.transform.position = position;
        Vector3 directionVector = toCell.transform.position - position;
        go.transform.forward = directionVector.normalized;
        List<SquareCell> cellsToAttack = ConstructAttackPath(fromCell, toCell);
        foreach (SquareCell cell in cellsToAttack)
            EventHandler.current.AttackTile(cell, this, tag);
    }

    private List<SquareCell> ConstructAttackPath(SquareCell fromCell, SquareCell toCell)
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
                if (neighbor == toCell)
                {
                    return cellsToAttack;
                }
                neighbor = neighbor.GetNeighbor(direction);
                cellsToAttack.Add(neighbor);
                cost++;
            }
            cellsToAttack.Clear();
        }

        return cellsToAttack;
    }

}