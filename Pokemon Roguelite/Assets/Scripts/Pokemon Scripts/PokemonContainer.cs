using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonContainer : MonoBehaviour
{
    EventSystem eventSystem;
    
    [SerializeField]
    GameObject attackCanvasContainer;

    [SerializeField]
    GameObject attackPrefab;

    [SerializeField]
    Pokemon pokemon;

    Dictionary<PokemonAttack, int> learnset;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;

        learnset = new Dictionary<PokemonAttack, int>();
        GetComponent<MeshFilter>().mesh = pokemon.mesh;

        for(int i = 0; i < pokemon.attackToLearn.Count; i++)
        {
            learnset.Add(pokemon.attackToLearn[i], pokemon.levelToLearnAt[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EventHandler.current.AllySelected(gameObject);
    }

    

}
