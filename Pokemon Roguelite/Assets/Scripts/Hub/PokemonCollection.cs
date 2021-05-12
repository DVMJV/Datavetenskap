using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class PokemonCollection : MonoBehaviour
{
    [SerializeField] List<GameObject> collection = new List<GameObject>();
    [SerializeField] GameObject handbookPortraitPrefab;

    RectTransform rectTransform;
    GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,
            gridLayoutGroup.spacing.y * (Mathf.CeilToInt(collection.Count * 0.5f) - 1)
            + gridLayoutGroup.padding.top
            + gridLayoutGroup.padding.bottom
            + gridLayoutGroup.cellSize.y * Mathf.CeilToInt(collection.Count * 0.5f));

        rectTransform.anchoredPosition = new Vector2(0, -rectTransform.sizeDelta.y / 2f);

        foreach (GameObject collectedPokemon in collection)
        {
            PokemonContainer pokemon = collectedPokemon.GetComponent<PokemonContainer>();
            GameObject go = Instantiate(handbookPortraitPrefab, transform);
            go.GetComponent<DragDrop>().CreatePortrait(GetComponent<PokemonSlot>(), pokemon);
        }
    }
}
