using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoView : MonoBehaviour
{
    public Material mat;
    public Color ok;
    [ColorUsage(true, true)]
    public Color okHDR;
    public Color notok;
    [ColorUsage(true, true)]
    public Color notokHDR;

    public void IsOk()
    {
        mat.color = ok;
        mat.SetColor("_EmissionColor", okHDR);
    }

    public void IsNotOk()
    {
        mat.color = notok;
        mat.SetColor("_EmissionColor", notokHDR);
    }
}
