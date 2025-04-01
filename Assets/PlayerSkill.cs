using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject blastWavePrefab; 
    private float delayBeforeNextCast = 1f;
    private bool nextCastTime = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && nextCastTime)
        {
            CastBlastWave(); 
        }
    }

    void CastBlastWave()
    {
        nextCastTime = false; 
        Invoke(nameof(EnableSkill), delayBeforeNextCast); 
        Instantiate(blastWavePrefab, transform.position, Quaternion.identity);
    }

    void EnableSkill()
    {
        nextCastTime = true; 
    }
}
