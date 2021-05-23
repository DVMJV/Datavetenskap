
using UnityEngine;

public class PortraitUI : MonoBehaviour
{
    public Transform portraitParent;
    //TeamManager = teamManager;

    MiniPortrait[] miniPortraits;
    Portrait portrait;
    void Start()
    {
        //EventHandler.current.onAddedPokemon += UpdateMiniPortrait;
        //EventHandler.current.onAllySelected += UpdatePortrait;

        miniPortraits = portraitParent.GetComponentsInChildren<MiniPortrait>();
    }

    void Update()
    {
        
    }

    //void UpdateMiniPortrait(PokemonContainer pokemonContainer)
    //{
    //    for (int i = 0; i < portraits.Length; i++)
    //    {
    //        if (i < teamManager.pokemons.Count)
    //        {
    //            miniPortraits[i].AddTeamMember(teamManager.pokemons[i]);
    //        }
    //        else
    //            miniPortraits[i].ClearTeamMember();
    //    }
    //}

    void UpdatePortrait(PokemonContainer pokemonContainer)
    {
        //Set selected pokemon
    }
}
