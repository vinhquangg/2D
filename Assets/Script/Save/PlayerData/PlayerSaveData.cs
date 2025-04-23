using UnityEngine; 

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 position;  
    public float health;
    public string currentState;
    public float energy;
    public string currentSceneName;
   
    public PlayerSaveData(Vector3 position, float health, string currentState, float energy,string sceneName)
    {
        this.position = position;  
        this.health = health;
        this.currentState = currentState;
        this.energy = energy;
        this.currentSceneName = sceneName;
    }

    public PlayerSaveData() 
    {
        position = Vector3.zero;
        health = 100f; // Giá trị mặc định cho máu
        currentState = "IdleState"; // Trạng thái mặc định
        energy = 100f; // Giá trị mặc định cho năng lượng
        currentSceneName = "SampleScene"; // Tên cảnh mặc định
    }
}
