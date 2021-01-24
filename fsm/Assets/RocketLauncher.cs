using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public Transform Origin;
    public GameObject Rocket;

    public void Fire()
    {
        GameObject go = GameObject.Instantiate(Rocket);
        go.transform.position = Origin.position;
        go.transform.rotation = Origin.rotation;
    }
}
