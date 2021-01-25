using UnityEngine;

public class PatrolFSMState : FSMStateBase
{

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       AI.Wander(5f, 5f, 0.5f);
    }
}
