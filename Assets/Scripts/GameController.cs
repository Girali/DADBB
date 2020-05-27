using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public GameObject camera;
    public MeteorRain meteorRain;
    public GameObject playerPrefab;

    public Transform spawnPointPlayer;
    public Transform spawnPointMaster;

    public Transform spawnMinion;
    public Transform spawnElite;
    public Transform spawnBoss;

    public GameObject[] mobs;

    PhotonView pv;

    public bool gameStarted = false;
    public float endGameTimer = 0f;

    float[] cdAbilitys = { 10, 20, 30, 50 };
    float[] nextTimeOk = { 0, 0, 0, 0 };

    public void Interact(int i)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (Time.time > nextTimeOk[i])
        {
            switch (i)
            {
                case 0:
                    StartCoroutine(CRT_SpawnMobs(0, 5 * players.Length, spawnMinion.transform));
                    GUI_Controller.Instance.StartAbility1();
                    break;

                case 1:
                    GUI_Controller.Instance.StartAbility2();
                    StartCoroutine(CRT_SpawnMobs(1, 2 * players.Length, spawnElite.transform));
                    break;

                case 2:
                    GUI_Controller.Instance.StartAbility3();
                    StartCoroutine(CRT_SpawnMobs(2, 1 * players.Length, spawnBoss.transform));
                    break;

                case 3:
                    GUI_Controller.Instance.StartAbility4();
                    StartCoroutine(CRT_SpawnMobs(0, 5, spawnMinion.transform));
                    StartCoroutine(CRT_SpawnMobs(1, 2, spawnElite.transform));
                    StartCoroutine(CRT_SpawnMobs(2, 1, spawnBoss.transform));
                    meteorRain.SpawnMeteorRain();
                    GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

                    foreach (GameObject go in towers)
                    {
                        go.GetComponent<Tower>().AddLife(-40);
                    }

                    foreach (GameObject go in players)
                    {
                        if(!go.GetComponent<PhotonView>().Owner.IsMasterClient)
                            go.GetComponent<PlayerController>().AddLife(-40);
                    }
                    break;

                default:
                    break;
            }

            nextTimeOk[i] = Time.time + cdAbilitys[i];
        }
    }

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

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (PhotonNetwork.IsMasterClient && gameStarted)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if(players.Length == 0)
                pv.RPC("RPC_WinMaster", RpcTarget.All);

            if(Time.time > endGameTimer)
                pv.RPC("RPC_WinPlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_WinPlayer()
    {
        GUI_Controller.Instance.TimerWin();
    }

    [PunRPC]
    void RPC_WinMaster()
    {
        GUI_Controller.Instance.PlayerDeathWin();
    }

    public void SpawnMinions()
    {
        Interact(0);
    }

    public void SpawnElites()
    {
        Interact(1);
    }
    public void SpawnBoss()
    {
        Interact(2);
    }
    public void SpawnMeteors()
    {
        Interact(3);
    }

    IEnumerator CRT_SpawnMobs(int index, int count, Transform spawn)
    {
        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate(mobs[index].name, spawn.position, spawn.rotation);
            yield return new WaitForSeconds(2f);
        }
    }

    private void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        endGameTimer = Time.time + (6 * 10);
        pv.RPC("RPC_StartGame", RpcTarget.All);
        for (int i = 0; i < 4; i++)
        {
            nextTimeOk[i] = Time.time + cdAbilitys[i];
        }
    }
    
    [PunRPC]
    public void RPC_StartGame()
    {
        gameStarted = true;
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerView>().StartGame();
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerBuilderMotor>().StartGame();
        GUI_Controller.Instance.StartPlayerAbility1();
        GUI_Controller.Instance.StartPlayerAbility2();
        GUI_Controller.Instance.StartPlayerAbility3();
        GUI_Controller.Instance.StartPlayerAbility4();
        GUI_Controller.Instance.StartAbility1();
        GUI_Controller.Instance.StartAbility2();
        GUI_Controller.Instance.StartAbility3();
        GUI_Controller.Instance.StartAbility4();
        GUI_Controller.Instance.StartedGame();
    }

}
