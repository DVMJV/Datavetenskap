using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonContainer : MonoBehaviour
{

    [SerializeField]
    Pokemon pokemon;

    Dictionary<PokemonAttack, int> learnset;
    // Start is called before the first frame update
    void Start()
    {
        learnset = new Dictionary<PokemonAttack, int>();
        GetComponent<MeshFilter>().mesh = pokemon.mesh;

        for(int i = 0; i < pokemon.attackToLearn.Count; i++)
        {
            learnset.Add(pokemon.attackToLearn[i], pokemon.levelToLearnAt[i]);
        }
    }

    private void OnMouseEnter()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
