using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokemonAttackContainer : MonoBehaviour, IPointerDownHandler
{


    public int id;
    public void OnPointerDown(PointerEventData eventData)
    {
        EventHandler.current.MoveSelected(id);
    }


    
}
