using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrolState : INPCState
{
    private NPCStateMachine npcStateMachine;
    //private Transform currentPoint;
    //private GameObject pointA;
    //private GameObject pointB;

    public NPCPatrolState(NPCStateMachine npcStateMachine)
    {
        this.npcStateMachine = npcStateMachine;
        //this.pointA = npcStateMachine.npc.pointA;
        //this.pointB = npcStateMachine.npc.pointB;
        //currentPoint = pointA.transform;
    }

    public void EnterState()
    {
        npcStateMachine.animNPC.SetBool("isWalk", true);
        npcStateMachine.animNPC.Play("Walk");
    }

    public void ExitState()
    {
        npcStateMachine.animNPC.SetBool("isWalk", false);
    }

    public void UpdateState()
    {
        Patrol();
    }

    private void Patrol()
    {
        npcStateMachine.npc.transform.position = Vector2.MoveTowards(
            npcStateMachine.npc.transform.position,
             npcStateMachine.npc.currentPoint.position,
             npcStateMachine.npc.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(npcStateMachine.npc.transform.position, npcStateMachine.npc.currentPoint.position) < 0.1f)
        {

            npcStateMachine.SwitchState(new NPCIdleState(npcStateMachine));
        }

        npcStateMachine.npc.Flip(npcStateMachine.npc.currentPoint);
    }

}
