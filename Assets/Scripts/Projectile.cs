using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed;
    public int damage;
    public int radiusDamage;
    public int radius;

    public void Init(int pSpeed, int pDamage, int pRadiusDamage, int pRadius)
    {
        speed = pSpeed;
        damage = pDamage;
        radiusDamage = pRadiusDamage;
        radius = pRadius;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Explode();
        }
    }

    public virtual void Explode()
    {
        Destroy(gameObject);
    }
}
