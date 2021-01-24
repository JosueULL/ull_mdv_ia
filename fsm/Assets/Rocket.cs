using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float Speed;
    public float Damage;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.collider.GetComponent<Health>();
        if (health)
        {
            health.Damage(Damage);
        }
        Destroy(gameObject);
    }
}
