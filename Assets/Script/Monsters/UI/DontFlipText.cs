using UnityEngine;

public class DontFlipText : MonoBehaviour
{
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;

        float parentScaleX = transform.parent.lossyScale.x > 0 ? 1f : -1f;

        transform.localScale = new Vector3(
            Mathf.Abs(initialScale.x) * parentScaleX,
            initialScale.y,
            initialScale.z
        );
    }
}
