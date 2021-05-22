using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpike : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;

    void Update()
    {
        if (ps.isPlaying)
        {
            ps.TriggerSubEmitter(0);
        }
    }
}
