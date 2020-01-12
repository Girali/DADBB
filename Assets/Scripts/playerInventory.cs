using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInventory : MonoBehaviour
{
    [SerializeField]
    constructionButton[] constructions;

    [SerializeField]
    Text[] ressourcesView;

    public int[] ressources = new int[4]; //0 = bois, 1 = fer, 2 = fibre, 3 = pierre

    private void Start()
    {
        for (int i = 0; i < ressources.Length; i++)
        {
            ressources[i] = 10;
        }
    }

    private void Update()
    {
        foreach (constructionButton constru in constructions) if (constru.canConstruct()) constru.gameObject.GetComponent<Button>().interactable = true;
        else constru.gameObject.GetComponent<Button>().interactable = false;
        ressourcesView[0].text = ressources[0].ToString() + " bois";
        ressourcesView[1].text = ressources[1].ToString() + " fer";
        ressourcesView[2].text = ressources[2].ToString() + " fibre";
        ressourcesView[3].text = ressources[3].ToString() + " pierre";
    }
}
