using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_PlayerController : MonoBehaviour
{
    //Could be int list corresponding amount of items per item
    [SerializeField] List<GameObject> playerItems = new List<GameObject>(); 

    void Start()
    {
        EventHandler.current.onItemBought += NewItemBought;
    }

    void NewItemBought(GameObject boughtItemPrefab)
    {
        GameObject go = Instantiate(boughtItemPrefab, transform);
        playerItems.Add(go);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerItems.Count > 0)
        {
            playerItems[0].GetComponent<ItemContainer>().GetItem().Activate(transform.gameObject);
            playerItems.RemoveAt(0);
        }
    }

    void OnDestroy()
    {
        EventHandler.current.onItemBought -= NewItemBought;
    }
}
