using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberManager : MonoBehaviour
{
    public static List<GameObject> selectedParty = new List<GameObject>();
    public static int maxPartySize = 3;

    void Start()
    {
        EventHandler.current.onAddedPokemon += AddToParty;
        EventHandler.current.onRemovedPokemon += RemoveFromParty;
    }

    void AddToParty(GameObject go)
    {
        selectedParty.Add(go);
    }

    void RemoveFromParty(GameObject go)
    {
        if (selectedParty.Contains(go))
            selectedParty.Remove(go);
    }
}
