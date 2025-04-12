using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float attackCooldown = 0.25f;
    public float energyPerHit = 2f;
    [Space(10)]

    [Header("Attack Settings")]
    public int attackDamage = 10;
    public float attackRange = 0.5f;
    public float comboResetTime = 0.5f;
    public GameObject GameObject;
}
