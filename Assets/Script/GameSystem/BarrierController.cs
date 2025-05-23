using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public SpawnZone[] zonesToCheck;
    public GameObject barrierObject;

    [Header("Tham chiếu Teleport")]
    public TeleportManager teleportManager;  // kéo vào trong Inspector

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
                // Khi barrier tắt, bật mũi tên
                if (teleportManager != null)
                    teleportManager.ShowTeleportArrow(true);
                    teleportManager.ShowNPCArrow(true);
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
