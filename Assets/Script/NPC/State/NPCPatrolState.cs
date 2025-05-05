using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrolState : INPCState
{
    private NPCStateMachine npcStateMachine;
    private Transform currentPoint;
    private GameObject pointA;
    private GameObject pointB;

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
        throw new System.NotImplementedException();
    }

}
