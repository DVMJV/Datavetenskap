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
    public event Action<Vector3> onTileSelected;


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

    public void TileSelected(Vector3 currentTilePos) 
    {
        if (onTileSelected != null)
        {
            onTileSelected(currentTilePos);
        }
    }
}
