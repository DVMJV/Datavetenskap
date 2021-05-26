using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIHandler : MonoBehaviour
{
    [SerializeField]
    int id;

    private List<PokemonContainer> pokemons;

    [SerializeField]
    int visionRange;

    [SerializeField]
    LayerMask pokemonLayer;

    List<PokemonContainer> VisiblePokemon;

    bool allowedToEndTurn = true;
    private bool allowedToAttack = false; 
    private int movingPokemon;
    void Awake()
    {
        pokemons = new List<PokemonContainer>();
        VisiblePokemon = new List<PokemonContainer>();
        EventHandler.current.onTurnStart += TurnStart;
        EventHandler.current.onAllowedToEndTurn += AllowedToEndTurn;
        EventHandler.current.onAISpawned += AddPokemon;
    }

    void AddPokemon(PokemonContainer pokemonContainer)
    {
        pokemons.Add(pokemonContainer);
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
        allowedToEndTurn = true;
        foreach (PokemonContainer pokemon in pokemons)
        {
            pokemon.currentMovement = pokemon.pokemon.movementSpeed;
            VisiblePokemon.Clear();
            if(pokemon.IsStunned())
                continue;
            
            CheckForPokemons(pokemon);

            if(VisiblePokemon.Count > 0)
            {
                if (!ChooseAttack(pokemon))
                {
                    if (!RandomAttack(pokemon))
                    {
                        MovePokemon(pokemon, RandomPath(pokemon));
                    }
                }
                VisiblePokemon.Clear();
            }
            else
            {
                MovePokemon(pokemon, RandomPath(pokemon));
            }

        }
        StartCoroutine(EndTurn());
    }

    bool RandomAttack(PokemonContainer pokemon)
    {
        AttackContainer randomMove = pokemon.learnedMoves[Random.Range(0,pokemon.learnedMoves.Count)];

        if (randomMove != null && randomMove.OnCooldown())
        {
            PokemonContainer target = VisiblePokemon[Random.Range(0, VisiblePokemon.Count)];
            float dist = Distance(pokemon.CurrentTile, target.CurrentTile);
            if (dist <= randomMove.GetAttack().range)
            {
                randomMove.FindAttackableTiles(pokemon.CurrentTile);
                randomMove.Attack(pokemon.CurrentTile, target.CurrentTile, pokemon.tag);
                return true;
            }
            else
            {
                Stack<SquareCell> path = CreatePath(pokemon, target.CurrentTile);
                allowedToAttack = false;
                MovePokemon(pokemon, path);
                StartCoroutine(AttackWait(pokemon, target, randomMove));
                return true;

            }
        }
        return false;
    }

    bool ChooseAttack(PokemonContainer selectedPokemon)
    {
        foreach (PokemonContainer target in VisiblePokemon)
        {
            float dist = Distance(selectedPokemon.CurrentTile, target.CurrentTile);

            if (CanKill(selectedPokemon, dist, target))
                return true;

            bool lowHealth = selectedPokemon.CurrentHealth <= selectedPokemon.pokemon.health * 0.25f;
            if (lowHealth)
            {
                return RunAway(selectedPokemon, dist, target);
            }
            
            if (MultiHit(selectedPokemon, target))
                return true;
        }
        return false;
    }

    private bool MultiHit(PokemonContainer selectedPokemon, PokemonContainer target)
    {
        List<PokemonContainer> xList = VisiblePokemon.FindAll(pokemon =>
            pokemon.CurrentTile.coordinates.X == selectedPokemon.CurrentTile.coordinates.X);

        List<PokemonContainer> yList = VisiblePokemon.FindAll(pokemon =>
            pokemon.CurrentTile.coordinates.Y == selectedPokemon.CurrentTile.coordinates.Y);

        if (xList.Count <= 0 && yList.Count <= 0)
            return false;

        AttackContainer lineMove = selectedPokemon.learnedMoves.Find(x => x.GetAttack() is PokemonLineAttack);

        if (lineMove == null)
            return false;

        if (xList.Count > yList.Count)
        {
            SquareCell moveTarget = FindDirection(selectedPokemon, target, lineMove.GetAttack());
            Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
            if (path == null)
            {
                Debug.LogError("No possible Path");
                {
                    return true;
                }
            }

            allowedToAttack = false;
            MovePokemon(selectedPokemon, path);
            StartCoroutine(AttackWait(selectedPokemon, target, lineMove));
            {
                return true;
            }
        }
        else
        {
            SquareCell moveTarget = FindDirection(selectedPokemon, target, lineMove.GetAttack());
            Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
            if (path == null)
            {
                Debug.LogError("No possible Path");
                {
                    return true;
                }
            }

            allowedToAttack = false;
            MovePokemon(selectedPokemon, path);
            StartCoroutine(AttackWait(selectedPokemon, target, lineMove));
            {
                return true;
            }
        }
    }

    private bool RunAway(PokemonContainer selectedPokemon, float dist, PokemonContainer target)
    {
        AttackContainer stunMove = selectedPokemon.learnedMoves.Find(x =>
            x.GetAttack().effect == PokemonAttack.SecondaryEffect.Stun ||
            x.GetAttack().effect == PokemonAttack.SecondaryEffect.Knockback);

        if (stunMove != null && dist <= stunMove.GetAttack().range)
        {
            stunMove.Attack(selectedPokemon.CurrentTile, target.CurrentTile, selectedPokemon.tag);
            allowedToAttack = false;
            Stack<SquareCell> path = FleePath(selectedPokemon, target);
            MovePokemon(selectedPokemon, path);
            return true;
        }
        else
        {
            Stack<SquareCell> path = FleePath(selectedPokemon, target);
            allowedToAttack = false;
            MovePokemon(selectedPokemon, path);
            return true;
        }
    }

    private bool CanKill(PokemonContainer selectedPokemon, float dist, PokemonContainer target)
    {
        foreach (AttackContainer move in selectedPokemon.learnedMoves)
        {
            if (!(dist <= move.GetAttack().range + selectedPokemon.currentMovement))
                continue;
            if (!(target.CurrentHealth <= target.CalculateDamage(move.GetAttack())))
                continue;
            if (move.OnCooldown())
                continue;

            if (move.GetAttack() is PokemonLineAttack)
            {
                bool onLine = LineAttackSearch(selectedPokemon.CurrentTile, move.GetAttack(), target);
                if (onLine)
                {
                    move.FindAttackableTiles(target.CurrentTile);
                    move.Attack(selectedPokemon.CurrentTile, target.CurrentTile, selectedPokemon.tag);
                    {
                        return true;
                    }
                }
                else
                {
                    SquareCell moveTarget = FindDirection(selectedPokemon, target, move.GetAttack());
                    Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
                    if (path == null)
                    {
                        Debug.LogError("No possible Path");
                        {
                            return true;
                        }
                    }

                    allowedToAttack = false;
                    MovePokemon(selectedPokemon, path);
                    StartCoroutine(AttackWait(selectedPokemon, target, move));
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (dist <= move.GetAttack().range)
                {
                    move.FindAttackableTiles(selectedPokemon.CurrentTile);
                    move.Attack(selectedPokemon.CurrentTile, target.CurrentTile, selectedPokemon.tag);
                    {
                        return true;
                    }
                }
                else
                {
                    Stack<SquareCell> path = CreatePath(selectedPokemon, target.CurrentTile, false);
                    allowedToAttack = false;
                    MovePokemon(selectedPokemon, path);
                    StartCoroutine(AttackWait(selectedPokemon, target, move));
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    Stack<SquareCell> FleePath(PokemonContainer selectedPokemon, PokemonContainer pokemon)
    {
        Stack<SquareCell> path = new Stack<SquareCell>();
        Stack<SquareCell> reversePath = new Stack<SquareCell>();
        SquareCell nextCell = selectedPokemon.CurrentTile;
        float speed = selectedPokemon.currentMovement;
        while(speed > 0)
        {
            float dist = Distance(nextCell, pokemon.CurrentTile);
            for (int i = 0; i < 4; i++)
            {
                SquareCell neighbor = nextCell.GetNeighbor((SquareDirection)i);
                if (neighbor == null || Mathf.Abs(neighbor.Elevation - nextCell.Elevation) > 1)
                    continue;
                if(dist < Distance(neighbor, pokemon.CurrentTile))
                {
                    nextCell = neighbor;
                    reversePath.Push(nextCell);
                    speed--;
                    break;
                }
            }
        }
        while(reversePath.Count > 0)
        {
            path.Push(reversePath.Pop());
        }
        return path;
    }

    IEnumerator AttackWait(PokemonContainer selectedPokemon, PokemonContainer target, AttackContainer attack)
    {
        allowedToEndTurn = false;
        movingPokemon++;
        while (true)
        {
            yield return null;

            if (allowedToAttack)
                break;
        }

        movingPokemon--;
        attack.FindAttackableTiles(selectedPokemon.CurrentTile);
        attack.Attack(selectedPokemon.CurrentTile, target.CurrentTile, selectedPokemon.tag);
        
        if(movingPokemon == 0)
            allowedToEndTurn = true;
    }

    Stack<SquareCell> CreatePath(PokemonContainer pokemon, SquareCell toCell, bool notExludeToCell = true)
    {
        SquareCell fromCell = pokemon.CurrentTile;
        int speed = pokemon.currentMovement;

        Stack<SquareCell> reset = new Stack<SquareCell>();
        reset.Push(fromCell);

        while (reset.Count > 0)
        {
            SquareCell tileToReset = reset.Pop();
            tileToReset.Distance = int.MaxValue;
            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = tileToReset.GetNeighbor(d);
                if(neighbor != null && neighbor.Distance != int.MaxValue)
                    reset.Push(neighbor);
            }
        }

        fromCell.Distance = 0;


        SquareCellPriorityQueue openSet = new SquareCellPriorityQueue();
        openSet.Enqueue(fromCell);

        while (openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current == toCell)
            {
                toCell.obstructed = true;
                Stack<SquareCell> stack = new Stack<SquareCell>();
                while (toCell != pokemon.CurrentTile)
                {
                    if (notExludeToCell)
                    {
                        while (toCell != fromCell)
                        {
                            stack.Push(toCell);
                            toCell = toCell.PathFrom;
                        }
                         return stack;
                    }
                    else
                    {
                        while (toCell != fromCell)
                        {
                            toCell = toCell.PathFrom;
                            stack.Push(toCell);
                        }
                        return stack;
                    }
                }
                break;
            }

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 2  || neighbor.obstructed)
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
        return possibleTiles.Find(cell => selectedPokemon.CurrentTile.coordinates.DistanceTo(cell.coordinates) <= selectedPokemon.currentMovement && cell != pokemon.CurrentTile);
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
            if (neighbor == null || Mathf.Abs(neighbor.Elevation - fromCell.Elevation) > 1)
                continue;
            while (cost < attack.range)
            {
                if (neighbor == null || Mathf.Abs(neighbor.Elevation - fromCell.Elevation) > 1)
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
            if (neighbor == null || Mathf.Abs(neighbor.Elevation - fromCell.Elevation) > 1)
                continue;
            while (cost < attack.range)
            {
                if (neighbor == null || Mathf.Abs(neighbor.Elevation - fromCell.Elevation) > 1)
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
            while (neighborCell == null || Mathf.Abs(newCell.Elevation - neighborCell.Elevation) > 2 || neighborCell.obstructed)
            {
                Direction = Random.Range(0, 4);
                neighborCell = newCell.GetNeighbor((SquareDirection)Direction);
            }

            newCell = neighborCell;
            currentPath.Push(newCell);
        }

        SquareCell finalCell = currentPath.Peek();
        if (!finalCell.obstructed)
        {
            finalCell.obstructed = true;
            currentPath.Pop();
        }
        
        while (currentPath.Count > 0)
        {
            path.Push(currentPath.Pop());
        }

        return path;
    }

    void MovePokemon(PokemonContainer pokemon, Stack<SquareCell> path)
    {
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

            if(allowedToEndTurn)
                break;
        }
        Reset();
        EventHandler.current.EndTurn();
    }

    void AllowedToEndTurn()
    {
        allowedToAttack = true;
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
