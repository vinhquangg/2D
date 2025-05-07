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

        var currentZone = GameObject.FindObjectOfType<SpawnZone>();
        if (currentZone != null && currentZone.zoneID == player.currentZoneID)
        {
            spawnPositionOutsideZone = currentZone.GetEntryPointOutsideZone(); 
        }

        player.rb.velocity = Vector2.zero;
        player.rb.bodyType = RigidbodyType2D.Dynamic;

        hasRevived = true;

        player.gameObject.SetActive(true);
        player.transform.position = spawnPositionOutsideZone; 

        player.SwitchState(new IdleState(player));

        PlayerSaveData defaultData = PlayerManager.Instance.GetDefaultPlayer();
        PlayerManager.Instance.LoadPlayerData(defaultData, false);
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
