using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerView : MonoBehaviour
{
    public Timer timer;
    public Text metaLife;
    public Text diagLife;
    public MeshRenderer meshRenderer;
    public bool offline = false;
    PhotonView pv;

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
                Destroy(diagLife.transform.parent.gameObject);
            }

            if (pv.Owner.IsMasterClient)
            {
                Destroy(diagLife.transform.parent.gameObject);
                Destroy(metaLife.transform.parent.gameObject);
            }
        }
    }

    public void UpdateLife(int life)
    {
        if (metaLife)
            metaLife.text = life.ToString();

        if (diagLife)
            diagLife.text = life.ToString();
    }

    public void StartGame()
    {
        timer.StartTimer();
    }
}
