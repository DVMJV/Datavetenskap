using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPortraitSelect : MonoBehaviour
{
    PokemonContainer pokemonContainer;
    Image image;
    

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void AddPokemon(PokemonContainer pc)
    {
        pokemonContainer = pc;
    }

    public void SelectPokemon()
    {
        EventHandler.current.AllySelected(pokemonContainer);
    }

    public void AddPortrait(Sprite portrait)
    {
        image.sprite = portrait;
    }
}
