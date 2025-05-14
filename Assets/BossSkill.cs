using System;
using UnityEngine;

public class BossSkill 
{
    public string skillName;
    public float castTime;
    public string animationName;
    public Action onCast;

    public BossSkill(string skillName, float castTime, string animationName, Action onCast)
    {
        this.skillName = skillName;
        this.castTime = castTime;
        this.animationName = animationName;
        this.onCast = onCast;
    }
}
