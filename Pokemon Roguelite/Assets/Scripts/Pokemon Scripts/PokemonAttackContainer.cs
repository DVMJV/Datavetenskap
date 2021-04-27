using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonAttackContainer : IPointerDownHandler
{
    PokemonAttack attack;
    PokemonContainer pokemon;

    public PokemonAttackContainer(PokemonAttack attack, PokemonContainer pokemon)
    {
        this.attack = attack;
        this.pokemon = pokemon;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventHandler.current.MoveSelected(pokemon, attack);
    }

    public void Display(GameObject prefab, Transform parent, int pos)
    {
        prefab.GetComponentInChildren<Text>().text = attack.name;
        GameObject.Instantiate(prefab, new Vector3(pos * 100, 50, 0), Quaternion.identity, parent);
        attack.id = pos;
    }
}
