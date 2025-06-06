﻿using System.Collections;
using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerStateMachine player;


    public DeadState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void EnterState()
    {

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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.death);
        player.StartCoroutine(ReviveCoroutine());
    }

    public void ExitState()
    {
        player.rb.bodyType = RigidbodyType2D.Dynamic;

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

    private IEnumerator ReviveCoroutine()
    {
        while (!player.anim.GetCurrentAnimatorStateInfo(0).IsName("Dead") || player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            yield return null;
        }

        if (player != null)
        {
            player.SwitchState(new ReviveState(player));
        }
        //else
        //{
        //    Debug.LogError("Player is null during revive coroutine.");
        //}
    }
}
