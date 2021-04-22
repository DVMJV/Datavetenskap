using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int id;

    private PokemonContainer selected;

    [SerializeField]
    private List<PokemonContainer> pokemons;
    bool turn;
    // Start is called before the first frame update
    void Start()
    {
        turn = true;
        EventHandler.current.onAllySelected += SetSelected;
        EventHandler.current.onTileSelected += MovePokemon;
        EventHandler.current.onTurnStart += TurnStart;
        EventHandler.current.onTurnReset += TurnEnd;
        EventHandler.current.OnStart();
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
            turn = false;
            foreach (PokemonContainer pokemon in pokemons)
            {
                pokemon.Reset();
            }
            Debug.Log("My Reset");
        }
    }
    void SetSelected(PokemonContainer pokemon)
    {
        if(turn)
        {
            selected = pokemon;
        }
    }

    void MovePokemon(SquareCell selectedCell)
    {
        if (turn)
        {
            EventHandler.current.MovePokemon(selectedCell, selected);   
        }
    }


}
