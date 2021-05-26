//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class MiniPortrait : MonoBehaviour
//{
//    List<PokemonContainer> pokemons = new List<PokemonContainer>();
//    List<Sprite> miniPortraits = new List<Sprite>();
//    Sprite sprite;

//    private void Awake()
//    {
//        EventHandler.current.onCreatePlayerPokemons += GetPokemons;
//    }
//    void Start()
//    {
//        sprite = GetComponent<Sprite>();
//    }

    
//    void GetPokemons(List<GameObject> playerPokemons)
//    {
//        foreach (GameObject g in playerPokemons)
//        {
//            pokemons.Add(g.GetComponent<PokemonContainer>());
//        }
//    }

//    void SetPortrait()
//    {
//        foreach(Transform child in transform)
//        {
//            miniPortraits.Add(child.GetComponent<Sprite>());
//        }

//        for (int i = 0; i < pokemons.Count; i++)
//        {
//            miniPortraits[i] = pokemons[i].pokemon.sprite;
//        }
//    }

  
//}
