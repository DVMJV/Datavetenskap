using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour
{
    public Image portrait;
    public PokemonContainer pokemonContainer;


    


    public void SelectMainAlly(PokemonContainer pokeCont)
    {
        pokemonContainer = pokeCont;
        //portrait.sprite = pokeCont.sprite;
        portrait.enabled=true;
    }

    public void DeselectAlly()
    {
        pokemonContainer = null;
        //portrait.sprite = null;
        portrait.enabled = false;
    }

}
