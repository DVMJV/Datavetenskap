using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Item", menuName = "ScriptableObjects/Item/HealItem", order = 0)]
public class HealingSpray : ItemAbstract
{
    public int healAmount;

    public override void Activate(GameObject target)
    {
        Debug.Log("HEALED FOR " + healAmount);
    }

    public override List<string> Print()
    {
        List<string> data = new List<string>();
        data.Add(name);
        data.Add(description);
        data.Add(shopCost.ToString());
        data.Add(healAmount.ToString());
        return data;
    }
}
