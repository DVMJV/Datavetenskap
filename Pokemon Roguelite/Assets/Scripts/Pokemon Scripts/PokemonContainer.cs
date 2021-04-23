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
    public int currentMovement;
    public int currentHealth = 5;

    public List<PokemonAttack> learnedMoves = new List<PokemonAttack>();

    [SerializeField]
    SquareCell currentCell;
    public SquareCell CurrentTile { get { return currentCell; } 
        set
        {
            if (currentCell == value)
                return;

            currentCell = value;

            transform.position = new Vector3(currentCell.transform.position.x, currentCell.transform.position.y * currentCell.Elevation + transform.position.y , currentCell.transform.position.z);

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
        Instantiate(pokemon.mesh, transform);
        CurrentTile = currentCell;
        currentMovement = pokemon.movementSpeed;
        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.onTileSelected += Selected;
        EventHandler.current.onMovePokemon += Move;
        EventHandler.current.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Reset()
    {
        currentMovement = pokemon.movementSpeed;
    }


    private void Move(SquareCell selectedCell, PokemonContainer pokemon)
    {
        if (pokemon == this && selectedCell.Distance <= currentMovement)
        {
            Debug.Log("Current Movement: " + currentMovement + "Distance: " + selectedCell.Distance);
            currentMovement -= selectedCell.Distance;
            CurrentTile = selectedCell;
        }
    }

    public void LearnMove(PokemonAttack newMove)
    {
        //if (!learnedMoves.Contains(newMove))
            learnedMoves.Add(newMove);
    }

    private void Selected(SquareCell selectedTile)
    {
        if(selectedTile == currentCell)
        {
            EventHandler.current.AllySelected(this);
        }
    }

}
