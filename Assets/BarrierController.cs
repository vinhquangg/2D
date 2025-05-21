using UnityEngine;

public class BarrierController : MonoBehaviour
{
    [Tooltip("Các zone cần clear để mở barrier")]
    public SpawnZone[] zonesToCheck;

    [Tooltip("Đối tượng thanh chắn (ví dụ tilemap hay sprite)")]
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
                Debug.Log("[Barrier] Đã mở thanh chắn vì tất cả zone đã được clear.");
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
