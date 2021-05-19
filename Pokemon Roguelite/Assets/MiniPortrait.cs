
using UnityEngine;
using UnityEngine.UI;

public class MiniPortrait : MonoBehaviour
{
    public Image portrait;
    PokemonContainer pokemonContainer;

    public void AddTeamMember(PokemonContainer pokeCont)
    {
        pokemonContainer = pokeCont;
        //portrait.sprite = pokemonContainer.portrait;
        portrait.enabled = true;
    }

    public void ClearTeamMember()
    {
        pokemonContainer = null;
        portrait.sprite = null;
        portrait.enabled = false; 
    }
   
}
