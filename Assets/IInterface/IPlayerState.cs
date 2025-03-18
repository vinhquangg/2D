using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState();
    void HandleInput();
    void UpdateState();
    void PhysicsUpdate();
    void ExitState();
}
