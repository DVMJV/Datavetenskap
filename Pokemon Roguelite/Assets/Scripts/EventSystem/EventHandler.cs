using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler current;


    #region Events
    public event Action<PokemonContainer> onAllySelected;
    public event Action onStart;
    public event Action<int> onMoveSelected;
    public event Action onChangeSelectedObject;
    public event Action<Vector3> onTileSelected;
    public event Action<Vector3, PokemonContainer> onMovePokemon;
    public event Action<PokemonContainer> onAddedPokemon;
    public event Action<int> onTurnStart;
    public event Action onTurnEnd;
    public event Action<int> onTurnReset;

    #endregion




    private void Awake()
    {
        current = this;
    }


    public void MovePokemon(Vector3 tilePos, PokemonContainer pokemon)
    {
        onMovePokemon?.Invoke(tilePos, pokemon);
    }

    public void TileSelected(Vector3 tilePos)
    {
        onTileSelected?.Invoke(tilePos);
    }

    public void AllySelected(PokemonContainer pokemon)
    {
        onAllySelected?.Invoke(pokemon);
    }

    public void ChangeSelectedObject()
    {
        onChangeSelectedObject?.Invoke();
    }

    public void OnStart()
    {
        onStart?.Invoke();
    }

    public void MoveSelected(int id)
    {
        onMoveSelected?.Invoke(id);
    }

    public void AddPokemon(PokemonContainer pokemonContainer)
    {
        onAddedPokemon?.Invoke(pokemonContainer);
    }

    //HUB EVENTS

    public event Action<GameObject> onItemBought;
    public void ItemBought(GameObject boughtItemPrefab)
    {
        if(onItemBought != null)
        {
            onItemBought(boughtItemPrefab);
        }
    }

    public event Action<PokemonContainer> onUpgradeSlotFilled;
    public void UpgradeSlotFilled(PokemonContainer pokemonContainer)
    {
        if (onUpgradeSlotFilled != null)
        {
            onUpgradeSlotFilled(pokemonContainer);
        }
    }

    public event Action onUpgradeSlotEmpty;
    public void UpgradeSlotEmpty()
    {
        if(onUpgradeSlotEmpty != null)
        {
            onUpgradeSlotEmpty();
        }
    }

    #region TurnSystem

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

    #endregion
}
