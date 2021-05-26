using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    int id;

    private bool allowedToEndTurn = true;

    private PokemonContainer selected;

    [SerializeField]
    GameObject attackDisplay;

    [SerializeField]
    GameObject attackDisplayContainer;

    [SerializeField]
    private List<PokemonContainer> pokemons = new List<PokemonContainer>();
    bool turn;
    // Start is called before the first frame update
    void Awake()
    {
        turn = true;
        EventHandler.current.onAllySelected += SetSelected;
        EventHandler.current.onTileSelected += MovePokemon;
        EventHandler.current.onTurnStart += TurnStart;
        EventHandler.current.onTurnReset += TurnEnd;
        EventHandler.current.onAllowedToEndTurn += AllowedToEndTurn;
        EventHandler.current.OnStart();
        EventHandler.current.onCreatePlayerPokemons += GetPokemons;
        EventHandler.current.onPlayerSpawnCells += SetCells;
        EventHandler.current.onPokemonDestroyed += RemovePokemon;
    }

    private void OnDestroy()
    {
        EventHandler.current.onAllySelected -= SetSelected;
        EventHandler.current.onTileSelected -= MovePokemon;
        EventHandler.current.onTurnStart -= TurnStart;
        EventHandler.current.onTurnReset -= TurnEnd;
        EventHandler.current.onAllowedToEndTurn -= AllowedToEndTurn;
        EventHandler.current.onCreatePlayerPokemons -= GetPokemons;
        EventHandler.current.onPlayerSpawnCells -= SetCells;
        EventHandler.current.onPokemonDestroyed -= RemovePokemon;
    }

    void GetPokemons(List<GameObject> playerPokemons)
    {
        foreach (GameObject g in playerPokemons)
        {
            pokemons.Add(g.GetComponent<PokemonContainer>());
        }
    }

    void RemovePokemon(PokemonContainer container)
    {
        if (pokemons.Contains(container))
            pokemons.Remove(container);
    }

    void SetCells(List<SquareCell> spawnCells)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            foreach (var cell in spawnCells)
            {
                if(cell.obstructed)
                    continue;
                bool canSpawnAtCell = false;

                for (SquareDirection d = SquareDirection.UP; d < SquareDirection.LEFT; d++)
                {
                    SquareCell neighbor = cell.GetNeighbor(d);
                    if (neighbor != null && !neighbor.obstructed)
                    {
                        canSpawnAtCell = true;
                        break;
                    }
                }

                if (canSpawnAtCell)
                {
                    pokemons[i].CurrentTile = cell;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
            EventHandler.current.AllySelected(null);
    }


    void TurnStart(int id)
    {
        if(this.id == id)
        {
            turn = true;

            Debug.Log("My turn");
        }
    }
    void TurnEnd(int id)
    {
        if(this.id == id)
        {
            StartCoroutine(WaitForEndTurn());
        }
    }

    IEnumerator WaitForEndTurn()
    {
        while (true)
        {
            if (allowedToEndTurn)
                break;

            yield return null;
        }
        turn = false;
        foreach (PokemonContainer pokemon in pokemons)
        {
            pokemon.EndTurn();
        }
        ClearAttacks();
        selected = null;
        EventHandler.current.AllySelected(null);
        Debug.Log("My Reset");
    }

    private void SetSelected(PokemonContainer pokemon)
    {
        if (!turn) return;
        if (selected == pokemon) return;

        selected = pokemon;
        allowedToEndTurn = true;

        if (selected == null)
        {
            ClearAttacks();
            return;
        }
                
        DisplayAttacks(pokemon);
    }

    void MovePokemon(SquareCell selectedCell)
    {
        if (turn)
        {
            if(selected != null)
            {
                allowedToEndTurn = false;
                EventHandler.current.MovePokemon(selectedCell, selected);   
            }
        }
    }

    void AllowedToEndTurn()
    {
        allowedToEndTurn = true;
    }

    private void DisplayAttacks(PokemonContainer selectedPokemon)
    {
        ClearAttacks();
        for (int i = 0; i < selectedPokemon.learnedMoves.Count; i++)
        {
            String name = selectedPokemon.learnedMoves[i].GetName();
            GameObject go = Instantiate(attackDisplay, new Vector3(attackDisplayContainer.transform.position.x + 50, (attackDisplayContainer.transform.position.y - 30) + ((i * 80) + 1), 0), Quaternion.identity, attackDisplayContainer.transform);
            go.GetComponentInChildren<TMP_Text>().SetText(name);
            go.GetComponent<AttackDisplay>().SetAttackContainer(selectedPokemon.learnedMoves[i]);
            go.GetComponent<AttackDisplay>().id = i;
            
            
        }
    }

    private void ClearAttacks()
    {
        foreach (Transform child in attackDisplayContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
    