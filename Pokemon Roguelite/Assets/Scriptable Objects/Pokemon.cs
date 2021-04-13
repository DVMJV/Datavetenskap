using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Pokemon", menuName = "ScriptableObjects/PokemonObjects", order = 1)]
public class Pokemon : ScriptableObject
{
    

    public string pokemonName;

    public Mesh mesh;

    public List<int> levelToLearnAt;

    public List<PokemonAttack> attackToLearn;

    public Type type;
}
