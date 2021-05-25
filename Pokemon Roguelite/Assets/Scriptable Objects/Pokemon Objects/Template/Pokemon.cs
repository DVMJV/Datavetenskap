using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Pokemon", menuName = "ScriptableObjects/PokemonObjects", order = 1)]
public class Pokemon : ScriptableObject
{
    public string pokemonName;

    public int movementSpeed;

    public int health;

    public GameObject mesh;

    public List<PokemonAttack> attackToLearn;

    public Type type;

    public Sprite sprite;

    public void OnStart()
    {
        for (int i = 0; i < attackToLearn.Count; i++)
        {   
            EventHandler.current.onChangeSelectedObject += attackToLearn[i].ResetEvent;
        }
    }
}
