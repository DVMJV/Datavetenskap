using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokemonAttackContainer : MonoBehaviour, IPointerDownHandler
{

    PokemonAttack attack;
    PokemonContainer pokemon;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventHandler.current.MoveSelected(pokemon, attack);
    }


    
}
