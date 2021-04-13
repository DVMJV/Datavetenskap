using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokemonAttackContainer : MonoBehaviour, IPointerDownHandler
{
    public PokemonAttack attack;

    public void OnPointerDown(PointerEventData eventData)
    {
        attack.Attack();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
