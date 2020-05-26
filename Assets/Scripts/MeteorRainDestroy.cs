using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeteorRainDestroy : MonoBehaviour
{
    public float lifeTime = 10f;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                {
                    Death();
                }
            }
        }
    }

    void Death()
    {
        pv.RPC("RPC_Death", pv.Owner);
    }

    [PunRPC]
    void RPC_Death()
    {
        PhotonNetwork.RemoveRPCs(pv);
        PhotonNetwork.Destroy(pv);
    }
}
