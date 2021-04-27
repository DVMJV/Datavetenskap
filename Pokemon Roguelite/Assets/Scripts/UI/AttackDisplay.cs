using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject attackDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.current.onAllySelected += AddAttacks;
        EventHandler.current.onChangeSelectedObject += RemoveAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveAttacks()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void AddAttacks(PokemonContainer pokemon)
    {
        int i = 1;
        foreach (var item in pokemon.learnedMoves)
        {
            item.Display(attackDisplayPrefab, transform, i);
            i++;
        }
    }
}
