using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject tp;

    void OnTriggerEnter(Collider other)
    {
        tp.transform.position = teleportTarget.transform.position;
    }
}