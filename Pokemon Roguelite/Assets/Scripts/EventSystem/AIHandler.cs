using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackType
{
    SingleTarget,
    LineAttack,
    StunAttack
}
public class AIHandler : MonoBehaviour
{
    [SerializeField]
    int id;

    [SerializeField]
    private List<PokemonContainer> pokemons;

    [SerializeField]
    int visionRange;

    [SerializeField]
    LayerMask pokemonLayer;

    List<PokemonContainer> VisiblePokemon;

    bool allowedToEndTurn = false;
    AttackType chosenAttack;
    void Start()
    {
        VisiblePokemon = new List<PokemonContainer>();
        EventHandler.current.onTurnStart += TurnStart;
        EventHandler.current.onAllowedToEndTurn += AllowedToEndTurn;
    }


    void TurnStart(int id)
    {
        if (this.id == id)
        {
            Debug.Log("AI turn");
            Turn();
        }
    }

    void Turn()
    {
        foreach (PokemonContainer pokemon in pokemons)
        {
            if(pokemon.IsStunned())
                continue;

            CheckForPokemons(pokemon);

            if(VisiblePokemon.Count > 0)
            {
                // Decide Attack Type 
                DecideAttack(pokemon);
                // Finns det kill pontetial 0
                // Finns det 2 i rad 1
                // Håller jag på att dö? 2

                // Decide Target

                // How to move

                // Attack

               
                MovePokemon(pokemon, RandomPath(pokemon));
                VisiblePokemon.Clear();
            }
            else
            {
                MovePokemon(pokemon, RandomPath(pokemon));
            }

        }
        StartCoroutine(EndTurn());
    }

    void DecideAttack(PokemonContainer selectedPokemon)
    {
        foreach (PokemonContainer pokemon in VisiblePokemon)
        {
            foreach (AttackContainer move in selectedPokemon.learnedMoves)
            {
                if (Distance(selectedPokemon.CurrentTile, pokemon.CurrentTile) > move.GetAttack().range + selectedPokemon.currentMovement)
                {
                    if (pokemon.CurrentHealth <= pokemon.CalculateDamage(move.GetAttack())){
                        if (move.GetAttack() is PokemonLineAttack)
                        {
                            bool onLine = LineAttackSearch(selectedPokemon.CurrentTile, move.GetAttack(), pokemon);
                            if (onLine)
                            {
                                move.FindAttackableTiles(pokemon.CurrentTile);
                                move.Attack(selectedPokemon.CurrentTile, pokemon.CurrentTile, selectedPokemon.gameObject.tag);
                            }
                            else
                            {
                                SquareCell moveTarget = FindDirection(selectedPokemon, pokemon, move.GetAttack());
                                Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
                                if(path == null)
                                {
                                    Debug.LogError("No possible Path");
                                    return;
                                }
                                MovePokemon(selectedPokemon, path);
                                move.FindAttackableTiles(pokemon.CurrentTile);
                                move.Attack(selectedPokemon.CurrentTile, pokemon.CurrentTile, selectedPokemon.gameObject.tag);
                            }
                        }
                        else
                        {
                            //move();
                            //attack();
                        }
                    }
                }
            }


            if (selectedPokemon.CurrentHealth < selectedPokemon.pokemon.health * 0.25f)
            {
                //if (inRange())
                //{
                //    //attack;
                //    // run();
                //}
                //else
                //{
                //    //run();
                //}
            }
            foreach (PokemonContainer pokemonContainer in VisiblePokemon)
            {
                List<PokemonContainer> xlist = VisiblePokemon.FindAll(pokemon => pokemon.CurrentTile.coordinates.X == pokemonContainer.CurrentTile.coordinates.X);

                List<PokemonContainer> ylist = VisiblePokemon.FindAll(pokemon => pokemon.CurrentTile.coordinates.Y == pokemonContainer.CurrentTile.coordinates.Y);

                if(xlist.Count > 0 || ylist.Count > 0)
                {
                    if (xlist.Count > ylist.Count)
                    {
                        // Move in X
                        // Attack
                    }
                    else
                    {
                        // Move in y
                        // Attack
                    }
                }
            }
        }
        //move;
        // attack Random;

    }

