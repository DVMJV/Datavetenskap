using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour
{
    Image image;
    PokemonContainer pokemonContainer;
    private void Start()
    {
        image = GetComponent<Image>();
        EventHandler.current.onAllySelected += SetPortrait;
    }

    void SetPortrait(PokemonContainer pc)
    {
        pokemonContainer = pc;
        if(pc != null)
            image.sprite = pc.pokemon.sprite;
    }

}
