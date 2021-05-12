using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item", order = 2)]
public abstract class ItemAbstract : ScriptableObject
{
    public new string name;
    public string description;

    //public Sprite sprite;

    public int shopCost;
  
    public abstract void Activate(GameObject target);

    public abstract List<string> Print();
}
