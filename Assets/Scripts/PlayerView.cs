using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerView : MonoBehaviour
{
    public Text diagLife;
    public bool offline = false;
    PhotonView pv;
    public GameObject renderObject;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if(offline)
        {
            Destroy(diagLife.transform.parent.gameObject);
        }
        else
        {
            if (pv.IsMine)
            {
                Destroy(renderObject);
                Destroy(diagLife.transform.parent.gameObject);
            }

            if (pv.Owner.IsMasterClient)
            {
                Destroy(renderObject);
                Destroy(diagLife.transform.parent.gameObject);
            }
        }
    }

    public void UpdateLife(int life)
    {
        GUI_Controller.Instance.life.text = life.ToString();

        if (diagLife)
            diagLife.text = life.ToString();
    }

    public void StartGame()
    {
        GUI_Controller.Instance.timer.StartTimer();
    }
}
