using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRain : MonoBehaviour
{
    private Vector3 meteorRainSpawnPos;
    public GameObject meteorRain;
    public float spawnDistance = 10.0f;
    public float spawnHeight = 3f;

    void SpawnMeteorRain()
    {
        meteorRainSpawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;

        meteorRainSpawnPos.y += spawnHeight;

        if (Input.GetMouseButtonDown(0))
        {
            LaunchMeteorRain();
        }
    }

    void LaunchMeteorRain()
    {
        Instantiate(meteorRain, meteorRainSpawnPos, Quaternion.identity);
    }
}
