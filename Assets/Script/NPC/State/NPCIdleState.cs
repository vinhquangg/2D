using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : INPCState
{
    public NPCStateMachine npcStateMachine;
    private float idleDuration = 3f;
    private float idleTimer;
    public NPCIdleState(NPCStateMachine npcStateMachine)
    {
        this.npcStateMachine = npcStateMachine;
    }
    public void EnterState()
    {
        npcStateMachine.animNPC.Play("Idle");
    }

    public void ExitState()
    {
        npcStateMachine.animNPC.StopPlayback();
    }

    public void UpdateState()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            npcStateMachine.npc.currentPoint = (npcStateMachine.npc.currentPoint == npcStateMachine.npc.pointA.transform) ?
                                        npcStateMachine.npc.pointB.transform :
                                        npcStateMachine.npc.pointA.transform;
            npcStateMachine.SwitchState(new NPCPatrolState(npcStateMachine));
        }
    }
}
