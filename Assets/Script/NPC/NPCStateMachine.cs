using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public INPCState npcCurrentState { get; private set; }
    public NPCData npcData;
    public Animator animNPC { get; private set; }
    public Rigidbody2D rbNPC { get; private set; }
    public Dictionary<string, System.Func<INPCState>> stateFactory;

    private void Awake()
    {
        InitializeNull();
    }
    private void InitializeNull()
    {
        if (rbNPC == null) rbNPC = GetComponent<Rigidbody2D>();
        if(animNPC == null) animNPC = GetComponent<Animator>();
    }
}
