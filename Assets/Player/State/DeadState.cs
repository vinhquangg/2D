using System.Collections;
using System.Collections.Generic;
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
        player.anim.SetTrigger("isDead");
        player.PlayAnimation("Dead");

        player.rb.velocity = Vector2.zero; 
        player.rb.bodyType= RigidbodyType2D.Static; 
        //player.input.Disable(); 
    }

    public void ExitState()
    {

        player.rb.bodyType = RigidbodyType2D.Dynamic;
        //player.input.Enable(); 
    }

    public void HandleInput()
    {
       
    }

    public void PhysicsUpdate()
    {
       
    }

    public void UpdateState()
    {
 
        if (hasDisappeared) return;

        
        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);

        
        if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 0.7f)
        {

            hasDisappeared = true;
            player.gameObject.SetActive(false);
        }
    }
}
