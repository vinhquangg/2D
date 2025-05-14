using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    public List<BossSkill> skills = new List<BossSkill>();
    private int currentSkillIndex = 0;  

    private Animator animator;
    private BossSummoner bossSummoner;


    void Start()
    {
        animator = GetComponent<Animator>();
        bossSummoner = GetComponent<BossSummoner>();
        InitializedSkills();
    }

    private void InitializedSkills()
    {
        skills.Add(new BossSkill("SummonMinions", 1f, "PoinsonsLord_CastSkill", SummonEnemies));
    }

    public void UseNextSkill()
    {
        if (skills.Count > 0)
        {
            BossSkill skill = skills[currentSkillIndex];
            StartCoroutine(CastSkill(skill));  
            currentSkillIndex = (currentSkillIndex + 1) % skills.Count;  
        }
    }

    private IEnumerator CastSkill(BossSkill skill)
    {
        animator.Play(skill.animationName); 
        yield return new WaitForSeconds(skill.castTime);  
        skill.onCast.Invoke();
    }

    private void SummonEnemies()
    {
        bossSummoner.SummonEnemies(); 
    }

}
