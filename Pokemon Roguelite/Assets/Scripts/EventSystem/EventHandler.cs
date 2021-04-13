using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour
{
    public static EventHandler current;


    private void Awake()
    {
        current = this;
    }


    public event Action<GameObject> onAllySelected;

    public void AllySelected(GameObject go)
    {
        if(onAllySelected != null)
        {
            onAllySelected(go);
        }
    }
}
