using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeManager : MonoBehaviour
{
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            GetComponent<MeshRenderer>().material.SetFloat("Vector1_1CA2D5C7", GetComponent<MeshRenderer>().material.GetFloat("Vector1_1CA2D5C7") - 0.001f);
            timer = 0;
            if (GetComponent<MeshRenderer>().material.GetFloat("Vector1_1CA2D5C7") <= -1) GetComponent<MeshRenderer>().material.SetFloat("Vector1_1CA2D5C7", -1);

        }
    }
}
