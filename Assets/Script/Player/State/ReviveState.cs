using UnityEngine;

public class ReviveState : IPlayerState
{
    private PlayerStateMachine player;
    private bool hasRevived = false;
    [HideInInspector]
    public Vector3 spawnPositionOutsideZone;
    public ReviveState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void EnterState()
    {
        if (hasRevived) return;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.revive);
        var allZones = GameObject.FindObjectsOfType<SpawnZone>();
        foreach (var zone in allZones)
        {
            if (zone.zoneID == player.currentZoneID)
            {
                spawnPositionOutsideZone = zone.GetEntryPointOutsideZone();
                break;
            }
        }

        if (spawnPositionOutsideZone == Vector3.zero)
        {
            Debug.LogWarning($"[ReviveState] Không tìm thấy zone với ID {player.currentZoneID}. Dùng (0,0,0)");
        }

        player.rb.velocity = Vector2.zero;
        player.rb.bodyType = RigidbodyType2D.Dynamic;

        hasRevived = true;

        player.gameObject.SetActive(true);
        player.transform.position = spawnPositionOutsideZone;

        player.SwitchState(new IdleState(player));
        player.GetComponent<PlayerHealth>()?.FullRestore(player.playerData.maxHealth);
        player.GetComponent<PlayerEnergy>()?.UpdateEnergySlider();
    }



    public void ExitState()
    {
        
    }

    public void HandleInput()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void UpdateState()
    {
    }
}
