using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public TowerStats tower;

    private string towerNameText;
    private string descriptionText;

    private Image artworkImage;

    private string manaCost;
    private int attack;
    private int health;
    private int attackSpeed;
    private GameObject projectile;


    private int radiusProjectile;
    private int radiusDamageProjectile;

    public Transform headTransform;
    public Transform baseTransform;
    public Transform barel;

    private bool canShot = true;

    // Start is called before the first frame update
    void Start()
    {
        Init(tower);
    }

    void Init(TowerStats towerStats)
    {
        projectile = towerStats.projectile;
        attackSpeed = towerStats.attackSpeed;
        towerNameText = towerStats.towerName;
        descriptionText = towerStats.description;

        manaCost = towerStats.manaCost.ToString();
        attack = towerStats.attack;
        health = towerStats.health;

        radiusProjectile = towerStats.radiusProjectile;
        radiusDamageProjectile = towerStats.radiusDamageProjectile;

        if (towerStats.artwork)
        {
            artworkImage.sprite = towerStats.artwork;
        }
    }

    void Update()
    {
        float distanceToEnemy = 0;
        GameObject nearestEnemy = null;
        float singleStep = 1f * Time.deltaTime;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
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
    
    IEnumerator ShotCoroutine()
    {
        yield return new WaitForSeconds(attackSpeed);
        canShot = true;
    }

    void Fire()
    {
        GameObject p = Instantiate(projectile, barel.position, barel.rotation);
        p.GetComponent<Projectile>().Init(attackSpeed, attack, radiusDamageProjectile, radiusProjectile);
        canShot = false;
    }
}

