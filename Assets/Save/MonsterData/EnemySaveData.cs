using UnityEngine;



[System.Serializable]
public class EnemySaveData 
{

    public EnemyType type;
    public Vector3 position;
    public float health;
    public string currentState;

    public EnemySaveData(EnemyType type, Vector3 position, float health, string state)
    {
        this.type = type;
        this.position = position;
        this.health = health;
        this.currentState = state;
    }

    public EnemySaveData() { }
}
