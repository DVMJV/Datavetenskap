using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour, IPointerDownHandler
{
    Button slotButton;
    TMP_Text tmpText;

    PokemonContainer pokemon;
    PokemonAttack moveToLearn;

    void Awake()
    {
        slotButton = GetComponent<Button>();
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void SetUpUpgradeSlot(PokemonContainer _pokemon, PokemonAttack _moveToLearn)
    {
        pokemon = _pokemon;
        moveToLearn = _moveToLearn;

        tmpText.text = moveToLearn.attackName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == null)
            return;

        Debug.Log("MOVE BOUGHT");
        pokemon.LearnMove(moveToLearn);
        gameObject.SetActive(false);
        slotButton.interactable = false;
    }
}
