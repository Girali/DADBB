using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constructionButton : MonoBehaviour
{
    [SerializeField]
    int coutBois;

    [SerializeField]
    int coutFer;

    [SerializeField]
    int coutFibre;

    [SerializeField]
    int coutPierre;

    bool canBeConstruct = false;

    [SerializeField]
    playerInventory inventory;

    public bool canConstruct()
    {
        canBeConstruct = true;
        if (coutBois > inventory.ressources[0]) canBeConstruct = false;
        if (coutFer > inventory.ressources[1]) canBeConstruct = false;
        if (coutFibre > inventory.ressources[2]) canBeConstruct = false;
        if (coutPierre > inventory.ressources[3]) canBeConstruct = false;

        return canBeConstruct;
    }

    public void construct()
    {
        inventory.ressources[0] -= coutBois;
        inventory.ressources[1] -= coutFer;
        inventory.ressources[2] -= coutFibre;
        inventory.ressources[3] -= coutPierre;
    }
}
