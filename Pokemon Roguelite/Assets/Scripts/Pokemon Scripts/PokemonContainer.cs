using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokemonContainer : MonoBehaviour
{
    #region Variables
    #region Public Variables
    
    public int currentLevel = 5;
    public int currentMovement;
    
    public bool hasAttacked = false;
    
    public List<AttackContainer> learnedMoves = new List<AttackContainer>();
    [SerializeField] public Pokemon pokemon;

    #endregion

    #region Private Variables
    public int currentHealth = 5;
    
    private bool stunned;
    
    [SerializeField] private SquareCell currentCell;
    [SerializeField] private PokemonAttack[] temp;

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private AttackContainer attackSelected;
    #endregion
    
    #region Properties
    public SquareCell CurrentTile { 
        get => currentCell;
        set
        {
            if (currentCell == value)
                return;
            if(currentCell != null)
                currentCell.obstructed = false;

            currentCell = value;

            currentCell.obstructed = true;

            var position = currentCell.transform.position;
            transform.position = new Vector3(position.x, position.y, position.z);
        }
    }

    public float CurrentHealth
    {
        get => currentHealth;
    }
    #endregion
    #endregion

    #region Unity Functions

    private void Start()
    {
        //Destroy(GetComponent<MeshRenderer>());
        //GameObject go = Instantiate(pokemon.mesh, transform);
        CurrentTile = currentCell;
        currentMovement = pokemon.movementSpeed;

        EventHandler.current.onStart += pokemon.OnStart;
        EventHandler.current.onTileSelected += PokemonSelected; 
        EventHandler.current.onTileSelected += AttackTile;
        EventHandler.current.onPathFound += Move;
        EventHandler.current.onAttackSelected += AttackSelected;
        EventHandler.current.onAllySelected += Unselect;
        EventHandler.current.onAttackTile += TileAttacked;

        SetMaxHealth();

        foreach(PokemonAttack attack in pokemon.attackToLearn)
        {
            LearnMove(attack);
        }
    }

    private void OnDestroy()
    {
        EventHandler.current.onStart -= pokemon.OnStart;
        EventHandler.current.onTileSelected -= PokemonSelected; 
        EventHandler.current.onTileSelected -= AttackTile;
        EventHandler.current.onPathFound -= Move;
        EventHandler.current.onAttackSelected -= AttackSelected;
        EventHandler.current.onAllySelected -= Unselect;
        EventHandler.current.onAttackTile -= TileAttacked;

        EventHandler.current.PokemonDestroyed(this);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            currentCell.obstructed = false;
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// Handles ending the turn for the pokemon
    /// </summary>
    public void EndTurn()
    {
        currentMovement = pokemon.movementSpeed;
        hasAttacked = false;
        stunned = false;
        foreach (AttackContainer attack in learnedMoves)
            attack.LowerCooldown();
    }
    /// <summary>
    /// Learns a new move.
    /// </summary>
    /// <param name="newMove"></param>
    public void LearnMove(PokemonAttack newMove)
    {
        learnedMoves.Add(new AttackContainer(newMove));
    }
    
    /// <summary>
    /// Returns if the pokemon is currently stunned or not
    /// </summary>
    /// <returns></returns>
    public bool IsStunned()
    {
        return stunned;
    }
    /// <summary>
    /// Calculates the damage taken based on pokemon type and attack type
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public int CalculateDamage(PokemonAttack attack)
    {
        var adjustedDamage = attack.damage;
        var attackType = attack.type;
        switch (attackType)
        {
            case Type.NEUTRAL:
                adjustedDamage = attack.damage;
                break;
            case Type.WATER:
                if (pokemon.type == Type.LIGHTNING)
                    adjustedDamage = attack.damage / 2;
                else if (pokemon.type == Type.METAL)
                    adjustedDamage = attack.damage * 2;
                else
                    adjustedDamage = attack.damage;
                break;
            case Type.LIGHTNING:
                if (pokemon.type == Type.WATER)
                    adjustedDamage = attack.damage * 2;
                else if (pokemon.type == Type.METAL)
                    adjustedDamage = attack.damage / 2;
                else
                    adjustedDamage = attack.damage;
                break;
            case Type.METAL:
                if (pokemon.type == Type.LIGHTNING)
                    adjustedDamage = attack.damage * 2;
                else if (pokemon.type == Type.WATER)
                    adjustedDamage = attack.damage / 2;
                else
                    adjustedDamage = attack.damage;
                break;
        }

        return adjustedDamage;
    }
    #endregion

    #region Private

    void SetMaxHealth()
    {
        currentHealth = pokemon.health;
        slider.maxValue = pokemon.health;
        slider.value = pokemon.health;

        fill.color = gradient.Evaluate(1f);
    }

    void SetHealth()
    {
        slider.value = currentHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    #endregion

    #region Event Listeners

    /// <summary>
    /// Checks if the tile the pokemon is standing on was attacked.
    /// </summary>
    /// <param name="attackedTile"></param>
    /// <param name="attack"></param>
    /// <param name="tag"></param>
    private void TileAttacked(SquareCell attackedTile, PokemonAttack attack, string tag)
    {
        if (attackedTile != currentCell || CompareTag(tag)) return;
        var effect = attack.effect;

        switch (effect)
        {
            case PokemonAttack.SecondaryEffect.Stun:
                stunned = true;
                currentMovement = 0;
                break;
            default:
                break;
        }

        currentHealth -= CalculateDamage(attack);
        SetHealth();
    }
    
    /// <summary>
    /// Moves the pokemon following a specific path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pokemon"></param>
    private void Move(Stack<SquareCell> path, PokemonContainer pokemon)
    {
        if (pokemon == this && attackSelected == null)
        {
            StartCoroutine(MoveEnumerator(path));
        }
    }
    
    /// <summary>
    /// Selects an attack
    /// </summary>
    /// <param name="attack"></param>
    private void AttackSelected(AttackContainer attack)
    {
        if (!learnedMoves.Contains(attack) || hasAttacked || stunned)
            return;
        attackSelected = attack;
        Debug.Log("Attack Selected: " + attackSelected.GetName());
        attack.FindAttackableTiles(currentCell);
        attack.HighlightAttack();
    }
    
    /// <summary>
    /// Checks if this was the pokemon selected
    /// </summary>
    /// <param name="selectedTile"></param>
    private void PokemonSelected(SquareCell selectedTile)
    {
        if(selectedTile == currentCell && gameObject.CompareTag("Friendly"))
        {
            Debug.Log("Test");
            EventHandler.current.AllySelected(this);
        }
    }
    
    /// <summary>
    /// Attacks a tile
    /// </summary>
    /// <param name="selectedTile"></param>
    private void AttackTile(SquareCell selectedTile)
    {
        if (attackSelected == null) return;
        hasAttacked = attackSelected.Attack(currentCell, selectedTile, tag);
        EventHandler.current.AllySelected(null);
    }
    
    /// <summary>
    /// Unselects the pokemon
    /// </summary>
    /// <param name="pokemon"></param>
    private void Unselect(PokemonContainer pokemon)
    {
        attackSelected = null;
    }

    #endregion

    #region Enumerators

    /// <summary>
    /// Slows down how the pokemon moves, when finished it allows the current turn to be ended.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator MoveEnumerator(Stack<SquareCell> path)
    {
        WaitForSeconds delay = new WaitForSeconds(1 / 8f);

        while (path.Count > 0)
        {
            SquareCell moveToCell = path.Pop();
            Vector3 rotateDirection = moveToCell.transform.position - currentCell.transform.position;
            rotateDirection.y = 0;
            transform.forward = rotateDirection.normalized;
            yield return delay;
            currentMovement -= 1;
            CurrentTile = moveToCell;
        }
        if (CompareTag("Friendly"))
        {
            EventHandler.current.AllySelected(this);
        }
        EventHandler.current.AllowedToEndTurn();
    }
    

    #endregion
}
