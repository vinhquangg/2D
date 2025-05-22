using UnityEngine;

public class TeleportArrow : MonoBehaviour
{
    private Transform player;
    private Transform teleportTarget;
    private Vector3 offset = new Vector3(0, 2.5f, 0); 

    public void SetTeleportTarget(Transform target)
    {
        teleportTarget = target;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || teleportTarget == null) return;

        transform.position = player.position + offset;

        Vector3 dir = transform.position - teleportTarget.position ;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
