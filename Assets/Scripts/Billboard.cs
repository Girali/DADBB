using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform target;

    void Start()
    {
        if(Camera.main)
            target = Camera.main.transform;
    }

    void FixedUpdate()
    {
        if (target)
        {
            if (target.tag != "MainCamera")
                target = Camera.main.transform;
            transform.LookAt(target.position);
        }
        else
            if (Camera.main)
                target = Camera.main.transform;
    }
}
