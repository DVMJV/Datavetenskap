using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] ItemAbstract item;

    public ItemAbstract GetItem()
    {
        return item;
    }
}
