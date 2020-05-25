using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubZeroBomb : Projectile
{
    Projectile projectile;

    void Start()
    {
        projectile = gameObject.AddComponent<Projectile>();
    }

    void Update()
    {
        
    }

    public override void Explode()
    {
        base.Explode();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, projectile.radius);
        int i = 0;
        while(i < hitColliders.Length)
        {
            Debug.Log("Explode");
            if(hitColliders[i].tag == "Enemy")
            {
                Debug.Log(gameObject);
                hitColliders[i].SendMessage("AddDamage");
                //hitColliders[i].GetComponent < "Enemy" >.SlowDown();
            }
            i++;
        }
    }
}
