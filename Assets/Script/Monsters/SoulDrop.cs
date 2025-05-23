using UnityEngine;

public class SoulDrop : MonoBehaviour
{
    private int soulAmount;

    public void SetSoulAmount(int amount)
    {
        soulAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerSoul playerSoul = collision.GetComponent<PlayerSoul>();
            if (playerSoul != null)
            {
                playerSoul.AddSoul(soulAmount);
                Destroy(gameObject);
            }
        }
    }
}
