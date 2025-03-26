using TMPro;
using UnityEngine;

public class DamageFloat : MonoBehaviour
{
    public TextMeshPro textMesh; // UI Text hiển thị số damage
    private float timeElapsed;
    private float duration = 1f; // Thời gian tồn tại của text
    private Color[] colors = new Color[]
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white
    };

    private Vector3 originalPosition;

    public void SetText(int damage)
    {
        textMesh.text = damage.ToString(); // Hiển thị số damage
        originalPosition = transform.position; // Lưu vị trí ban đầu
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        // Đổi màu mỗi 0.1 giây
        int colorIndex = Mathf.FloorToInt(Time.time * 10) % colors.Length;
        textMesh.color = colors[colorIndex];

        // Hiệu ứng rung
        float shakeAmount = 0.1f;
        transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
    }

    public void DestroyAfter(float time)
    {
        Destroy(gameObject, time);
    }
}
