using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject target;
    public float sensitivity = 120f;
    public Vector3 offset;
    public float minOffset;
    public float maxOffset;
    
    void Update()
    {
        offset.z -= -Input.GetAxis("Mouse ScrollWheel") * sensitivity * Time.deltaTime;
        offset.z = Mathf.Clamp(offset.z, minOffset, maxOffset);

        Vector3 position = Camera.main.transform.rotation * offset;

        Camera.main.transform.position = position;
    }
}
