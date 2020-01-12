using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    private void OnMouseOver()
    {
        GetComponent<MeshRenderer>().material.SetFloat("Vector1_EF97F8CF", 1.5f);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 1, transform.position.z), 0.1f);
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.SetFloat("Vector1_EF97F8CF", 100);
        while (transform.position.y >0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.1f);
        }
    }
}
