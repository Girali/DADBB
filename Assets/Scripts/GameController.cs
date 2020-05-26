using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{

    public GameObject playerPrefab;

    public Transform spawnPointPlayer;
    public Transform spawnPointMaster;

    public Transform spawnMinion;
    public Transform spawnElite;
    public Transform spawnBoss;

    public GameObject[] mobs;

    PhotonView pv;

    bool gameStarted = false;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPointMaster.position, spawnPointMaster.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPointPlayer.position, spawnPointPlayer.rotation);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
                StartGame();
    }

    public void SpawnMinions()
    {
        StartCoroutine(CRT_SpawnMobs(0, 5, spawnMinion.transform));
        GUI_Controller.Instance.StartAbility1();
    }

    public void SpawnElites()
    {
        GUI_Controller.Instance.StartAbility2();
        StartCoroutine(CRT_SpawnMobs(1, 2, spawnElite.transform));
    }
    public void SpawnBoss()
    {
        GUI_Controller.Instance.StartAbility3();
        StartCoroutine(CRT_SpawnMobs(2, 1, spawnBoss.transform));
    }
    public void SpawnMeteors()
    {
        //TODO
        GUI_Controller.Instance.StartAbility4();
    }

    IEnumerator CRT_SpawnMobs(int index, int count, Transform spawn)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate(mobs[index].name, spawn.position, spawn.rotation);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        pv.RPC("RPC_StartGame", RpcTarget.All);
    }
    
    [PunRPC]
    public void RPC_StartGame()
    {
        gameStarted = true;
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerView>().StartGame();
    }

}
