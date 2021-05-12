using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Revive Item", menuName = "ScriptableObjects/Item/ReviveStone", order = 1)]
public class ReviveStone : ItemAbstract
{
    public override void Activate(GameObject target)
    {
        Debug.Log("REVIVE TARGET");
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
