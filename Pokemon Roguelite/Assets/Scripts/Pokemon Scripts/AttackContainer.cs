using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackContainer
{
    #region Variables

    private PokemonAttack attack;
    private int cooldown;
    private List<SquareCell> attackableCells;
    
    #endregion

    #region Constructor
    
    /// <summary>
    /// Creates a container for a specific attack that tracks individual cooldown. 
    /// </summary>
    /// <param name="attack"></param>
    public AttackContainer(PokemonAttack attack)
    {
        this.attack = attack;
    }

    #endregion

    #region Public
    
    /// <summary>
    /// Finds all attackable tiles
    /// </summary>
    /// <param name="fromCell"></param>
    public void FindAttackableTiles(SquareCell fromCell)
    {
        attackableCells = new List<SquareCell>();
        if (attack is PokemonLineAttack)
            LineAttackSearch(fromCell, attack);
        else if(attack is PokemonSingleAttack)
            SingleAttackSearch(fromCell, attack);
    }
    
    /// <summary>
    /// Highlights all attackable tiles
    /// </summary>
    public void HighlightAttack()
    {
        foreach(SquareCell cell in attackableCells)
        {
            cell.EnableHighlight(Color.red);
        }
    }
    
    /// <summary>
    /// Attacks a tile 
    /// </summary>
    /// <param name="fromCell"></param>
    /// <param name="toCell"></param>
    /// <param name="tag"></param>
    public bool Attack(SquareCell fromCell, SquareCell toCell, string tag)
    {
         if(attackableCells.Contains(toCell))
        {
            cooldown = attack.cooldown;
            attack.Attack(fromCell, toCell, tag);
            return true;
        }
         else
         {
             return false;
         }
    }
    
    /// <summary>
    /// Returns the attack name
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return attack.name;
    }
    
    /// <summary>
    /// Lowers the cooldown
    /// </summary>
    public void LowerCooldown()
    {
        if (cooldown == 0)
            return;
        else
        {
            cooldown--;
        }
    }
    public bool OnCooldown()
    {
        if(cooldown <= 0)
        {
            cooldown = 0;
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// Returns the attack
    /// </summary>
    /// <returns></returns>
    public PokemonAttack GetAttack()
    {
        return attack;
    }
    #endregion
    
    #region Private

    /// <summary>
    /// Searches for attackable tiles for line attacks
    /// </summary>
    /// <param name="fromCell"></param>
    /// <param name="attack"></param>
    private void LineAttackSearch(SquareCell fromCell, PokemonAttack attack)
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

    /// <summary>
    /// Searches for attackable tiles for single target attacks
    /// </summary>
    /// <param name="fromCell"></param>
    /// <param name="attack"></param>
    private void SingleAttackSearch(SquareCell fromCell, PokemonAttack attack)
    {
        Queue<SquareCell> openSet = new Queue<SquareCell>();
        Stack<SquareCell> reset = new Stack<SquareCell>();
        reset.Push(fromCell);

        while (reset.Count > 0)
        {
            SquareCell tileToReset = reset.Pop();
            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = tileToReset.GetNeighbor(d);
                if(neighbor != null && neighbor.Distance != int.MaxValue)
                    reset.Push(neighbor);
                tileToReset.Distance = int.MaxValue;
            }
        }
        
        
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

                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 2)
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

    #endregion
}
