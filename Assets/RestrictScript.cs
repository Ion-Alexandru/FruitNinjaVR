using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider in the trigger is one of the border cubes
        if (other.CompareTag("Border"))
        {
            // Prevent the main object from moving outside the boundaries
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
