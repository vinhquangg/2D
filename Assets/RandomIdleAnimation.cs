using UnityEngine;

public class RandomIdleAnimation : StateMachineBehaviour
{
    public float minIdleTime = 3f;
    public float maxIdleTime = 7f;
    private float nextChangeTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextChangeTime = Time.time + Random.Range(minIdleTime, maxIdleTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time >= nextChangeTime)
        {
            float randomVariant = Random.Range(1, 3);  // Giá trị 1 hoặc 2
            animator.SetFloat("IdleVariant", randomVariant);
            nextChangeTime = Time.time + Random.Range(minIdleTime, maxIdleTime);
        }
    }
}
