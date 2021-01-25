using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public Transform Player;
    // Usamos un transform para la linea de vision, lo que nos permite adjuntar un objeto a la cabeza de la armadura del personaje
    // de forma que la visión del personaje es acorde a la animación.
    public Transform SightOrigin;
    public float SightAngle;
    public float SightDistance;

    private Animator mAnimator;
    private bool mCanSeePlayer;
    private NavMeshAgent mNavMeshAgent;
    private Vector3 mPrevPlayerPos;
    private Health mHealth;

    void Awake()
    {
        mHealth = GetComponent<Health>();
        mAnimator = GetComponent<Animator>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }   

    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, Player.position);
        Vector3 dirToPlayer = Player.position - SightOrigin.position;
        dirToPlayer.y = 0;
        float angle = Vector3.Angle(SightOrigin.forward, dirToPlayer);
        mCanSeePlayer = angle < SightAngle && distToPlayer < SightDistance;

        mAnimator.SetFloat("DistanceToPlayer", distToPlayer);
        mAnimator.SetBool("CanSeePlayer", mCanSeePlayer);
        mAnimator.SetFloat("Health", mHealth.CurrentHealth);
        mAnimator.SetFloat("Speed", mNavMeshAgent.velocity.magnitude);
    }

    public void Pursue()
    {
        mNavMeshAgent.stoppingDistance = 3;

        // Se ha cambiado el algoritmo ya que el algoritmo proporcionado no funcionaba correctamente en escenarios
        // - El target puede estar moviendose de forma lateral, por lo que el lookAhead se tiene que aplicar al desplazamiento y no al vector forward
        // - El punto de lookAhead puede estar detrás del enemigo por lo que en ese caso queremos ir directos hacia el player en lugar de retroceder.

        Vector3 dirToTarget = (Player.position - transform.position).normalized;
        Vector3 targetDisplacement = Player.position - mPrevPlayerPos;
        float targetSpeed = targetDisplacement.magnitude / Time.deltaTime;
        targetDisplacement.Normalize();

        float lookAhead = 0;
        if (Vector3.Dot(dirToTarget, targetDisplacement) > -0.5f) // El lookAhead solo se aplica si el personaje está huyendo del enemigo
            lookAhead = Vector3.Distance(Player.position, transform.position) * targetSpeed / mNavMeshAgent.speed;
        
        Seek(Player.position + targetDisplacement * lookAhead);

        mPrevPlayerPos = Player.position;
    }

    public void Seek(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) > mNavMeshAgent.stoppingDistance)
        {
            mNavMeshAgent.SetDestination(position);
        }
        Debug.DrawLine(transform.position, position, Color.yellow);

    }

    public void Stop()
    {
        mNavMeshAgent.isStopped = true;
        mNavMeshAgent.ResetPath();
    }

    public void LookAtTarget()
    {
        Vector3 dirToTarget = Player.position - transform.position;
        dirToTarget.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirToTarget), mNavMeshAgent.angularSpeed * Time.deltaTime);
    }

    Vector3 mWanderTarget = Vector3.zero;
    public void Wander(float radius, float distance, float jitter)
    {
        mNavMeshAgent.stoppingDistance = 1;

        if (mWanderTarget == Vector3.zero)
            mWanderTarget = Vector3.forward * distance;

        mWanderTarget += new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * jitter;
        mWanderTarget.Normalize();
        mWanderTarget *= radius;

        Vector3 wanderLocal = mWanderTarget + Vector3.forward * distance;

        // Esto parece estar mal en los apuntes, es TransformPoint, no InverseTransformPoint
        Vector3 wanderWorld = transform.TransformPoint(wanderLocal); 

        Seek(wanderWorld);
    }

    public void CleverHide()
    {
        mNavMeshAgent.stoppingDistance = 1;

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject[] hidingSpots = World.Instance.GetHidingSpots();
        GameObject chosenGO = hidingSpots[0];

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            GameObject hidenSpot = hidingSpots[i];
            Vector3 hideDir = hidenSpot.transform.position - Player.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = hidenSpot.transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = hidenSpot;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.red);

        Vector3 seekPoint = info.point + chosenDir.normalized;
        Debug.DrawLine(seekPoint, seekPoint + Vector3.up * 10, Color.green);

        Seek(seekPoint);
    }
    
    public void OnDrawGizmos()
    {
        float sightAngleInRadians = Mathf.Deg2Rad * SightAngle;
        const float lineLength = 10;
        Vector3 endSightR = Vector3.RotateTowards(SightOrigin.forward, SightOrigin.right, sightAngleInRadians, 0) * lineLength;
        Vector3 endSightL = Vector3.RotateTowards(SightOrigin.forward, -SightOrigin.right, sightAngleInRadians, 0) * lineLength;
        Gizmos.color = mCanSeePlayer ? Color.green : Color.red;
        Gizmos.DrawLine(SightOrigin.position, SightOrigin.position + endSightR);
        Gizmos.DrawLine(SightOrigin.position, SightOrigin.position + endSightL);
    }
}
