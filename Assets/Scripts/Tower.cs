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
            if (headTransform != null)
            {
                float distanceToEnemy = 0;

                GameObject nearestEnemy = null;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(towerStats.target);

                foreach (GameObject enemy in enemies)
                {
                    if (distanceToEnemy > Vector3.Distance(enemy.transform.position, headTransform.position) || distanceToEnemy == 0)
                    {
                        RaycastHit hit;
                        Ray ray = new Ray(headTransform.position, enemy.transform.position - headTransform.position);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
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
                    Quaternion rot = Quaternion.LookRotation(nearestEnemy.transform.position - headTransform.position, Vector3.up);
                    headTransform.rotation = Quaternion.Lerp(headTransform.rotation, rot, 0.5f);
                }
                else
                {
                    headTransform.rotation = Quaternion.Lerp(headTransform.rotation, Quaternion.identity, 0.5f);
                }
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
        lifeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 110);
        lifeText.text = life.ToString();
    }

    public void UpdateLife(int life, int maxLife)
    {
        lifeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 110, life / (float)maxLife));
        lifeText.text = life.ToString();
    }

    void Death()
    {
        pv.RPC("RPC_Death",pv.Owner);
    }

    [PunRPC]
    void RPC_Death()
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
        GameObject p = PhotonNetwork.Instantiate(towerStats.projectile.name, barel.position, barel.rotation);
        p.GetComponent<Projectile>().Init(towerStats.bulletSpeed, towerStats.attack);
        canShot = false;
    }
}

