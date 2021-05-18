using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    bool allowedToEndTurn = true;
    private bool allowedToAttack = false; 
    AttackType chosenAttack;
    private int movingPokemon;
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
                DecideAttack(pokemon);

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
        foreach (PokemonContainer p in VisiblePokemon)
        {
            foreach (AttackContainer m in selectedPokemon.learnedMoves)
            {
                if (!(Distance(selectedPokemon.CurrentTile, p.CurrentTile) <=
                      m.GetAttack().range + selectedPokemon.currentMovement)) 
                    continue;
                if (!(p.CurrentHealth <= p.CalculateDamage(m.GetAttack()))) 
                    continue;
                
                if (m.GetAttack() is PokemonLineAttack)
                {
                    bool onLine = LineAttackSearch(selectedPokemon.CurrentTile, m.GetAttack(), p);
                    if (onLine)
                    {
                        StartCoroutine(AttackWait(selectedPokemon, p, m));
                    }
                    else
                    {
                        SquareCell moveTarget = FindDirection(selectedPokemon, p, m.GetAttack());
                        Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
                        if(path == null)
                        {
                            Debug.LogError("No possible Path");
                            return;
                        }
                        MovePokemon(selectedPokemon, path);
                        StartCoroutine(AttackWait(selectedPokemon, p, m));
                    }
                }
                else
                {
                    float d = Distance(selectedPokemon.CurrentTile, p.CurrentTile);
                    if (d <= m.GetAttack().range)
                    {
                        StartCoroutine(AttackWait(selectedPokemon, p, m));
                    }
                    else
                    {
                        Stack<SquareCell> path = CreatePath(selectedPokemon, p.CurrentTile, false);
                        MovePokemon(selectedPokemon, path);
                        StartCoroutine(AttackWait(selectedPokemon, p, m));
                    }
                }
            }

            if (selectedPokemon.CurrentHealth < selectedPokemon.pokemon.health * 0.25f)
            {
                float d = Distance(selectedPokemon.CurrentTile, p.CurrentTile);
                AttackContainer move1 = selectedPokemon.learnedMoves.Find(x => 
                x.GetAttack().effect == PokemonAttack.SecondaryEffect.Stun ||
                x.GetAttack().effect == PokemonAttack.SecondaryEffect.Knockback);
                
                if (move1 != null && d <= move1.GetAttack().range)
                {
                    StartCoroutine(AttackWait(selectedPokemon, p, move1));
                    Stack<SquareCell> path = FleePath(selectedPokemon, p);
                    MovePokemon(selectedPokemon, path);
                }
                else
                {
                    Stack<SquareCell> path = FleePath(selectedPokemon, p);
                    MovePokemon(selectedPokemon, path);
                }
            }
            
            List<PokemonContainer> xlist = VisiblePokemon.FindAll(pokemon => pokemon.CurrentTile.coordinates.X == selectedPokemon.CurrentTile.coordinates.X);

            List<PokemonContainer> ylist = VisiblePokemon.FindAll(pokemon => pokemon.CurrentTile.coordinates.Y == selectedPokemon.CurrentTile.coordinates.Y);

            if (xlist.Count <= 0 && ylist.Count <= 0) 
                continue;
            
            AttackContainer move2 = selectedPokemon.learnedMoves.Find(x => x.GetAttack() is PokemonLineAttack);
            
            if (move2 == null) 
                continue;
            
            if (xlist.Count > ylist.Count)
            {
                SquareCell moveTarget = FindDirection(selectedPokemon, p, move2.GetAttack());
                Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
                if (path == null)
                {
                    Debug.LogError("No possible Path");
                    return;
                }
                MovePokemon(selectedPokemon, path);
                StartCoroutine(AttackWait(selectedPokemon, p, move2));
            }
            else
            {
                SquareCell moveTarget = FindDirection(selectedPokemon, p, move2.GetAttack());
                Stack<SquareCell> path = CreatePath(selectedPokemon, moveTarget);
                if (path == null)
                {
                    Debug.LogError("No possible Path");
                    return;
                }
                MovePokemon(selectedPokemon, path);
                StartCoroutine(AttackWait(selectedPokemon, p, move2));
            }
        }

        AttackContainer move3 = selectedPokemon.learnedMoves.Find(x => x.GetAttack() is PokemonSingleAttack);
        
        if (move3 == null)
            return;
        
        PokemonContainer pokemon = VisiblePokemon[Random.Range(0, VisiblePokemon.Count)];
        float dist = Distance(selectedPokemon.CurrentTile, pokemon.CurrentTile);
        if (dist <= move3.GetAttack().range)
        {
            StartCoroutine(AttackWait(selectedPokemon, pokemon, move3));
        }
        else
        {
            Stack<SquareCell> path = CreatePath(selectedPokemon, pokemon.CurrentTile);
            MovePokemon(selectedPokemon, path);
            StartCoroutine(AttackWait(selectedPokemon, pokemon, move3));
        }
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
        while (!allowedToAttack)
        {
            yield return null;
        }
        
        attack.FindAttackableTiles(target.CurrentTile);
        attack.Attack(selectedPokemon.CurrentTile, target.CurrentTile, selectedPokemon.tag);
        movingPokemon--;
        
        if(movingPokemon == 0)
            allowedToEndTurn = true;
    }

    Stack<SquareCell> CreatePath(PokemonContainer pokemon, SquareCell toCell, bool notExludeToCell = true)
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
                    if (notExludeToCell)
                    {
                        stack.Push(toCell);
                        toCell = toCell.PathFrom;
                        return stack;
                    }
                    else
                    {
                        toCell = toCell.PathFrom;
                        stack.Push(toCell);
                        return stack;
                    }
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
