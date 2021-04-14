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
        Destroy(GetComponent<MeshRenderer>());
        Instantiate(pokemon.mesh, transform);
        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.onTileSelected += Selected;
        EventHandler.current.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Selected(Vector3 tilePos)
    {
        if(Mathf.FloorToInt(transform.position.x) == tilePos.x && Mathf.FloorToInt(transform.position.z) == tilePos.z)
        {
            EventHandler.current.ChangeSelectedObject();
            EventHandler.current.AllySelected(pokemon);
        }
    }

}
