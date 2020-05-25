using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Turret")]
public class TowerStats : ScriptableObject
{
    public string towerName;
    public string description;

    public Sprite artwork;

    public int manaCost;
    public int attack;
    public int health;
    public int attackSpeed;

    public int radiusProjectile;
    public int radiusDamageProjectile;

    public GameObject projectile;

    public void Print()
    {
        Debug.Log(towerName + ": " + description + " ManaCost : " + manaCost);
    }
}
