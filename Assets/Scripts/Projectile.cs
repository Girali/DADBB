using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public void Init(float pSpeed, int pDamage)
    {
        speed = pSpeed;
        damage = pDamage;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyController>().AddLife(-damage);
                Explode();
            }
        }
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }
}
