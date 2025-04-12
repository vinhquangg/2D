using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterCombat
{
    void Attack();
    void StopAttack();
    bool IsAttacking { get; }
}
