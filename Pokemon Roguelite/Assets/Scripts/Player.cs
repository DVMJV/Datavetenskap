using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private PokemonContainer selected;
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.current.onAllySelected += SetSelected;
        EventHandler.current.onTileSelected += MovePokemon;
    }


    void SetSelected(PokemonContainer pokemon)
    {
        selected = pokemon;
    }

    void MovePokemon(SquareCell selectedCell)
    {
        EventHandler.current.MovePokemon(selectedCell, selected);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
