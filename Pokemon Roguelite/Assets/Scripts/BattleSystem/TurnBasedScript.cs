using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedScript : MonoBehaviour
{
    int id = 0;
    void Start()
    {
        EventHandler.current.onTurnEnd += EndTurn;
        EventHandler.current.onStart += StartTurn;
    }

    void StartTurn()
    {
        EventHandler.current.StartTurn(id);
    }

    void EndTurn()
    {
        EventHandler.current.ResetTurn(id);
        id = 1 - id;
        StartTurn();
    }
}
