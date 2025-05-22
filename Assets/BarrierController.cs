using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public SpawnZone[] zonesToCheck;

    public GameObject barrierObject;

    private bool isCleared = false;

    private void Update()
    {
        if (isCleared) return;

        if (AllZonesCleared())
        {
            isCleared = true;
            if (barrierObject != null)
            {
                barrierObject.SetActive(false);
            }
        }
    }

    private bool AllZonesCleared()
    {
        foreach (var zone in zonesToCheck)
        {
            if (zone != null && !zone.IsZoneCleared())
                return false;
        }
        return true;
    }
}
