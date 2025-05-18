using UnityEngine;
[CreateAssetMenu(fileName = "NewMonstersData", menuName = "Game Data/Monsters Data")]

public class MonsterData : ScriptableObject
{
    [Header("Monsters Stats")]
    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public float attackCooldown = 0.25f;
    public int soulDrop = 5;
    [Space(10)]

    [Header("Attack Settings")]
    //public int attackDamage = 10;
    public int attackDamageToPlayer = 10;
    public float attackRange = 0.5f;
    public float attackMonsterRange = 0.5f;
}
