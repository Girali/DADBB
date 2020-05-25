using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;

    public int attack;
    public int health;
    public float attackSpeed;
    public float radiusAttack;
}
