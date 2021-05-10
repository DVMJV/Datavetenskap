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
    int currentHealth = 5;

    [SerializeField]
    public SquareCell currentCell;

    public List<AttackContainer> learnedMoves = new List<AttackContainer>();

    [SerializeField]
    PokemonAttack[] temp;

    AttackContainer attackSelected;

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
        GameObject go = Instantiate(pokemon.mesh, transform);
        CurrentTile = currentCell;
        currentMovement = pokemon.movementSpeed;

        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.onTileSelected += PokemonSelected; 
        EventHandler.current.onTileSelected += AttackTile;
        EventHandler.current.onPathFound += Move;
        EventHandler.current.onAttackSelected += AttackSelected;
        EventHandler.current.onAllySelected += Unselect;
        EventHandler.current.onAttackTile += TileAttacked;

        foreach(PokemonAttack attack in temp)
        {
            LearnMove(attack);
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    private void TileAttacked(SquareCell attackedTile, PokemonAttack attack, string tag)
    {
        if(attackedTile == currentCell && tag != gameObject.tag)
        {
            currentHealth -= attack.damage;
        }
    }

    public void EndTurn()
    {
        currentMovement = pokemon.movementSpeed;
        foreach (AttackContainer attack in learnedMoves)
            attack.LowerCooldown();
    }

    private void Move(Stack<SquareCell> path, PokemonContainer pokemon)
    {
        if (pokemon == this && attackSelected == null)
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


        EventHandler.current.AllowedToEndTurn();
        if (CompareTag("Friendly"))
        {
            EventHandler.current.AllySelected(this);
            Player.AllowedToEndTurn = true;
        }
    }

    public void AttackSelected(AttackContainer attack)
    {
        if (learnedMoves.Contains(attack))
        {
            attackSelected = attack;
            attack.FindAttackableTiles(currentCell);
        }
    }

    public void LearnMove(PokemonAttack newMove)
    {
        learnedMoves.Add(new AttackContainer(newMove));
    }

    private void PokemonSelected(SquareCell selectedTile)
    {
        if(selectedTile == currentCell && gameObject.CompareTag("Friendly"))
        {
            EventHandler.current.AllySelected(this);
        }
    }

    private void AttackTile(SquareCell selectedTile)
    {
        if(attackSelected != null)
        {
            attackSelected.Attack(currentCell, selectedTile, tag);
        }
    }

    private void Unselect(PokemonContainer pokemon)
    {
        if(pokemon != this)
        {
            attackSelected = null;
        }
    }
}
