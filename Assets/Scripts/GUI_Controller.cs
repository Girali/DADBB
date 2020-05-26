using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Controller : MonoBehaviour
{
    static GUI_Controller instance;
    public Timer timer;
    public Text life;

    public GameObject master;
    public GameObject player;

    public GameObject Button_SpawnMinions;
    public GameObject Button_SpawnElites;
    public GameObject Button_SpawnBoss;
    public GameObject Button_SpawnMeteors;

    public GameObject Button_Tower;
    public GameObject Button_Heal_Tower;
    public GameObject Button_Barricade;
    public GameObject Button_Super_Tower;

    public static GUI_Controller Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GUI_Controller>();
            return instance;
        }
    }

    public void StartAbility1()
    {
        StartCoroutine(Activate(Button_SpawnMinions,10));
    }

    public void StartAbility2()
    {
        StartCoroutine(Activate(Button_SpawnElites,20));
    }

    public void StartAbility3()
    {
        StartCoroutine(Activate(Button_SpawnBoss,30));
    }

    public void StartAbility4()
    {
        StartCoroutine(Activate(Button_SpawnMeteors,50));
    }

    public void StartPlayerAbility1()
    {
        StartCoroutine(Activate(Button_Tower,10));
    }

    public void StartPlayerAbility2()
    {
        StartCoroutine(Activate(Button_Heal_Tower,20));
    }

    public void StartPlayerAbility3()
    {
        StartCoroutine(Activate(Button_Barricade,30));
    }

    public void StartPlayerAbility4()
    {
        StartCoroutine(Activate(Button_Super_Tower,50));
    }


    private void Start()
    {
        if(Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            player.SetActive(false);
        }
        else
        {
            master.SetActive(false);
        }
    }

    IEnumerator Activate(GameObject Button,float time)
    {
        Button.GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(time);

        Button.GetComponent<Button>().interactable = true;
    }

}
