using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public static int money = 100; //MOVE TO PLAYER CONTROLLER LOGIC

    [SerializeField] List<GameObject> itemPool = new List<GameObject>();

    List<ShopSlot> shopButtons = new List<ShopSlot>();

    void Start()
    {
        ShopSlot[] children = transform.GetComponentsInChildren<ShopSlot>();
        foreach (ShopSlot c in children)
        {
            if (c.transform.parent == transform)
            {
                shopButtons.Add(c);
            }
        }

        RestockShop();
    }

    void RestockShop()
    {
        foreach (ShopSlot slot in shopButtons)
        {
            GameObject i = itemPool[Random.Range(0, itemPool.Count)];
            slot.RestockSlot(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestockShop();
    }
}
