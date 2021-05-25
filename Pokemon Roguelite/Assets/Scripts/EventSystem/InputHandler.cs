using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public SquareGrid squareGrid;
    [SerializeField] private LayerMask mask;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            SquareCell currentCell = squareGrid.GetCell(hit.point);
            EventHandler.current.TileSelected(currentCell);
        }
    }
}
