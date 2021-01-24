using UnityEngine;

public class HideFSMState : FSMStateBase
{
    private readonly float HealInterval = 1f;
    private readonly float HealAmount = 0.05f;

    private float mCurrentInterval;
    private Health mHealth;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (!mHealth)
            mHealth = animator.GetComponent<Health>();

        mCurrentInterval = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI.CleverHide();

        mCurrentInterval += Time.deltaTime;
        if (mCurrentInterval > HealInterval)
        {
            mHealth.Regenerate(HealAmount);
            mCurrentInterval = 0;
        }
    }
}
