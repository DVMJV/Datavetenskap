using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberManager : MonoBehaviour
{
    public static List<PokemonContainer> selectedParty = new List<PokemonContainer>();
    public static int maxPartySize = 3;

    void Start()
    {
        EventHandler.current.onAddedPokemon += AddToParty;
        EventHandler.current.onRemovedPokemon += RemoveFromParty;
    }

    void AddToParty(PokemonContainer pc)
    {
        selectedParty.Add(pc);

        foreach (PokemonContainer item in selectedParty)
        {
            Debug.Log(item.pokemon.name);
        }
    }

    void RemoveFromParty(PokemonContainer pc)
    {
        if (selectedParty.Contains(pc))
            selectedParty.Remove(pc);

        foreach (PokemonContainer item in selectedParty)
        {
            Debug.Log(item.pokemon.name);
        }
    }
}
