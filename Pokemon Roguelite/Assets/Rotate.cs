using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 5;

    void Update()
    {
        Vector3 rot = new Vector3(0, rotationSpeed, 0);
        rot = rot * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rot);    
    }
}
