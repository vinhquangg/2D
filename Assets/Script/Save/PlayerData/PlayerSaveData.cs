using UnityEngine; 

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;  
    public float health;
    public string currentState;
    public float energy;
    public int soul;
    public string currentSceneName;
   
    public PlayerSaveData(Vector3 position, float health, string currentState, float energy,string sceneName, int soul)
    {
        this.position = position;  
        this.health = health;
        this.currentState = currentState;
        this.energy = energy;
        this.soul = soul;
        this.currentSceneName = sceneName;
    }

    public PlayerSaveData() 
    {}
}
