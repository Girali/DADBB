using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyController : MonoBehaviour
{
    PhotonView pv;
    EnemyView enemyView;
    NavMeshAgent navMeshAgent;
    int currentLife;
    int maxLife = 80;
    float speed = 5f;
    int dmg = 10;
    float attackSpeed = 1f;
    bool isAttacking = false;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        enemyView = GetComponent<EnemyView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(int i)
    {
        currentLife = maxLife;
        enemyView.Init(currentLife);
    }

    private void Update()
    {

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            float minDistance = float.MaxValue;
            GameObject closestPlayer = null;

            foreach (GameObject p in players)
            {
                if (Vector3.Distance(p.transform.position, transform.position) < minDistance)
                {
                    closestPlayer = p;
                    minDistance = Vector3.Distance(p.transform.position, transform.position);
                }
            }

            navMeshAgent.SetDestination(closestPlayer.transform.position);

        if(!isAttacking)
            if (Vector3.Distance(closestPlayer.transform.position, transform.position) < 1.5f)
            {
                StartCoroutine(Attack(closestPlayer));
            }
        
    }

    IEnumerator Attack(GameObject other)
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackSpeed);
        if(Vector3.Distance(other.transform.position, transform.position) < 1.5f)
        {
            if (other.GetComponent<PlayerController>())
            {
                other.GetComponent<PlayerController>().AddLife(-dmg);
                //run animation
            }
        }
        isAttacking = false;
    }

    public void AddLife(int i)
    {
        currentLife += i;

        if (currentLife < 0)
            currentLife = 0;


        if (currentLife > maxLife)
            currentLife = maxLife;

        enemyView.UpdateLife(currentLife, maxLife);

        pv.RPC("RPC_setLife", RpcTarget.Others, currentLife);

        if (currentLife == 0)
            Death();
    }

    [PunRPC]
    public void RPC_setLife(int life)
    {
        enemyView.UpdateLife(life, maxLife);
        currentLife = life;
    }

    void Death()
    {
        PhotonNetwork.RemoveRPCs(pv);
        PhotonNetwork.Destroy(pv);
    }

}
