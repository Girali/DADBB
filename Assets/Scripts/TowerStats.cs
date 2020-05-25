using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Turret")]
public class TowerStats : ScriptableObject
{
    public string towerName;

    public int attack;
    public int health;
    public float attackSpeed;
    public float rotationSpeed;

    public float bulletSpeed;

    public GameObject projectile;
}
