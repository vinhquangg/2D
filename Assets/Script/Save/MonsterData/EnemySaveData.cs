using System;
using UnityEngine;

[Serializable]
public class EnemySaveData
{
    public int enemyID;             
    public EnemyType type;             
    public Vector3 position;           
    public float health;               
    public string currentState;        
    public Vector3 patrolA;            
    public Vector3 patrolB;            
    public string zoneID;              

    public EnemySaveData(
        int enemyID,
        EnemyType type,
        Vector3 position,
        float health,
        string currentState,
        Vector3 patrolA,
        Vector3 patrolB,
        string zoneID
    )
    {
        this.enemyID = enemyID;
        this.type = type;
        this.position = position;
        this.health = health;
        this.currentState = currentState;
        this.patrolA = patrolA;
        this.patrolB = patrolB;
        this.zoneID = zoneID;
    }
}
