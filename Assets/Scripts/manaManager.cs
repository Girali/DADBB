using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaManager : MonoBehaviour
{
    public void LoseMana(float mana)
    {
        GetComponent<MeshRenderer>().material.SetFloat("Vector1_84D8F053", GetComponent<MeshRenderer>().material.GetFloat("Vector1_84D8F053")-mana);
        if (GetComponent<MeshRenderer>().material.GetFloat("Vector1_84D8F053") <= -1) GetComponent<MeshRenderer>().material.SetFloat("Vector1_84D8F053",-1);
    }

    public void GainMana(float mana)
    {
        GetComponent<MeshRenderer>().material.SetFloat("Vector1_84D8F053", GetComponent<MeshRenderer>().material.GetFloat("Vector1_84D8F053") + mana);
        if (GetComponent<MeshRenderer>().material.GetFloat("Vector1_84D8F053") >= 1) GetComponent<MeshRenderer>().material.SetFloat("Vector1_84D8F053", 1);
    }
}
