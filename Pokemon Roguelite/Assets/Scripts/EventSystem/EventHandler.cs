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
    public event Action<SquareCell> onTileSelected;
    public event Action<SquareCell, PokemonContainer> onMovePokemon;
    public event Action<PokemonContainer> onAddedPokemon;
    
    #endregion

    private void Awake()
    {
        current = this;
    }


    public void MovePokemon(SquareCell selectedCell, PokemonContainer pokemon)
    {
        if(onMovePokemon != null)
        {
            onMovePokemon(selectedCell, pokemon);
        }
    }

    public void TileSelected(SquareCell selectedCell)
    {
        if(onTileSelected != null)
        {
            onTileSelected(selectedCell);
        }
    }

    public void AllySelected(PokemonContainer pokemon)
    {
        if(onAllySelected != null)
        {
            onAllySelected(pokemon);
        }
    }

    public void ChangeSelectedObject()
    {
        if(onChangeSelectedObject != null)
        {
            onChangeSelectedObject();
        }
    }

    public void OnStart()
    {
        if(onStart != null)
        {
            onStart();
        }
    }

    public void MoveSelected(int id)
    {
        if(onMoveSelected != null)
        {
            onMoveSelected(id);
        }
    }

    public void AddPokemon(PokemonContainer pokemonContainer)
    {
        if(onAddedPokemon != null)
        {
            onAddedPokemon(pokemonContainer);
        }
    }
}
