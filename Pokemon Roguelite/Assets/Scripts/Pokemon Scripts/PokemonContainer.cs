using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonContainer : MonoBehaviour
{
    [SerializeField]
    public Pokemon pokemon;

    public int currentLevel = 5;

    public int currentHealth = 5;

    List<PokemonAttack> learnedMoves;

    public SquareCell CurrentTile { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
        Instantiate(pokemon.mesh, transform);
        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.onTileSelected += Selected;
        EventHandler.current.onMovePokemon += Move;
        EventHandler.current.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Move(Vector3 pos, PokemonContainer pokemon)
    {
        if(pokemon == this)
        {
            Vector3 moveVector = pos - transform.position;

            if(moveVector.sqrMagnitude < this.pokemon.movementSpeed * this.pokemon.movementSpeed)
            {
                transform.position = pos;
            }
        }
    }

    private void Selected(Vector3 tilePos)
    {
        Debug.Log("Current Health: " + currentHealth);


        if (Mathf.FloorToInt(transform.position.x) == Mathf.FloorToInt(tilePos.x) && Mathf.FloorToInt(transform.position.z) == Mathf.FloorToInt(tilePos.z))
        {
            EventHandler.current.ChangeSelectedObject();
            EventHandler.current.AllySelected(this);
        }
    }

}
