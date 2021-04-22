using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokemonSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] bool limitToOnePerSlot;
    [SerializeField] bool isUpgradeSlot;
    GameObject slottedObject;

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");

        if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("Dragable"))
        {
            if (limitToOnePerSlot)
                if (slottedObject != null)
                    return;

            DragDrop heldObject = eventData.pointerDrag.GetComponent<DragDrop>();
            heldObject.SetAnchorAndPosition(transform, this);
            
            if (isUpgradeSlot)
                EventHandler.current.UpgradeSlotFilled(heldObject.GetPokemonContainer());

            if (limitToOnePerSlot)
                slottedObject = heldObject.gameObject;
        }
    }

    public void EmptySlot()
    {
        if (isUpgradeSlot)
            EventHandler.current.UpgradeSlotEmpty();

        slottedObject = null;
    }
}
