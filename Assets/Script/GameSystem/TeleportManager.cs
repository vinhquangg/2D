using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform teleportTarget;

    private GameObject arrowInstanceTeleport;
    private GameObject arrowInstanceNPC;

    void Start()
    {
        // Arrow chỉ điểm teleport (offset trên đầu)
        if (arrowPrefab != null && teleportTarget != null)
        {
            arrowInstanceTeleport = Instantiate(arrowPrefab);
            TeleportArrow arrowScript = arrowInstanceTeleport.GetComponent<TeleportArrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTeleportTarget(teleportTarget);
                arrowScript.SetOffset(new Vector3(0, 2.5f, 0)); 
            }
            arrowInstanceTeleport.SetActive(false);
        }




        // Arrow chỉ NPC (offset bên trái)
        GameObject npcObj = GameObject.Find("NPC-Store");
        if (arrowPrefab != null && npcObj != null)
        {
            arrowInstanceNPC = Instantiate(arrowPrefab);
            TeleportArrow arrowScript = arrowInstanceNPC.GetComponent<TeleportArrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTeleportTarget(npcObj.transform);
                arrowScript.SetOffset(new Vector3(-1.5f, 0.5f, 0)); 
            }
            arrowInstanceNPC.SetActive(false);
        }
    }

    public void ShowTeleportArrow(bool show)
    {
        if (arrowInstanceTeleport != null)
            arrowInstanceTeleport.SetActive(show);
    }

    public void ShowNPCArrow(bool show)
    {
        if (arrowInstanceNPC != null)
            arrowInstanceNPC.SetActive(show);
    }
}
