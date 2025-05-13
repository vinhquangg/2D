using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBossData", menuName = "Game/BossData")]
public class BossData : ScriptableObject
{
    public string bossName;
    public float hitDuration;
    public float maxHealth;
    public float attackDamage;
    public float attackRange;
    public float moveSpeed;
    public float knockbackForce;
    public float specialAbilityCD;
    public string phaseTwoSpecialAbilityName;
}
