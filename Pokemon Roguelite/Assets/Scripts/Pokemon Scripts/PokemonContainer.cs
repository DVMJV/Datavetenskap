using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonContainer : MonoBehaviour
{
    [SerializeField]
    Pokemon pokemon;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = pokemon.mesh;
        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        EventHandler.current.ChangeSelectedObject();
        EventHandler.current.AllySelected(pokemon);
    }
}
