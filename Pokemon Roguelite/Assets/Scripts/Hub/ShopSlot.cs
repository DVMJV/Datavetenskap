using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Button slotButton;
    [SerializeField] TMP_Text tmpText;

    [HideInInspector] public Item itemForSale;

    public void RestockSlot(Item _itemForSale)
    {
        slotButton.interactable = true;
        itemForSale = _itemForSale;
        tmpText.text = itemForSale.name + "\n" + itemForSale.cost + " $";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == null)
            return;
        if (Shop.money < itemForSale.cost)
        {
            Debug.Log("NOT ENOUGH FUNDS!");
            return;
        }

        Shop.money -= itemForSale.cost;
        Debug.Log("FUNDS: " + Shop.money);
        slotButton.interactable = false;
    }
}
