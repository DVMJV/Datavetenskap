using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    GameObject dragLayerObject;
    Canvas canvas;
    PokemonSlot activeSlot;
    Pokemon activePokemonTemplate;
    PokemonContainer activePokemon;

    Image image;
    TMP_Text tmpText;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>(); //Should be changed to accommodate multiple canvas
        dragLayerObject = GameObject.Find("DragLayer");
        image = GetComponent<Image>();
        tmpText = GetComponentInChildren<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        rectTransform.SetParent(dragLayerObject.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        SetPosition(activeSlot.transform); 
        //^Resets even when a new slot is found 
        //(onDrop seems to happen before onEndDrag meaning its is double reseting position)
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

    public void SetAnchorAndPosition(Transform newParent, PokemonSlot newSlot)
    {
        SetPosition(newParent);

        if (activeSlot != null)
            activeSlot.EmptySlot();
        activeSlot = newSlot;
    }

    public void SetPosition(Transform newParent) 
    {
        //Messing with anchor position forces layout group to reset. 
        //(Used be in SetAnchorAndPositon and this method would only reset anc. pos. to zero)
        //Only changing anc. pos. will put object in the layout group in the corner.
        rectTransform.SetParent(newParent);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector3.zero;
    }

    public void CreatePortrait(PokemonSlot pokemonSlot, PokemonContainer pokemonContainer)
    {
        activeSlot = pokemonSlot;
        activePokemon = pokemonContainer;
        activePokemonTemplate = pokemonContainer.pokemon;

        image.sprite = activePokemonTemplate.sprite;
        tmpText.text = activePokemonTemplate.pokemonName;
    }

    public PokemonContainer GetPokemonContainer()
    {
        return activePokemon;
    }
}
