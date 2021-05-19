using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberManager : MonoBehaviour
{
    List<PokemonContainer> selectedParty = new List<PokemonContainer>();

    void Start()
    {
        EventHandler.current.onAddedPokemon += UpdateParty;
    }

    void UpdateParty(PokemonContainer pc)
    {
        selectedParty.Add(pc);
    }
}
