using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;
    public float MovementSpeed = 1;
    public float RotationSpeed = 1;
    public float MovementThreshold = 1;
    public bool LockY;

    void Update()
    {
        Vector3 dirToTarget = (Target.position - transform.position).normalized;
        Vector3 rotationDir = dirToTarget;
        if (LockY)
            rotationDir.y = 0;
        rotationDir.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationDir), Time.deltaTime * RotationSpeed);

        if (Vector3.Distance(transform.position, Target.position) > MovementThreshold)
        {
            // Usamos dirección en espacio de mundo ya que si bloqueamos Y el personaje no iría hacia el jugador cuando
            // hay un cambio de altura
            transform.Translate(dirToTarget * Time.deltaTime * MovementSpeed, Space.World);
        }
    }
}
