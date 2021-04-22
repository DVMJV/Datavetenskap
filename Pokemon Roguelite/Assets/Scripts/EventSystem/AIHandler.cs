using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    [SerializeField]
    int id;

    void Start()
    {
        EventHandler.current.onTurnStart += TurnStart;
    }
    void TurnStart(int id)
    {
        if (this.id == id)
        {
            Debug.Log("AI turn");
            Turn();
        }
    }

    void Turn()
    {
        //foreach (PokemonContainer pokemon in pokemons)
        //{
        //    // Decide Action for pokemon
        //    // Activate action
        //}

        Reset();
        EventHandler.current.EndTurn();
    }

    void Reset()
    {
        //foreach (PokemonContainer pokemon in pokemons)
        //{
        //    // Reset Pokemon Actions
        //}
        Debug.Log("AI Reset");
    }
}
