using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{

    public GameObject playerPrefab;

    public Transform spawnPointPlayer;
    public Transform spawnPointMaster;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPointMaster.position, spawnPointMaster.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPointPlayer.position, spawnPointPlayer.rotation);
        }
    }
}
