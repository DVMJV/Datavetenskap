using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 2)]
public class Item : ScriptableObject
{
    public new string name;
    public string description;

    //public Sprite sprite;

    public int cost;
    //public Component useEffect;
}
