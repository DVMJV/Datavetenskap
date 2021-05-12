using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerDownHandler
{
    Button slotButton;
    TMP_Text tmpText;

    [HideInInspector] public GameObject itemForSalePrefab;
    ItemAbstract itemForSale;

    void Awake()
    {
        slotButton = GetComponent<Button>();
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void RestockSlot(GameObject _itemForSalePrefab)
    {
        slotButton.interactable = true;
        itemForSalePrefab = _itemForSalePrefab;
        itemForSale = itemForSalePrefab.GetComponent<ItemContainer>().GetItem();
        List<string> data = itemForSale.Print();
        tmpText.text = data[0] + "\n" + data[2] + " $";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == null)
            return;

        if (Shop.money < itemForSale.shopCost)
        {
            Debug.Log("NOT ENOUGH FUNDS!");
            return;
        }

        EventHandler.current.ItemBought(itemForSalePrefab);
        Shop.money -= itemForSale.shopCost;
        Debug.Log("FUNDS: " + Shop.money);
        slotButton.interactable = false;
    }
}
