using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBuilderMotor : MonoBehaviour
{
    PhotonView pv;
    public GameObject[] towers;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void SpawnTowerAt(Vector3 pos, int i)
    {
        PhotonNetwork.Instantiate(towers[i].name, pos, Quaternion.identity);
    }
}
