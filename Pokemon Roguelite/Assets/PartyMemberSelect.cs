using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberSelect : MonoBehaviour
{
    [SerializeField] PokemonContainer slottedMember;


    public void UpdateSlot()
    {
        Debug.Log("Button CLicked");
        EventHandler.current.AddPokemon(slottedMember);
    }
}
