using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
	public float speed;
    void Update()
    {
        transform.eulerAngles += Vector3.up * Time.deltaTime * speed;
    }
}
