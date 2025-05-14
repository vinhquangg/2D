using System.Collections;
using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerStateMachine player;
    private bool hasDisappeared = false;

    public DeadState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void EnterState()
    {
        // Phát animation Dead
        var allZones = GameObject.FindObjectsOfType<SpawnZone>();
        foreach (var zone in allZones)
        {
            var box = zone.GetComponent<BoxCollider2D>();
            if (box != null && box.OverlapPoint(player.transform.position))
            {
                player.currentZoneID = zone.zoneID;
                break;
            }
        }

        player.anim.SetTrigger("isDead");
        player.PlayAnimation("Dead");

        player.rb.velocity = Vector2.zero;
        player.rb.bodyType = RigidbodyType2D.Static;
        //player.input.Disable(); 

        // Không vô hiệu hóa player ngay lập tức, để coroutine có thể chạy
        player.StartCoroutine(ReviveCoroutine());
    }

    public void ExitState()
    {
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        //player.input.Enable(); 
    }

    public void HandleInput()
    {
        // Không xử lý input trong trạng thái chết
    }

    public void PhysicsUpdate()
    {
        // Không cần xử lý physics update trong trạng thái chết
    }

    public void UpdateState()
    {
        // Cập nhật khi ở trạng thái Dead (nếu cần)
    }

    private IEnumerator ReviveCoroutine()
    {
        // Chờ đến khi animation Dead kết thúc
        while (!player.anim.GetCurrentAnimatorStateInfo(0).IsName("Dead") || player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            yield return null;
        }

        // Bắt đầu quá trình hồi sinh
        if (player != null)
        {
            player.SwitchState(new ReviveState(player));
        }
        else
        {
            Debug.LogError("Player is null during revive coroutine.");
        }
    }
}
