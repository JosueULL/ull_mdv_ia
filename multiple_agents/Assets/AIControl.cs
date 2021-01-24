using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public Transform Player;
    
    Animator mAnimator;
    NavMeshAgent mNavMeshAgent;
    
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        mNavMeshAgent.SetDestination(Player.position);
        mAnimator.SetFloat("Speed", mNavMeshAgent.velocity.magnitude);
    }
}
