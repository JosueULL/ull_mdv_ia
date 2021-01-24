using UnityEngine;

public class FSMStateBase : StateMachineBehaviour
{
    protected GameObject NPC;
    protected AIControl AI;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC = animator.gameObject;
        if (!AI) AI = NPC.GetComponent<AIControl>();
    }
}
