using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bonk Item", menuName = "ScriptableObjects/Item/Bonk", order = 2)]
public class Bonk : ItemAbstract
{
    public override void Activate(GameObject target)
    {
        Debug.Log("BONKED");
    }

    public override List<string> Print()
    {
        List<string> data = new List<string>();
        data.Add(name);
        data.Add(description);
        data.Add(shopCost.ToString());
        return data;
    }
}
