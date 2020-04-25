using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Kill!");
            // TODO -- ragdoll or something, and then respawn
        }
    }
}
