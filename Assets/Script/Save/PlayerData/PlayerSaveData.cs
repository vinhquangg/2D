using UnityEngine; 

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;  
    public float health;
    public string currentState;
    public float energy;

   
    public PlayerSaveData(Vector3 position, float health, string currentState, float energy)
    {
        this.position = position;  
        this.health = health;
        this.currentState = currentState;
        this.energy = energy;
    }
<<<<<<< HEAD:Assets/Script/Save/PlayerData/PlayerSaveData.cs

    //public PlayerSaveData() { }
=======
>>>>>>> parent of d29f92e (SaveGame):Assets/Save/PlayerData/PlayerSaveData.cs
}
