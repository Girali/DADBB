using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public Transform target;
    bool inited = false;
    PhotonView pv;


    public void Init(float pSpeed, int pDamage, Transform pTarget)
    {
        pv = GetComponent<PhotonView>();
        speed = pSpeed;
        damage = pDamage;
        inited = true;
        target = pTarget;
    }

    private void Update()
    {
        if (inited)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (target != null)
                {
                    transform.position += transform.forward * Time.deltaTime * speed;
                    transform.LookAt(target);
                }
                else
                {
                    Explode();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.tag == target.tag)
            {
                if(other.GetComponent<EnemyController>())
                    other.GetComponent<EnemyController>().AddLife(-damage);

                if (other.GetComponent<PlayerController>())
                    other.GetComponent<PlayerController>().AddLife(-damage);
                Explode();
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


    public virtual void Explode()
    {
        Death();
    }
}
