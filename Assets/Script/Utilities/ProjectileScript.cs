using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 10;
    private float lifeTime = 3f;

    private void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(damage);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.hit);
                Destroy(gameObject);
            }
        }
    }

}
