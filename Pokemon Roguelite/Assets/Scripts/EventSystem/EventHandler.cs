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
    
    #endregion




    private void Awake()
    {
        current = this;
    }


    public void MovePokemon(Vector3 tilePos, PokemonContainer pokemon)
    {
        if(onMovePokemon != null)
        {
            onMovePokemon(tilePos, pokemon);
        }
    }

    public void TileSelected(Vector3 tilePos)
    {
        if(onTileSelected != null)
        {
            onTileSelected(tilePos);
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
