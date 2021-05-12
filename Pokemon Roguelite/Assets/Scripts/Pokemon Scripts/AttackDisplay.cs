using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackDisplay : MonoBehaviour, IPointerDownHandler
{
    public int id;
    AttackContainer attackContainer;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventHandler.current.MoveSelected(attackContainer);
    }

    public void SetAttackContainer(AttackContainer attackContainer)
    {
        this.attackContainer = attackContainer;
    }
}
