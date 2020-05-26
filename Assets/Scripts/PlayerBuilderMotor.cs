using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBuilderMotor : MonoBehaviour
{
    PhotonView pv;
    public GameObject[] towers;

    float[] cdAbilitys = { 10, 30, 50, 60 };
    float[] nextTimeOk = { 0, 0, 0, 0 };

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
            nextTimeOk[i] = Time.time + cdAbilitys[i];
        }
    }

    public void SpawnTowerAt(Vector3 pos, int i)
    {
        if (Time.time > nextTimeOk[i])
        {
            switch (i)
            {
                case 0:
                    GUI_Controller.Instance.StartPlayerAbility1();
                    break;

                case 1:
                    GUI_Controller.Instance.StartPlayerAbility2();
                    break;

                case 2:
                    GUI_Controller.Instance.StartPlayerAbility3();
                    break;

                case 3:
                    GUI_Controller.Instance.StartPlayerAbility4();
                    break;

                default:
                    break;
            }

            nextTimeOk[i] = Time.time + cdAbilitys[i];
            PhotonNetwork.Instantiate(towers[i].name, pos, Quaternion.identity);
        }
    }
}
