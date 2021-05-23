using System;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler current;

    #region Unity Functions

    private void Awake()
    {
        current = this;
    }

    #endregion
    
    #region Events

    #region GeneralSystem

    #region GeneralEvents
    public event Action onStart;
    #endregion

    #region GeneralEventCalls
    public void OnStart()
    {
        onStart?.Invoke();
    }
    #endregion

    #endregion

    #region SelectSystem

    #region SelectEvents
    public event Action<PokemonContainer> onAllySelected;
    public event Action onChangeSelectedObject;
    public event Action<SquareCell> onTileSelected;
    #endregion

    #region SelectEventCalls

    public void AllySelected(PokemonContainer pokemon)
    {
        onAllySelected?.Invoke(pokemon);
    }
    public void TileSelected(SquareCell selectedCell)
    {
        onTileSelected?.Invoke(selectedCell);
    }
    public void ChangeSelectedObject()
    {
        onChangeSelectedObject?.Invoke();
    }

    #endregion
    #endregion
    
    #region AttackSystem

    #region AttackEvents
    public event Action<AttackContainer> onAttackSelected;
    public event Action<SquareCell, PokemonAttack, string> onAttackTile;
    public event Action clearHighlights;
    #endregion

    #region AttackEventCalls
    public void AttackTile(SquareCell tile, PokemonAttack attack, string tag)
    {
        onAttackTile?.Invoke(tile, attack, tag);
    }
    public void MoveSelected(AttackContainer attack)
    {
        clearHighlights?.Invoke();
        onAttackSelected?.Invoke(attack);
    }
    #endregion
    #endregion

    #region MoveSystem
    #region MoveEvents
    public event Action<SquareCell, PokemonContainer> onMovePokemon;
    public event Action<Stack<SquareCell>, PokemonContainer> onPathFound;
    #endregion

    #region MoveEventCalls

    public void PathFound(Stack<SquareCell> path, PokemonContainer pokemon)
    {
        onPathFound?.Invoke(path, pokemon);
    }
    public void MovePokemon(SquareCell selectedCell, PokemonContainer pokemon)
    {
        onMovePokemon?.Invoke(selectedCell, pokemon);
    }

    #endregion
    #endregion

    #region PokemonSystem

    #region PokemonEvents
    public event Action<GameObject> onAddedPokemon;
    public event Action<GameObject> onRemovedPokemon;
    public event Action<List<GameObject>> onCreatePlayerPokemons;
    public event Action<List<SquareCell>> onPlayerSpawnCells;
    #endregion

    #region PokemonEventCalls

    public void AddPokemon(GameObject go)
    {
        onAddedPokemon?.Invoke(go);
    }
    
    public void RemovePokemon(GameObject go)
    {
        onRemovedPokemon?.Invoke(go);
    }

    public void CreatePlayerPokemons(List<GameObject> playerPokemons)
    {
        onCreatePlayerPokemons?.Invoke(playerPokemons);
    }

    public void PlayerSpawnCells(List<SquareCell> spawnCells)
    {
        onPlayerSpawnCells?.Invoke(spawnCells);
    }

    #endregion

    #endregion

    #region HubSystem

    #region HubEvents
    public event Action<GameObject> onItemBought;
    public event Action<PokemonContainer> onUpgradeSlotFilled;
    public event Action onUpgradeSlotEmpty;
    #endregion

    #region HubEventCalls
    public void ItemBought(GameObject boughtItemPrefab)
    {
        if(onItemBought != null)
        {
            onItemBought(boughtItemPrefab);
        }
    }

    public void UpgradeSlotFilled(PokemonContainer pokemonContainer)
    {
        if (onUpgradeSlotFilled != null)
        {
            onUpgradeSlotFilled(pokemonContainer);
        }
    }

    public void UpgradeSlotEmpty()
    {
        if(onUpgradeSlotEmpty != null)
        {
            onUpgradeSlotEmpty();
        }
    }

    #endregion
    #endregion

    #region TurnSystem
    #region TurnEvents
    public event Action<int> onTurnStart;
    public event Action onTurnEnd;
    public event Action<int> onTurnReset;
    public event Action onAllowedToEndTurn;
    #endregion

    #region TurnEventCalls
    public void StartTurn(int id)
    {
        onTurnStart?.Invoke(id);
    }

    public void EndTurn()
    {
        onTurnEnd?.Invoke();
    }

    public void ResetTurn(int id)
    {
        onTurnReset?.Invoke(id);
    }

    public void AllowedToEndTurn()
    {
        onAllowedToEndTurn?.Invoke();
    }
    

    #endregion
    
    #endregion
    
    #endregion

}
