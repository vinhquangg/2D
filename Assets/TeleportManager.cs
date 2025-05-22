using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform teleportTarget;

    private GameObject arrowInstance;

    void Start()
    {
        if (arrowPrefab != null && teleportTarget != null)
        {
            arrowInstance = Instantiate(arrowPrefab);

            TeleportArrow arrowScript = arrowInstance.GetComponent<TeleportArrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTeleportTarget(teleportTarget);
            }

            arrowInstance.SetActive(false); 
        }
    }

    public void ShowArrow(bool show)
    {
        if (arrowInstance != null)
            arrowInstance.SetActive(show);
    }
}
