using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerStateMachine player;
    //private bool isDead = false;
    private bool hasDisappeared = false;
    public  DeadState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void EnterState()
    {
        //Debug.Log("Player has died.");
        //player.anim.SetTrigger("isDead"); 
        player.anim.SetTrigger("Dead"); 
        player.PlayAnimation("Dead");

        //hasDisappeared = false;
        //isDead  = true;
        //player.gameObject.SetActive(false);
        //player.rigidbody.velocity = Vector3.zero;
        //player.rigidbody.isKinematic = true; 
        //player.input.Disable(); // 
    }

    public void ExitState()
    {
        //player.rb.isKinematic = false;
        //player.anim.SetBool("isDead", false);

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


        if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 0.8f)
        {
            //player.anim.SetBool("isDead", false);
           // player.anim.SetTrigger("Dead");
            hasDisappeared = true;
            if (hasDisappeared)
            {
                player.gameObject.SetActive(false);
            }
        }
    }

}
