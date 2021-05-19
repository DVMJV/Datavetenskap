using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Portrait : MonoBehaviour, IPointerDownHandler
{
    public PokemonContainer pokemonContainer;


    public void OnPointerDown(PointerEventData eventData)
    {
        EventHandler.current.AllySelected(pokemonContainer);

    }


}
