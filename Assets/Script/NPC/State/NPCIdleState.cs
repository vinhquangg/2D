using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : INPCState
{
    public NPCStateMachine npcStateMachine;
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

    }
}
