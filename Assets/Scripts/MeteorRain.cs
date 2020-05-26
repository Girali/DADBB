using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRain : MonoBehaviour
{
    private Vector3 meteorRainSpawnPos;
    public GameObject meteorRain;
    public float spawnDistance = 10.0f;
    public float spawnHeight = 3f;

    public void SpawnMeteorRain()
    {
        meteorRainSpawnPos = new Vector3(0,25,0);
        meteorRainSpawnPos.y += spawnDistance;
        LaunchMeteorRain();
    }

    void LaunchMeteorRain()
    {
        Photon.Pun.PhotonNetwork.Instantiate(meteorRain.name, meteorRainSpawnPos, Quaternion.identity);
    }
}
