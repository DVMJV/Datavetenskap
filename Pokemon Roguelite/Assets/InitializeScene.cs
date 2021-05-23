using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeScene : MonoBehaviour
{
    List<GameObject> pokemons;

    void Start()
    {
        InstantiatePokemons();
        EventHandler.current.CreatePlayerPokemons(pokemons);
    }

    void InstantiatePokemons()
    {
        for (int i = 0; i < PartyMemberManager.selectedParty.Count; i++)
        {
            GameObject go = Instantiate(PartyMemberManager.selectedParty[i]);
            pokemons.Add(go);
        }
    }
}
