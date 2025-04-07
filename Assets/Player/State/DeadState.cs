using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerStateMachine player;
    private bool isDead = false;
    public  DeadState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void EnterState()
    {
        Debug.Log("Player has died.");
        player.anim.SetTrigger("isDead"); // <- Animator cần có trigger "Dead"
        player.PlayAnimation("Dead");
        //player.gameObject.SetActive(false);
        //player.rigidbody.velocity = Vector3.zero;
        //player.rigidbody.isKinematic = true; // <- Ngăn di chuyển sau khi chết
        //player.input.Disable(); // <- Tắt input nếu bạn dùng InputAction
    }

    public void ExitState()
    {
        player.rb.isKinematic = false;
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