    Stack<SquareCell> CreatePath(PokemonContainer pokemon, SquareCell toCell)
    {
        SquareCell fromCell = pokemon.CurrentTile;
        int speed = pokemon.currentMovement;


        fromCell.Distance = 0;

        SquareCellPriorityQueue openSet = new SquareCellPriorityQueue();
        openSet.Enqueue(fromCell);

        while (openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current == toCell)
            {
                Stack<SquareCell> stack = new Stack<SquareCell>();
                while (toCell != pokemon.CurrentTile)
                {
                    stack.Push(toCell);
                    toCell = toCell.PathFrom;
                    return stack;
                }
                break;
            }

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 1)
                    continue;

                else if (neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    neighbor.PathFrom = current;
                    openSet.Enqueue(neighbor);
                }
                else if (current.Distance + 1 < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = current.Distance + 1;
                    neighbor.PathFrom = current;
                    openSet.Change(neighbor, oldPriority);
                }
            }
        }
        return null;
    }

    SquareCell FindDirection(PokemonContainer selectedPokemon, PokemonContainer pokemon, PokemonAttack attack)
    {
        List<SquareCell> possibleTiles = LineTileSearch(pokemon.CurrentTile, attack);

        // Maybe error check
        return possibleTiles.Find(cell => selectedPokemon.CurrentTile.coordinates.DistanceTo(cell.coordinates) <= selectedPokemon.currentMovement);
    }

    #region Attack

    float Distance(SquareCell fromCell, SquareCell toCell)
    {
        int dist = 0;
        Vector3 diff = fromCell.transform.position - toCell.transform.position;
        diff /= SquareMetrics.height;
        dist = Mathf.Abs((int)diff.x) + Mathf.Abs((int)diff.z);
        return dist;
    }
    bool LineAttackSearch(SquareCell fromCell, PokemonAttack attack, PokemonContainer target)
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
                if(target.CurrentTile == neighbor)
                {
                    return true;
                }
                neighbor = neighbor.GetNeighbor(direction);
                cost++;
            }

        }
            return false;
    }
    List<SquareCell> LineTileSearch(SquareCell fromCell, PokemonAttack attack)
    {
        List<SquareCell> possibleTiles = new List<SquareCell>();

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
                possibleTiles.Add(neighbor);
                neighbor = neighbor.GetNeighbor(direction);
                cost++;
            }

        }
        return possibleTiles;
    }
    #endregion
    Stack<SquareCell> RandomPath(PokemonContainer pokemon)
    {
        Stack<SquareCell> path = new Stack<SquareCell>();
        Stack<SquareCell> currentPath = new Stack<SquareCell>();
        SquareCell newCell = pokemon.CurrentTile;
        for (int i = 0; i < pokemon.currentMovement; i++)
        {
            int Direction = Random.Range(0, 4);
            SquareCell neighborCell = newCell.GetNeighbor((SquareDirection)Direction);
            while (neighborCell == null || Mathf.Abs(newCell.Elevation - neighborCell.Elevation) > 1)
            {
                Direction = Random.Range(0, 4);
                neighborCell = newCell.GetNeighbor((SquareDirection)Direction);
            }
            newCell = neighborCell;
            currentPath.Push(newCell);
        }
        while (currentPath.Count > 0)
        {
            path.Push(currentPath.Pop());
        }

        return path;
    }

    void MovePokemon(PokemonContainer pokemon, Stack<SquareCell> path)
    {
        allowedToEndTurn = false;
        EventHandler.current.PathFound(path, pokemon);
    }
    private void CheckForPokemons(PokemonContainer pokemon)
    {
        Collider[] pokemonColliders = Physics.OverlapSphere(pokemon.transform.position,visionRange,pokemonLayer);
        foreach (Collider pm in pokemonColliders)
        {
            PokemonContainer pokemonC = pm.GetComponentInParent<PokemonContainer>();
            if (pokemonC.CompareTag("Friendly"))
            {
                VisiblePokemon.Add(pokemonC);
            }
        }
    }

    IEnumerator EndTurn()
    {
        while (true)
        {
            yield return null;

            if(!allowedToEndTurn)
                yield return null;

            break;
        }
        Reset();
        EventHandler.current.EndTurn();
    }

    void AllowedToEndTurn()
    {
        allowedToEndTurn = true;
    }

    void Reset()
    {
        foreach (PokemonContainer pokemon in pokemons)
        {
            pokemon.EndTurn();
        }
        Debug.Log("AI Reset");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(pokemons[0].transform.position, visionRange);
    }

}
