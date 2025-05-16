using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewBossSkill", menuName = "Boss/BossSkill")]
public class BossSkillSO : ScriptableObject
{
    public string animationName = "PoinsonsLord_CastSkill";
    public string skillName;
    public float castTime;
    public int skillPhase;
    public float specialAbilityCD;
}
