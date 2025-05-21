using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayers;
    public SpriteRenderer spriteRenderer;
    public float hitDuration = 0.2f;
    public float invincibleTime = 0.25f;
    public float currentHealth { get; set; }
    public float currentEnergy { get; set; }
    public int currentSoul { get; set; }

    private PlayerHealth playerHealth;
    public PlayerEnergy playerEnergy { get; private set; }
    public PlayerSoul playerSoul { get; set; }
    public PlayerStateMachine playerState { get; set; }

    private bool isInvincible = false;
    public Dictionary<ItemType, float> itemCooldowns = new Dictionary<ItemType, float>();

    private void Start()
    {
        playerState = GetComponent<PlayerStateMachine>();
        playerHealth = GetComponent<PlayerHealth>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerSoul = GetComponent<PlayerSoul>();

        currentHealth = playerState.playerData.maxHealth;
        playerHealth.UpdateHealthBarPlayer(currentHealth, playerState.playerData.maxHealth);
        currentEnergy = playerEnergy.GetMaxEnergy();
        currentSoul = playerSoul.GetSoul();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TryUseItemInSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            TryUseItemInSlot(1);
    }

    private void TryUseItemInSlot(int slotIndex)
    {
        var slots = InventoryManager.Instance.inventorySlots;

        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogWarning("Invalid slot index");
            return;
        }

        var slot = slots[slotIndex];
        if (slot.item != null && slot.amount > 0)
        {
            var type = slot.item.itemType;


            if (itemCooldowns.TryGetValue(type, out float nextAvailableTime) && Time.time < nextAvailableTime)
            {
                float remaining = nextAvailableTime - Time.time;
                Debug.Log($"{type} vẫn đang hồi. Còn lại {remaining:F1} giây.");
                return;
            }


            UseItemEffect(slot.item);
            slot.amount--;
            InventoryUI ui = FindObjectOfType<InventoryUI>();
            if (slot.amount <= 0)
            {
                if (ui != null)
                {
                    ui.UpdateUI(); 
                }
            }

            itemCooldowns[type] = Time.time + slot.item.timeToUse;
            InventoryManager.Instance.OnInventoryUpdated?.Invoke();

            if (ui != null)
            {
                SlotUI slotUI = ui.GetSlotUI(slotIndex);
                if (slotUI != null)
                    slotUI.StartCooldown(slot.item.timeToUse);
            }

            else if (slot.item != null && slot.amount == 0)
            {
                Debug.Log($"{slot.item.itemType} đã hết (số lượng = 0), không thể sử dụng.");
            }
            else
            {
                Debug.Log($"Slot {slotIndex + 1} trống hoặc không chứa item hợp lệ.");
            }
        }
    }



    private void UseItemEffect(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.HealthPotion:
                currentHealth += item.amountRestored;
                currentHealth = Mathf.Min(currentHealth, playerState.playerData.maxHealth);
                playerHealth?.UpdateHealthBarPlayer(currentHealth, playerState.playerData.maxHealth);
                break;

            case ItemType.EnergyPotion:
                currentEnergy += item.amountRestored;
                currentEnergy = Mathf.Min(currentEnergy, playerEnergy.GetMaxEnergy());
                playerEnergy?.UpdateEnergySlider();
                break;
        }
    }

    public void OnAttackHit(float attackRange)
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(transform.localScale.x * attackRange, transform.localScale.y * attackRange);
        AttackHit(attackPosition, attackRange);
    }

    public void AttackHit(Vector2 attackPosition, float attackRange)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            MonsterCombat enemyScript = enemy.GetComponent<MonsterCombat>();
            if (enemyScript != null)
            {
                enemyScript.ReceiveDamage(playerState.playerData.attackDamage, transform.position);
                playerEnergy.AddEnergy(playerState.playerData.energyPerHit);
                playerEnergy.UpdateEnergySlider();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        playerHealth.UpdateHealthBarPlayer(currentHealth, playerState.playerData.maxHealth);

        StartCoroutine(BecomeInvincible());
        StartCoroutine(ChangeColorTemporarily(Color.red, hitDuration));

        CheckHealth();
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerState.SwitchState(new DeadState(playerState));
    }
}
