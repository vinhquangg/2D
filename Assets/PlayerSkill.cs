using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject blastWavePrefab; // Prefab của sóng xung kích

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastBlastWave();
        }
    }

    void CastBlastWave()
    {
        Instantiate(blastWavePrefab, transform.position, Quaternion.identity);
    }
}
