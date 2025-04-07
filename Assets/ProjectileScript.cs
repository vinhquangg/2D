using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 10;
    private float lifeTime = 5f;

    private void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();
    //        if (playerCombat != null)
    //        {
    //            playerCombat.TakeDamage(damage);
    //            Destroy(gameObject);
    //        }
    //    }

    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }


    //private void OntriggerEnter2D(Collider2D collision)
    //{
        
    //}
}
