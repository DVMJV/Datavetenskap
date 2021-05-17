using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            CheckForPokemons(pokemon);

            if(VisiblePokemon.Count > 0)
            {
                PokemonContainer target;
                foreach (AttackContainer attack in pokemon.learnedMoves)
                {
                    if(attack.GetAttack() is PokemonLineAttack)
                    {
                        target = LineAttackSearch(pokemon.CurrentTile, attack.GetAttack());
                        if(target != null)
                        {
                            attack.FindAttackableTiles(pokemon.CurrentTile);
                            attack.Attack(pokemon.CurrentTile, target.CurrentTile, pokemon.gameObject.tag);
                        }
                    }
                }
                // if(Within Range && HaslineAttack && VisiblePokemon == x/y)
                // LineAttack
                // Else
                // Move
                // LineAttack?
                // if(??)
                // Attack(pokemon)
                // else
                //MovePokemon(pokemon, SetPath(pokemon));
                VisiblePokemon.Clear();
            }
            else
            {
                MovePokemon(pokemon, RandomPath(pokemon));
            }

        }
        StartCoroutine(EndTurn());
    }

    #region Attack
    PokemonContainer LineAttackSearch(SquareCell fromCell, PokemonAttack attack)
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
                foreach(PokemonContainer pokemon in VisiblePokemon)
                {
                    if(pokemon.CurrentTile == neighbor)
                        return pokemon;
                }
                neighbor = neighbor.GetNeighbor(direction);
                cost++;
            }

        }
            return null;
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
