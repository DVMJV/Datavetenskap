using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    public int width = 5;
    public int height = 5;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    SquareMesh squareMesh;
    MeshCollider meshCollider;

    public Color defaultColor = Color.white;

    List<SquareCell> walkableTiles = new List<SquareCell>();

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        squareMesh = GetComponentInChildren<SquareMesh>();
        cells = new SquareCell[height * width];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void ClearHighlights()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
        }
    }

    private void Start()
    {
        squareMesh.Triangulate(cells);
        EventHandler.current.onAllySelected +=  FindAllPossibleTiles;
        EventHandler.current.onTurnEnd += ClearHighlights;
    }

    public SquareCell GetCell(Vector3 position, Color color) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);
        int index = ((coordinates.X + (coordinates.Z * width)));
        return cells[index];     
    }

    public void Refresh() 
    {
        squareMesh.Triangulate(cells);
    }

    public void FindAllPossibleTiles(PokemonContainer selectedPokemon)
    {
        SearchForTiles(selectedPokemon.currentMovement, selectedPokemon.CurrentTile);
    }

    public void FindPath(SquareCell fromCell, SquareCell toCell)
    {
        SearchForPath(fromCell, toCell);
    }

    void SearchForTiles(int speed, SquareCell currentTile)
    {

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
        }


        Queue<SquareCell> openSet = new Queue<SquareCell>();
        currentTile.Distance = 0;
        openSet.Enqueue(currentTile);

        while(openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current.Distance >= speed && current.Distance != int.MaxValue)
                continue;

            for(SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);

                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 1)
                    continue;
                else if(neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    if (neighbor.Distance > speed)
                        continue;
                    else
                    {
                        neighbor.EnableHighlight(Color.blue);
                        openSet.Enqueue(neighbor);
                        walkableTiles.Add(neighbor);
                    }
                }
            }

        }
    }


    void SearchForPath(SquareCell fromCell, SquareCell toCell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
        }

        fromCell.Distance = 0;
        fromCell.EnableHighlight(Color.blue);
        toCell.EnableHighlight(Color.red);

        SquareCellPriorityQueue openSet = new SquareCellPriorityQueue();
        openSet.Enqueue(fromCell);

        while(openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current == toCell)
            {
                current = current.PathFrom;
                while(current != fromCell)
                {
                    current.EnableHighlight(Color.black);
                    current = current.PathFrom;
                }
                break;
            }

            for(SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 1)
                    continue;
                else if(neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    neighbor.PathFrom = current;
                    openSet.Enqueue(neighbor);
                }
                else if(current.Distance + 1 < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = current.Distance + 1;
                    neighbor.PathFrom = current;
                    openSet.Change(neighbor, oldPriority);
                }
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        SquareCell cell = cells[i] = Instantiate<SquareCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = SquareCoordinates.FromOffsetCoordinates(x, z); // Create struct with coordinates. Might need adjustment.

        cell.color = defaultColor;

        if (x > 0)
            cell.SetNeighbor(SquareDirection.LEFT, cells[i - 1]);
        if (z > 0)
        {
            cell.SetNeighbor(SquareDirection.DOWN, cells[i - width]);
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        cell.uiRect = label.rectTransform;
    }
}


