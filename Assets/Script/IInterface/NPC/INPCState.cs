using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCState
{
    void EnterState();
    void UpdateState();
    void ExitState();   
}
