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

    public event Action onStart;
    public event Action<int> onMoveSelected;
    public event Action onChangeSelectedObject;


    public void ChangeSelectedObject()
    {
        if(onChangeSelectedObject != null)
        {
            onChangeSelectedObject();
        }
    }

    public void OnStart()
    {
        if(onStart != null)
        {
            onStart();
        }
    }

    public void MoveSelected(int id)
    {
        if(onMoveSelected != null)
        {
            onMoveSelected(id);
        }
    }

}
