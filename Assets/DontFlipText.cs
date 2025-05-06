using UnityEngine;

public class DontFlipText : MonoBehaviour
{
    private Vector3 initialScale;

    private void Start()
    {
        // Lưu giá trị scale ban đầu
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // Luôn giữ hướng quay của text đúng (không bị flip)
        transform.rotation = Quaternion.identity;

        // Giữ lại scale gốc để tránh bị ảnh hưởng bởi flip cha
        transform.localScale = new Vector3(
            Mathf.Abs(initialScale.x),
            initialScale.y,
            initialScale.z
        );
    }
}
