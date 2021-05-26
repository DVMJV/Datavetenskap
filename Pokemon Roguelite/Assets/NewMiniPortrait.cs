using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMiniPortrait : MonoBehaviour
{
    List<PokemonContainer> pokemons = new List<PokemonContainer>();
    //List<Sprite> miniPortraitSprites = new List<Sprite>();
    //List<Button> miniButtons = new List<Button>();
    List<MiniPortraitSelect> miniPortraits = new List<MiniPortraitSelect>();

    private void Awake()
    {
        EventHandler.current.onCreatePlayerPokemons += GetPokemons;
        
        
    }
    private void Start()
    {
        SetPortrait();
    }


    void GetPokemons(List<GameObject> playerPokemons)
    {
        foreach (GameObject g in playerPokemons)
        {
            pokemons.Add(g.GetComponent<PokemonContainer>());
        }
    }

    void SetPortrait()
    {
        Debug.Log(transform.childCount);
        foreach (Transform child in transform)
        {
            
            try
            {
                //miniPortraitSprites.Add(child.GetComponent<Sprite>());
                miniPortraits.Add(child.GetComponent<MiniPortraitSelect>());
            }
            catch (System.Exception)
            {

                
            }
        }

        for (int i = 0; i < pokemons.Count; i++)
        {
            //miniPortraitSprites[i] = pokemons[i].pokemon.sprite;
            miniPortraits[i].AddPortrait(pokemons[i].pokemon.sprite);
            miniPortraits[i].AddPokemon(pokemons[i]);
        }
    }

   


}
