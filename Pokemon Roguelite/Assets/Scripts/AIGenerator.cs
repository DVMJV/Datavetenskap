using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIGenerator : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float probability;
    [Range(1, 32)]
    [SerializeField] private int minimumAmountOfPokemon;
    [Range(1, 8)]
    [SerializeField] private int maxPokemonsPerRegion;
    [SerializeField] private List<GameObject> waterPokemons;
    [SerializeField] private List<GameObject> metalPokemons;
    [SerializeField] private List<GameObject> electricPokemons;
    [SerializeField] private List<GameObject> neutralPokemons;
    private void Awake()
    {
        EventHandler.current.onMapGenerated += GenerateAI;
    }

    private void GenerateAI(SquareCell[] map)
    {
        int waterPokemonsSpawned = 0;
        int metalPokemonsSpawned = 0;
        int electricPokemonsSpawned = 0;
        int neutralPokemonsSpawned = 0;

        while (waterPokemonsSpawned + metalPokemonsSpawned + electricPokemonsSpawned + neutralPokemonsSpawned <
               minimumAmountOfPokemon)
        {
            foreach (var currentCell in map)
            {
                float random = Random.Range(0f, 1f);
                if (currentCell.obstructed)
                    continue;
                if (currentCell.coordinates.X < 5 && currentCell.coordinates.Y < 5)
                    continue;
                if (random > probability)
                    continue;

                switch (currentCell.biomeType)
                {
                    case SquareCell.TYPE.WATER:
                        break;
                    case SquareCell.TYPE.BEACH:
                        if (waterPokemonsSpawned >= maxPokemonsPerRegion)
                            continue;
                        SpawnPokemon(waterPokemons, currentCell);
                        waterPokemonsSpawned++;
                        break;
                    case SquareCell.TYPE.FOREST:
                        if (neutralPokemonsSpawned >= maxPokemonsPerRegion)
                            continue;
                        SpawnPokemon(neutralPokemons, currentCell);
                        neutralPokemonsSpawned++;
                        break;
                    case SquareCell.TYPE.ELECTRIC:
                        if (electricPokemonsSpawned >= maxPokemonsPerRegion)
                            continue;
                        SpawnPokemon(electricPokemons, currentCell);
                        electricPokemonsSpawned++;
                        break;
                    case SquareCell.TYPE.METAL:
                        if (metalPokemonsSpawned >= maxPokemonsPerRegion)
                            continue;
                        SpawnPokemon(metalPokemons, currentCell);
                        metalPokemonsSpawned++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void SpawnPokemon(List<GameObject> pokemonList, SquareCell spawnCell)
    {
        GameObject go = Instantiate(pokemonList[Random.Range(0, pokemonList.Count)]);
        go.GetComponent<PokemonContainer>().CurrentTile = spawnCell;
        go.tag = "Enemy";
        EventHandler.current.AISpawned(go.GetComponent<PokemonContainer>());
    }
}
