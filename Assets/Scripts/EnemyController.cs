using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats;
    PhotonView pv;
    EnemyView enemyView;
    NavMeshAgent navMeshAgent;
    int currentLife;
    int maxLife;
    bool isAttacking = false;
    GameObject currentTargetTower = null;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        enemyView = GetComponent<EnemyView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        currentLife = enemyStats.health;
        maxLife = currentLife;
        enemyView.Init(currentLife);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (currentTargetTower == null)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

                float minDistancePLayer = float.MaxValue;
                GameObject closestPlayer = null;

                float minDistanceTower = float.MaxValue;
                GameObject closestTower = null;

                foreach (GameObject p in players)
                {
                    if (Vector3.Distance(p.transform.position, transform.position) < minDistancePLayer)
                    {
                        closestPlayer = p;
                        minDistancePLayer = Vector3.Distance(p.transform.position, transform.position);
                    }
                }

                foreach (GameObject p in towers)
                {
                    if (Vector3.Distance(p.transform.position, transform.position) < minDistanceTower)
                    {
                        closestTower = p;
                        minDistanceTower = Vector3.Distance(p.transform.position, transform.position);
                    }
                }

                if (currentTargetTower == null)
                {
                    if (Vector3.Distance(closestTower.transform.position, transform.position) < enemyStats.radiusAttack)
                    {
                        currentTargetTower = closestTower;
                        return;
                    }
                }

                navMeshAgent.SetDestination(closestPlayer.transform.position);

                if (!isAttacking)
                {
                    if (Vector3.Distance(closestPlayer.transform.position, transform.position) < enemyStats.radiusAttack)
                    {
                        StartCoroutine(Attack(closestPlayer));
                    }
                }
            }
            else
            {
                navMeshAgent.SetDestination(currentTargetTower.transform.position);
                if (!isAttacking)
                {
                    if (Vector3.Distance(currentTargetTower.transform.position, transform.position) < enemyStats.radiusAttack)
                    {
                        StartCoroutine(Attack(currentTargetTower));
                    }
                }
            }
        }
    }

    IEnumerator Attack(GameObject other)
    {
        isAttacking = true;
        yield return new WaitForSeconds(enemyStats.attackSpeed / 60f);
        if(Vector3.Distance(other.transform.position, transform.position) < enemyStats.radiusAttack)
        {
            if (other.GetComponent<PlayerController>())
            {
                other.GetComponent<PlayerController>().AddLife(-enemyStats.attack);
            }

            if (other.GetComponent<Tower>())
            {
                other.GetComponent<Tower>().AddLife(-enemyStats.attack);
            }
        }

        if (other.tag == "Tower")
            currentTargetTower = null;

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
