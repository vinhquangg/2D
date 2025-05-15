using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float impactY = 0f;
    public Vector3 startScale = new Vector3(0.2f, 0.2f, 1f);
    public Vector3 endScale = new Vector3(1f, 1f, 1f);

    private bool hasImpacted = false;
    private Vector3 targetPosition;

    void OnEnable()
    {
        transform.localScale = startScale;
        hasImpacted = false;
    }

    public void Initialize(Vector3 targetPos, float groundY)
    {
        targetPosition = targetPos;
        impactY = groundY;
        transform.position = new Vector3(targetPos.x, targetPos.y + 10f, 0);
        Debug.Log($"Meteor initialized at {transform.position}, falling to Y={impactY}");
    }


    void Update()
    {
        if (hasImpacted) return;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, impactY, 0), fallSpeed * Time.deltaTime);

        if (transform.position.y <= impactY + 0.1f)
        {
            Debug.Log("Meteor has impacted!");
            hasImpacted = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
