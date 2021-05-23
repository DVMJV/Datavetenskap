using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	Transform _camera;

    //void Start()
    //{
    //    _camera = Camera.main.transform;
    //}

    void LateUpdate()
	{
		transform.LookAt(transform.position + Camera.main.transform.forward);
	}
}
