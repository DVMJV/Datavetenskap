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

    [SerializeField]
    public SquareCell currentCell;


    public SquareCell CurrentTile { get { return currentCell; } 
        set
        {
            if (currentCell == value)
                return;

            currentCell = value;

            transform.position = new Vector3(currentCell.transform.position.x, currentCell.transform.position.y + currentCell.Elevation, currentCell.transform.position.z);

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
        EventHandler.current.onPathFound += Move;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        currentMovement = pokemon.movementSpeed;
    }


    private void Move(Stack<SquareCell> path, PokemonContainer pokemon)
    {
        if (pokemon == this)
        {
            StartCoroutine(MoveEnumerator(path));
        }
    }

    IEnumerator MoveEnumerator(Stack<SquareCell> path)
    {
        Player.AllowedToEndTurn = false;
        WaitForSeconds delay = new WaitForSeconds(1 / 10f);

        while (path.Count > 0)
        {
            SquareCell moveToCell = path.Pop();
            yield return delay;
            currentMovement -= 1;
            CurrentTile = moveToCell;
        }

        EventHandler.current.AllySelected(this);
        Player.AllowedToEndTurn = true;
    }

    public void LearnMove(PokemonAttack newMove)
    {
    }

    private void Selected(SquareCell selectedTile)
    {
        if(selectedTile == currentCell)
        {
            Debug.Log("Set selected tile: " + selectedTile.coordinates.ToString());
            EventHandler.current.AllySelected(this);
        }
    }

}
