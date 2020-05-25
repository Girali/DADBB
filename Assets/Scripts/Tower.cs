using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Tower : MonoBehaviour
{
    public TowerStats towerStats;
    PhotonView pv;

    private string towerNameText;
    private string descriptionText;

    private int currentLife;
    private int maxLife;
    private GameObject projectile;


    public Transform headTransform;
    public Transform barel;

    private bool canShot = true;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        currentLife = towerStats.health;
        maxLife = currentLife;
        Init(maxLife);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float distanceToEnemy = 0;

            GameObject nearestEnemy = null;
            float singleStep = 1f * Time.deltaTime * towerStats.rotationSpeed;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (distanceToEnemy > Vector3.Distance(enemy.transform.position, headTransform.position) || distanceToEnemy == 0)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(headTransform.position, enemy.transform.position - headTransform.position);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                    {
                        if (hit.transform == enemy.transform)
                        {
                            distanceToEnemy = Vector3.Distance(enemy.transform.position, headTransform.position);
                            nearestEnemy = enemy;
                            if (canShot == true)
                            {
                                Fire();
                                StartCoroutine(ShotCoroutine());
                            }

                        }
                    }
                }
            }

            if (nearestEnemy != null)
            {
                Vector3 targetDirection = headTransform.position - nearestEnemy.transform.position;
                Vector3 currentDirection = headTransform.forward;

                Vector3 newDirection = Vector3.RotateTowards(currentDirection, targetDirection, singleStep, 1.0f);
                Debug.DrawRay(headTransform.position, nearestEnemy.transform.position - headTransform.position);
                headTransform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }

    public void AddLife(int i)
    {
        currentLife += i;

        if (currentLife < 0)
            currentLife = 0;


        if (currentLife > maxLife)
            currentLife = maxLife;

        UpdateLife(currentLife, maxLife);

        pv.RPC("RPC_setLife", RpcTarget.Others, currentLife);

        if (currentLife == 0)
            Death();
    }

    [PunRPC]
    public void RPC_setLife(int life)
    {
        UpdateLife(life, maxLife);
        currentLife = life;
    }

    public Image lifeBar;
    public Text lifeText;

    public void Init(int life)
    {
        lifeBar.fillAmount = 1f;
        lifeText.text = life.ToString();
    }

    public void UpdateLife(int life, int maxLife)
    {
        lifeBar.fillAmount = Mathf.Lerp(0, 110, life / maxLife);
        lifeText.text = life.ToString();
    }

    void Death()
    {
        PhotonNetwork.RemoveRPCs(pv);
        PhotonNetwork.Destroy(pv);
    }

    IEnumerator ShotCoroutine()
    {
        yield return new WaitForSeconds(towerStats.attackSpeed / 60f);
        canShot = true;
    }

    void Fire()
    {
        GameObject p = Instantiate(projectile, barel.position, barel.rotation);
        p.GetComponent<Projectile>().Init(towerStats.bulletSpeed, towerStats.attack);
        canShot = false;
    }
}

