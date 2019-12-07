using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{

    public GameObject playerPrefab;

    public Transform spawnPointPlayer;
    public Transform spawnPointMaster;

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
