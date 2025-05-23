using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float growSpeed = 1.5f;
    private float maxScale = 1.5f;
    private float blinkSpeed = 6f;
    private float damage = 10f;
    private float moveSpeed = 1.2f;

    private SpriteRenderer sr;
    private Vector3 moveDir;

    public void Initialize(Vector3 direction, float _maxScale, float _blinkSpeed, float _damage, float _moveSpeed)
    {
        moveDir = direction.normalized;
        maxScale = _maxScale;
        blinkSpeed = _blinkSpeed;
        damage = _damage;
        moveSpeed = _moveSpeed;
        transform.localScale = Vector3.zero;

        Destroy(gameObject, 10f);
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * maxScale, Time.deltaTime * growSpeed);

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat pc = other.GetComponent<PlayerCombat>();
            if (pc != null)
                pc.TakeDamage(damage);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.hit);
            Destroy(gameObject);
        }
    }
}
